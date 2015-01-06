using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using System.Collections;
using DataIO.Mysql;

namespace Econophysics
{
    /// <summary>
    /// 实验状态改变触发的事件委托
    /// </summary>
    /// <param name="state">当前的事件状态</param>
    public delegate void ExperimentStateChangedDelegate(ExperimentState state);
    /// <summary>
    /// 开始新一轮实验触发的事件委托
    /// </summary>
    /// <param name="marketInfo">当前的市场信息</param>
    public delegate void ExperimentNextTurnDelegate(MarketInfo marketInfo);
    /// <summary>
    /// 图像准备好触发的事件委托
    /// </summary>
    /// <param name="graphicInfo">图像信息</param>
    public delegate void GraphicReadyDelegate(GraphicInfo graphicInfo);
    public static class Experiment
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
        /// 当前在线人数
        /// </summary>
        public static int NumberOfAgents { get { return _agents.Count; } }
        /// <summary>
        /// 暂停列表
        /// </summary>
        public static Hashtable PauseList { get { return _pauseList; } }
        /// <summary>
        /// 实验状态
        /// </summary>
        public static ExperimentState State { get { return _state; } }
        /// <summary>
        /// 实验时间，提供倒计时和暂停计时功能
        /// </summary>
        public static int TimeTick
        {
            get
            {
                switch (_state)
                {
                    case ExperimentState.Running:
                    case ExperimentState.Suspend:
                    case ExperimentState.Pause:
                        return _timeTick;
                    default:
                        return -1;
                }
            }
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        public static double Random { get { return _random.NextDouble(); } }
        public static event GraphicReadyDelegate GraphicReady;
        public static event ExperimentNextTurnDelegate NextTurnReady;
        public static event ExperimentStateChangedDelegate StateChanged;
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
        private static Random _random;
        private static ExperimentIO _experimentIO;
        private static int _timeTick;

        static Experiment()
        {
            _agents = new Hashtable();
            _pauseList = new Hashtable();
            _experimentIO = new ExperimentIO();
            _state = ExperimentState.Unbuilded;
            stateChanged(_state);
        }
        public static ExperimentState Build(Parameters parameters,int expId=0)
        {
            if (_state!=ExperimentState.Unbuilded)
            {
                return _state;
            }
            _random = new Random();
            _index = _experimentIO.Read() + 1;
            Parameters = parameters;
            _market = new Market();
            _priceGraph = new Graphic();
            _timeTick = Parameters.ExperimentPart.PeriodOfTurn;
            _turn = parameters.ExperimentPart.StartTurn;
            if (expId != 0)
            {
                _index = expId;
                Hashtable mht=_experimentIO.Read(string.Format("select * from market where expId={0} order by turn desc limit {1}",expId,parameters.MarketPart.Count));
                foreach (MarketInfo mi in mht.Values)
                {
                    _market.PriceList.Insert(0, mi.Price);
                }
            }
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
                store(comments);
                _state = ExperimentState.Running;
                stateChanged(_state);
                nextTurn();
                return _state;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static ExperimentState SetTimeTick()
        {
            switch (_state)
            {
                case ExperimentState.Running:
                    _timeTick--;
                    if (_timeTick == 0)
                    {
                        nextTurn();
                    }
                    return _state;
                case ExperimentState.Pause:
                    _timeTick++;
                    return _state;
                default:
                    return _state;
            }
        }
        public static bool AddPause(int turn)
        {
            if (_state==ExperimentState.Unbuilded)
            {
                return false;
            }
            if (turn <= _turn)
            {
                return false;
            }
            if (!_pauseList.ContainsKey(turn))
            {
                _pauseList.Add(turn, null);
                return true;
            }
            return false;
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
            if (_state != ExperimentState.Pause)
            {
                return _state;
            }
            RemovePause(_turn);
            _timeTick = Parameters.ExperimentPart.PeriodOfTurn - 1;
            //_startTime = DateTime.Now;
            _state = ExperimentState.Running;
            stateChanged(_state);
            return _state;
        }
        public static ExperimentState Reset()
        {
            if (_state != ExperimentState.End)
            {
                return _state;
            }
            _state = ExperimentState.Unbuilded;
            stateChanged(_state);
            _agents.Clear();
            _pauseList.Clear();

            return _state;
        }
        public static void Recovery()
        {
        }
        public static Hashtable List()
        {
            Hashtable eht = _experimentIO.Read("select * from parameters");
            Hashtable rtn = new Hashtable();
            foreach (int expId in eht.Keys)
            {
                Parameters para = (Parameters)eht[expId];
                Hashtable aht=_experimentIO.Read(string.Format("select * from Agents where Turn=0 and ExperimentId={0} limit 1", expId));
                foreach (AgentKey ak in aht.Keys)
                {
                    para.AgentPart.Init = (AgentInfo)aht[ak];
                }

                Hashtable mht=_experimentIO.Read(string.Format("select * from market where ExperimentId={0} order by turn desc limit 1", expId));
                foreach (MarketKey mk in mht.Keys)
                {
                    para.MarketPart.Init = (MarketInfo)mht[mk];
                }
                rtn.Add(expId, para);
            }
            return rtn;
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
            if (NextTurnReady != null)
            {
                NextTurnReady(marketInfo);
            }
        }
        private static void graphicReady(GraphicInfo graphicInfo)
        {
            if (GraphicReady != null)
            {
                GraphicReady(graphicInfo);
            }
        }
        private static void store(string comments)
        {
            _experimentIO.Write(_index, Parameters, comments);
        }
        private static void nextTurn()
        {
            if (_state != ExperimentState.Running)
            {
                return;
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
            _market.SyncUpdate();
            _priceGraph.Draw();
            graphicReady(_priceGraph.getInfo());
            if (_turn == Parameters.ExperimentPart.MaxTurn)
            {
                exit();
                return;
            }
            _turn++;
            //_startTime = DateTime.Now;
            _timeTick = (_pauseList.ContainsKey(_turn)) ? 0 : Parameters.ExperimentPart.PeriodOfTurn;
            _state = (_pauseList.ContainsKey(_turn)) ? ExperimentState.Pause : ExperimentState.Running;
            stateChanged(_state);
            nextTurnReady(_market.GetInfo());
        }
        private static void exit()
        {
            if (_state != ExperimentState.Suspend)
            {
                return;
            }
            _state = ExperimentState.End;
            stateChanged(_state);
        }
    }
}
