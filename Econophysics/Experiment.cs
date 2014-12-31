using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using System.Collections;
using DataIO.Mysql;

namespace Econophysics
{
    public delegate void ExperimentStateChangedDelegate(ExperimentState state);
    public delegate void ExperimentNextTurnDelegate(MarketInfo marketInfo);
    static class Experiment
    {
        /// <summary>
        /// 所有可设定参数
        /// </summary>
        public static Parameters Parameters;
        /// <summary>
        /// 实验编号
        /// </summary>
        public static int Index { get { return _index; } }
        /// <summary>
        /// 当前轮数
        /// </summary>
        public static int Turn { get { return _turn; } }
        public static event ExperimentNextTurnDelegate NextTurnReady;
        /// <summary>
        /// 实验状态
        /// </summary>
        public static ExperimentState State { get { return _state; } }
        public static event ExperimentStateChangedDelegate StateChanged;
        /// <summary>
        /// 实验时间，提供倒计时和暂停计时功能
        /// </summary>
        public static int Time 
        { 
            get 
            {
                switch (_state)
                {
                    case ExperimentState.Running:
                        return (Parameters.ExperimentPart.PeriodOfTurn - Convert.ToInt32((DateTime.Now - _startTime).TotalSeconds) - 1);
                    case ExperimentState.Pause:
                        return (Convert.ToInt32((DateTime.Now - _startTime).TotalSeconds));
                    default:
                        return -1;
                }
            } 
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        public static double Random { get { return _random.NextDouble(); } }
        /// <summary>
        /// 所有代理人
        /// </summary>
        internal static Hashtable _agents;
        /// <summary>
        /// 市场情况
        /// </summary>
        internal static Market _market;
        private static Graphic _priceGraph;
        private static int _index;
        private static int _turn;
        private static ExperimentState _state;
        private static Hashtable _pauseList;
        /// <summary>
        /// 关键时间点，每轮开始的时间或者暂停开始的时间
        /// </summary>
        private static DateTime _startTime;
        private static Random _random;
        private static ExperimentIO _experimentIO;

        static Experiment()
        {
            _random = new Random();
            _agents = new Hashtable();
            _pauseList = new Hashtable();
            _state = ExperimentState.Unbuilded;
            stateChanged(_state);
        }
        public static ExperimentState Initial(Parameters parameters,GraphicReadyDelegate _priceGraph_Ready)
        {
            _index = _experimentIO.Read() + 1;
            Parameters = parameters;
            _market = new Market();
            _priceGraph = new Graphic();
            _priceGraph.Ready+=_priceGraph_Ready;
            _experimentIO = new ExperimentIO(_index);
            _state = ExperimentState.Builded;
            stateChanged(_state);
            return _state;
        }
        public static ExperimentState Start(string comments = "")
        {
            if (_state != ExperimentState.Builded)
            {
                return _state;
            }
            try
            {
                _experimentIO.Write(Parameters, DateTime.Now, comments);
                _turn = 0;
                _state = ExperimentState.Running;
                stateChanged(_state);
                NextTurn();
                return _state;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static ExperimentState NextTurn()
        {
            if (_state != ExperimentState.Running)
            {
                return _state;
            }
            _state = ExperimentState.Suspend;
            stateChanged(_state);
            bool notAllowToNext;
            do
            {
                notAllowToNext = false;
                foreach (Agent agent in _agents.Values)
                {
                    notAllowToNext = (agent._isTrading || notAllowToNext);
                }
            } while (notAllowToNext);
            try
            {
                _market.SyncUpdate();
                _priceGraph.Draw();
                if (_turn == Parameters.ExperimentPart.MaxTurn)
                {
                    return Exit();
                }
                _turn++;
            }
            catch (Exception)
            {
                throw;
            }
            _startTime = DateTime.Now;
            _state = (_pauseList.ContainsKey(_turn))?ExperimentState.Pause:ExperimentState.Running;
            stateChanged(_state);
            nextTurnReady(_market.GetInfo());
            return _state;
        }
        public static void AddPause(int turn)
        {
            if (turn <=_turn)
            {
                return;
            }
            if (!_pauseList.ContainsKey(turn))
            {
                _pauseList.Add(turn, null);
            }
        }
        public static void RemovePause(int turn)
        {
            if (_pauseList.ContainsKey(turn))
            {
                _pauseList.Remove(turn);
            }
        }
        public static ExperimentState Continue()
        {
            if (_state!=ExperimentState.Pause)
            {
                return _state;
            }
            _pauseList.Remove(_turn);
            _startTime = DateTime.Now;
            _state = ExperimentState.Running;
            stateChanged(_state);
            return _state;
        }
        public static ExperimentState Exit()
        {
            if (_state!=ExperimentState.Running)
            {
                return _state;
            }
            _state = ExperimentState.End;
            stateChanged(_state);
            return _state;
        }
        public static ExperimentState Reset()
        {
            if (_state!=ExperimentState.End)
            {
                return _state;
            }
            _state = ExperimentState.Unbuilded;
            stateChanged(_state);
            _agents.Clear();
            return _state;
        }
        public static void Recovery()
        {
        }
        public static MarketInfo GetMarketInfo()
        {
            return _market.GetInfo();
        }
        public static bool AddAgent(int id)
        {
            if (_agents.ContainsKey(id))
                return false;
            _agents.Add(id, new Agent(id));
            return true;
        }
        public static void Trade(int agentId, int tradeStocks)
        {
            if (_state != ExperimentState.Running)
            {
                throw ErrorList.ExperimentNotRun;
            }
            if (!_agents.ContainsKey(agentId))
            {
                throw ErrorList.UserNotExist;
            }
            if (Math.Abs(tradeStocks) > Parameters.AgentPart.MaxStock)
            {
                throw ErrorList.TradeStocksOut;
            }
            try
            {
                ((Agent)_agents[agentId]).Trade(tradeStocks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static AgentInfo GetAgentInfo(int id)
        {
            if (!_agents.ContainsKey(id))
            {
                throw ErrorList.UserNotExist;
            }
            return ((Agent)_agents[id]).GetInfo();
        }
        private static void stateChanged(ExperimentState state)
        {
            if (StateChanged != null)
            {
                StateChanged(state);
            }
        }
        private static void nextTurnReady(MarketInfo marketInfo)
        {
            if (NextTurnReady!=null)
            {
                NextTurnReady(marketInfo);
            }
        }
    }
}
