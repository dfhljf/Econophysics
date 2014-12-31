using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using System.Collections;
using DataIO.Mysql;

namespace Econophysics
{
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
        /// <summary>
        /// 实验状态
        /// </summary>
        public static ExperimentState State { get { return _state; } }
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
        private static bool _isPause;
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
            _state = ExperimentState.Unbuilded;
        }
        public static ExperimentState Initial(Parameters parameters)
        {
            _index = _experimentIO.Read() + 1;
            Parameters = parameters;
            _isPause = false;
            _market = new Market();
            _priceGraph = new Graphic();
            _experimentIO = new ExperimentIO(_index);
            _state = ExperimentState.Builded;
            return _state;
        }
        public static ExperimentState Start(string comments = "")
        {
            if (_state != ExperimentState.Builded)
            {
                throw ErrorList.NotAllowToStart;
            }
            try
            {
                _experimentIO.Write(Parameters, DateTime.Now, comments);
                _turn = 0;
                _state = ExperimentState.Running;
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
                throw ErrorList.NotAllowToPauseOrNext;
            }
            _state = ExperimentState.Suspend;
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
            _state = (_isPause)?ExperimentState.Pause:ExperimentState.Running;
            return _state;
        }
        public static bool Pause()
        {
            _isPause = true;
            return _isPause;
        }
        public static ExperimentState Continue()
        {
            _isPause = false;
            _startTime = DateTime.Now;
            _state = ExperimentState.Running;
            return _state;
        }
        public static ExperimentState Exit()
        {
            _state = ExperimentState.End;
            return _state;
        }
        public static ExperimentState Reset()
        {
            _state = ExperimentState.Unbuilded;
            _agents.Clear();
            return _state;
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
        public static GraphicInfo GetGraphicInfo()
        {
            return _priceGraph.GetInfo();
        }
    }
}
