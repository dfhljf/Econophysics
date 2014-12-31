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
        /// 实验开始时间
        /// </summary>
        public static DateTime StartTime { get { return _startTime; } }
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
        private static DateTime _startTime;
        private static Random _random;
        private static ExperimentIO _experimentIO;

        static Experiment()
        {
            _index = _experimentIO.Read()+1;
            _state = ExperimentState.Unbuilded;
            _random = new Random();
            _agents = new Hashtable();
            _experimentIO = new ExperimentIO(_index);
        }
        public static void Initial(Parameters parameters)
        {
            Parameters = parameters;
            _market = new Market(Parameters.MarketPart);
            _priceGraph = new Graphic(Parameters.GraphicPart.Init);
        }
        public static MarketInfo GetMarketInfo()
        {
            return _market.GetInfo();
        }
        public static bool AddAgent(int id)
        {
            if (_agents.ContainsKey(id))
                return false;
            _agents.Add(id, Parameters.AgentPart.Init);
            return true;
        }
        public static void Trade(int agentId,int tradeStocks)
        {
            if (!_agents.ContainsKey(agentId))
            {
                throw ErrorList.UserNotExist;
            }
        }
    }
}
