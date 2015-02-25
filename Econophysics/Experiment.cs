using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;

namespace Econophysics
{
    using Type;
    using DataIO.Mysql;


    /// <summary>
    /// 实验静态类
    /// </summary>
    public static class Experiment
    {
        /// <summary>
        /// 所有可设定参数
        /// </summary>
        public static Parameters Parameters { get {return _parameters;} }
        /// <summary>
        /// 所有可恢复的历史记录
        /// </summary>
        public static Dictionary<int, Parameters> Histories { get { return _histories; } }
        /// <summary>
        /// 实验编号
        /// </summary>
        public static int Index { get { return _index; } }
        /// <summary>
        /// 当前轮数
        /// </summary>
        public static int Turn { get { return _turn; } }
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
        /// <summary>
        /// 实验市场
        /// </summary>
        public static Market Market { get { return _market; } }

        private static Market _market;
        private static int _index;
        private static int _turn;
        private static ExperimentState _state;
        private static Hashtable _pauseList;
        private static Random _random;
        private static ExperimentIO _experimentIO;
        private static int _timeTick;
        private static Timer _timer;
        private static Parameters _parameters;
        private static Dictionary<int, Parameters> _histories;

        static Experiment()
        {
            _pauseList = new Hashtable();
            _experimentIO = new ExperimentIO();
            _state = ExperimentState.Unbuilded;
            _random = new Random();
            getHistories();

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += setTimeTick;
        }
        /// <summary>
        /// 建立实验
        /// </summary>
        /// <param name="expId">实验编号，0表示新建实验</param>
        /// <param name="parameters">实验参数</param>
        /// <returns>实验状态<see cref="Econophysics.Type.ExperimentState"/></returns>
        public static ExperimentState Build(int expId = 0, Parameters parameters = new Parameters())
        {
            if (_state!=ExperimentState.Unbuilded)
            {
                return _state;
            }
            if (expId==0)// 新建实验
            {
                newExperiment(parameters);
            }
            else// 还原实验
            {
                recovery(expId,parameters);
            }
            // 清除上次的图像
            if (File.Exists(Parameters.Graphic.Init.Url))
            {
                File.Delete(Parameters.Graphic.Init.Url);
            }

            return _state;
        }
        /// <summary>
        /// 开始实验
        /// </summary>
        /// <returns>实验状态<see cref="Econophysics.Type.ExperimentState"/></returns>
        public static ExperimentState Start()
        {
            if (_state != ExperimentState.Builded)
            {
                return _state;
            }
            try
            {
                //if(Market.Now.NumberOfPeople==0)
                //{
                //    throw ErrorList.NotExistUsers;
                //}

                store();
                _state = ExperimentState.Running;
                nextTurn();
                _timer.Start();
                return _state;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 添加暂停
        /// </summary>
        /// <param name="turn">在本轮开始时暂停</param>
        /// <returns>是否成功</returns>
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
        /// <summary>
        /// 移除暂停
        /// </summary>
        /// <param name="turn">移除在该轮次的暂停</param>
        public static void RemovePause(int turn)
        {
            if (_pauseList.ContainsKey(turn))
            {
                _pauseList.Remove(turn);
            }
        }
        /// <summary>
        /// 继续实验
        /// </summary>
        /// <returns></returns>
        public static ExperimentState Continue()
        {
            if (_state != ExperimentState.Pause)
            {
                return _state;
            }
            RemovePause(_turn);
            _timeTick = Parameters.Experiment.PeriodOfTurn - 1;
            _state = ExperimentState.Running;
            return _state;
        }
        /// <summary>
        /// 重置实验
        /// </summary>
        /// <returns></returns>
        public static ExperimentState Reset()
        {
            if (_state != ExperimentState.End)
            {
                return _state;
            }
            _state = ExperimentState.Unbuilded;
            _pauseList.Clear();

            return _state;
        }        
        /// <summary>
        /// 添加代理人
        /// </summary>
        /// <param name="id">代理人编号</param>
        public static void AddAgent(int id)
        {
            if (_state==ExperimentState.Unbuilded||_state==ExperimentState.End)
            {
                throw ErrorList.NotAllowLogin;
            }
            if (Market.Agents.ContainsKey(id))
            {
                Market.Agents[id].Login();
            }
            try
            {
                   _market.Agents.TryAdd(id, new Agent(id));
                   Market.Agents[id].Login();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        /// <summary>
        /// 交易
        /// </summary>
        /// <param name="agentId">代理人编号</param>
        /// <param name="tradeStocks">交易数量</param>
        public static void Trade(int agentId, int tradeStocks)
        {
            if (_state != ExperimentState.Running)
            {
                throw ErrorList.ExperimentNotRun;
            }
            if (!_market.Agents.ContainsKey(agentId))
            {
                throw ErrorList.UserNotExist;
            }
            if (Math.Abs(tradeStocks) > Parameters.Agent.MaxStock)
            {
                throw ErrorList.TradeStocksOut;
            }
            try
            {
                _market.Agents[agentId].Trade(tradeStocks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void setTimeTick(object sender, ElapsedEventArgs e)
        {
            switch (_state)
            {
                case ExperimentState.Running:
                    _timeTick--;
                    if (_timeTick == 0)
                    {
                        nextTurn();
                    }
                    break;
                case ExperimentState.Pause:
                    _timeTick++;
                    break;
                default:
                    break;
            }
        }
        private static void store()
        {
            _experimentIO.Write(_index, Parameters);
        }
        private static void nextTurn()
        {
            _state = ExperimentState.Suspend;
            _market.SyncUpdate();
            if (_turn == Parameters.Experiment.MaxTurn)
            {
                exit();
                return;
            }
            _turn++;
            _timeTick = (_pauseList.ContainsKey(_turn)) ? 0 : Parameters.Experiment.PeriodOfTurn;
            _state = (_pauseList.ContainsKey(_turn)) ? ExperimentState.Pause : ExperimentState.Running;
        }
        private static void getHistories()
        {
            _histories = new Dictionary<int, Type.Parameters>();
            Hashtable eht = _experimentIO.Read("select * from parameters");
            foreach (int expId in eht.Keys)
            {
                Parameters para = (Parameters)eht[expId];
                Hashtable aht = _experimentIO.Read(string.Format("select * from Agents where Turn=0 and ExperimentId={0} limit 1", expId));
                foreach (AgentKey ak in aht.Keys)
                {
                    para.Agent.Init = (AgentInfo)aht[ak];
                }
                
                Hashtable mht = _experimentIO.Read(string.Format("select * from market where ExperimentId={0} order by turn desc limit 1", expId));
                foreach (MarketKey mk in mht.Keys)
                {
                    para.Market.Init = (MarketInfo)mht[mk];
                    para.Experiment.StartTurn = mk.Turn;
                }
                _histories.Add(expId, para);
            }
        }
        private static void newExperiment(Parameters parameters)
        {
            _index = _experimentIO.Read() + 1;
            _parameters = parameters;
            _state = ExperimentState.Builded;
            _market = new Market();
            _timeTick = _parameters.Experiment.PeriodOfTurn;
            _turn = _parameters.Experiment.StartTurn;
        }
        private static void recovery(int expId,Parameters parameters)
        {
            _index = expId;
            _parameters = _histories[expId];
            _parameters.Graphic.Init = parameters.Graphic.Init;
            _state = ExperimentState.Pause;
            _market = new Market();
            _market.PriceList.Clear();
            Hashtable mht = _experimentIO.Read(string.Format("select * from market where ExperimentId={0} order by turn desc limit {1}", expId, _parameters.Market.Count));
            foreach (MarketInfo mi in mht.Values)
            {
                _market.PriceList.Insert(0, mi.Price);
            }
            int tmpc = _market.PriceList.Count;
            double tmp = _market.PriceList[0];
            for (int i = 0; i < _parameters.Market.Count-tmpc; i++)
            {
                _market.PriceList.Insert(0, tmp);
            }
            _timeTick = 0;
            _turn = _parameters.Experiment.StartTurn;
            _turn++;
            _timer.Start();
        }
        private static void exit()
        {
            _state = ExperimentState.End;
            _timer.Stop();
        }
    
    }
}
