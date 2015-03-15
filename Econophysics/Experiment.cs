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
        public static Parameters Parameters { get { return _parameters; } }
        /// <summary>
        /// 所有可恢复的历史记录
        /// </summary>
        public static Dictionary<int, Parameters> Histories { get { return _histories; } }
        public static ExperimentInfo Now { get { return _now; } }
        /// <summary>
        /// 暂停列表
        /// </summary>
        public static Hashtable PauseList { get { return _pauseList; } }
        /// <summary>
        /// 实验时间，提供倒计时和暂停计时功能
        /// </summary>
        public static int TimeTick
        {
            get
            {
                switch (Now.State)
                {
                    case ExperimentState.Running:
                    case ExperimentState.Suspend:
                    case ExperimentState.Pause:
                        return _timeTick == 0 ? 1 : _timeTick;
                    default:
                        return -1;
                }
            }
        }
        /// <summary>
        /// 实验市场
        /// </summary>
        public static Market Market { get { return _market; } }

        private static Market _market;
        private static Graphic _graph;
        private static ExperimentInfo _now;
        private static Hashtable _pauseList;
        private static ExperimentIO _experimentIO;
        private static int _timeTick;
        private static Timer _timer;
        private static Parameters _parameters;
        private static Dictionary<int, Parameters> _histories;
        private static bool _isForceExit;
        private static object _lockThis = new object();

        static Experiment()
        {
            _pauseList = new Hashtable();
            _experimentIO = new ExperimentIO();
            _now = new ExperimentInfo();
            _now.State = ExperimentState.Unbuilded;
            getHistories();
            _isForceExit = false;
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
            if (Now.State != ExperimentState.Unbuilded)
            {
                return Now.State;
            }
            if (expId == 0)// 新建实验
            {
                newExperiment(parameters);
            }
            else// 还原实验
            {
                recovery(expId, parameters);
            }

            return Now.State;
        }
        /// <summary>
        /// 开始实验
        /// </summary>
        /// <returns>实验状态<see cref="Econophysics.Type.ExperimentState"/></returns>
        public static ExperimentState Start()
        {
            if (Now.State != ExperimentState.Builded)
            {
                return Now.State;
            }
            try
            {
                //if(Market.Now.NumberOfPeople==0)
                //{
                //    throw ErrorList.NotExistUsers;
                //}

                store();
                _now.State = ExperimentState.Running;
                nextTurn();
                _timer.Start();
                return Now.State;
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
            if (Now.State == ExperimentState.Unbuilded)
            {
                return false;
            }
            if (turn < Now.Turn)
            {
                return false;
            }
            if (!PauseList.ContainsKey(turn))
            {
                PauseList.Add(turn, null);
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
            if (PauseList.ContainsKey(turn))
            {
                PauseList.Remove(turn);
            }
        }
        /// <summary>
        /// 继续实验
        /// </summary>
        /// <returns></returns>
        public static ExperimentState Continue()
        {
            if (Now.State != ExperimentState.Pause)
            {
                return Now.State;
            }
            RemovePause(Now.Turn);
            _timeTick = Parameters.Experiment.PeriodOfTurn - 1;
            _now.Turn++;
            _now.State = ExperimentState.Running;
            return Now.State;
        }
        /// <summary>
        /// 重置实验
        /// </summary>
        /// <returns></returns>
        public static ExperimentState Reset()
        {
            if (Now.State != ExperimentState.End)
            {
                return Now.State;
            }
            _now.State = ExperimentState.Unbuilded;
            PauseList.Clear();
            _isForceExit = false;
            return Now.State;
        }
        /// <summary>
        /// 强制退出
        /// </summary>
        public static void ForceExit()
        {
            _isForceExit = true;
        }
        /// <summary>
        /// 添加代理人
        /// </summary>
        /// <param name="id">代理人编号</param>
        public static void AddAgent(int id)
        {
            if (Now.State == ExperimentState.Unbuilded || Now.State == ExperimentState.End)
            {
                throw ErrorList.NotAllowLogin;
            }
            if (Market.Agents.ContainsKey(id))
            {
                Market.Agents[id].Login();
            }
            try
            {
                Market.Agents.TryAdd(id, new Agent(id, Parameters.Agent.Init));
                Market.Agents[id].Login();
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 添加机器人
        /// </summary>
        /// <param name="count">数量</param>
        public static void AddAndroids(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int tmp = 256 + Market.Now.NumberOfAndroids;
                Market.Androids.TryAdd(tmp, new Android(tmp, Parameters.Agent.Init));
            }
        }
        /// <summary>
        /// 交易
        /// </summary>
        /// <param name="agentId">代理人编号</param>
        /// <param name="tradeStocks">交易数量</param>
        public static void Trade(int agentId, int tradeStocks)
        {
            if (Now.State != ExperimentState.Running)
            {
                throw ErrorList.ExperimentNotRun;
            }
            if (!Market.Agents.ContainsKey(agentId))
            {
                throw ErrorList.UserNotExist;
            }
            if (Math.Abs(tradeStocks) > Parameters.Agent.MaxStock)
            {
                throw ErrorList.TradeStocksOut;
            }
            try
            {
                Market.Agents[agentId].Trade(tradeStocks, Market, Parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void setTimeTick(object sender, ElapsedEventArgs e)
        {
            //lock
            lock (_lockThis)
            {
                switch (Now.State)
                {
                    case ExperimentState.Running:
                        _timeTick--;
                        if (_timeTick == 0)
                        {
                            nextTurn();
                        }
                        break;
                    case ExperimentState.Pause:
                        if (_isForceExit)
                        {
                            exit();
                            return;
                        }
                        _timeTick++;
                        break;
                    default:
                        break;
                }
            }

        }
        private static void store()
        {
            _experimentIO.Write(Now.Index, Parameters);
        }
        private static void nextTurn()
        {
            _now.State = ExperimentState.Suspend;
            Market.SyncUpdate(Now, Parameters);
            _graph.Draw();
            if (_isForceExit || Now.Turn == Parameters.Experiment.MaxTurn)
            {
                exit();
                return;
            }
            bool isPause = PauseList.ContainsKey(Now.Turn);
            _timeTick = isPause ? 0 : Parameters.Experiment.PeriodOfTurn;
            _now.State = isPause ? ExperimentState.Pause : ExperimentState.Running;
            _now.Turn = isPause ? Now.Turn : Now.Turn + 1;
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
                Histories.Add(expId, para);
            }
        }
        private static void newExperiment(Parameters parameters)
        {
            _now.Index = _experimentIO.Read() + 1;
            _parameters = parameters;
            _now.State = ExperimentState.Builded;
            _market = new Market(parameters.Market.Init, parameters.Market.Count);
            _graph = new Graphic(parameters.Graphic.Init);
            _timeTick = Parameters.Experiment.PeriodOfTurn;
            _now.Turn = Parameters.Experiment.StartTurn;
            // 清除上次的图像
            if (File.Exists(Parameters.Graphic.Init.Url))
            {
                File.Delete(Parameters.Graphic.Init.Url);
            }
        }
        private static void recovery(int expId, Parameters parameters)
        {
            _now.Index = expId;
            _parameters = Histories[expId];
            _parameters.Graphic.Init = parameters.Graphic.Init;
            _parameters.Graphic.Init.Count = parameters.Market.Count;
            _parameters.Experiment = parameters.Experiment;
            _now.State = ExperimentState.Pause;
            List<double> priceList = new List<double>();
            Hashtable mht = _experimentIO.Read(string.Format("select * from market where ExperimentId={0} order by turn desc limit {1}", expId, Parameters.Market.Count));

            foreach (MarketInfo mi in mht.Values)
            {
                priceList.Insert(0, mi.Price);
            }
            _market = new Market(Parameters.Market.Init, Parameters.Market.Count, priceList);
            _graph = new Graphic(Parameters.Graphic.Init);
            _graph.Draw();
            _timeTick = 0;
            _now.Turn = Parameters.Experiment.StartTurn;
            recoveryAgents(expId, Now.Turn);
            _timer.Start();
        }

        private static void recoveryAgents(int expId, int turn)
        {
            Hashtable aht = _experimentIO.Read(string.Format("select * from agents where ExperimentId={0} and Turn={1} and Id<=255", expId, turn));
            foreach (AgentKey agent in aht.Keys)
            {
                Market.Agents.TryAdd(agent.Id, new Agent(agent.Id, (AgentInfo)aht[agent]));
            }
            aht = _experimentIO.Read(string.Format("select * from agents where ExperimentId={0} and Turn={1} and Id>255", expId, turn));
            foreach (AgentKey android in aht.Keys)
            {
                Market.Androids.TryAdd(android.Id, new Android(android.Id, (AgentInfo)aht[android]));
            }
        }
        private static void exit()
        {
            _now.State = ExperimentState.End;
            _timer.Stop();
        }

    }
}
