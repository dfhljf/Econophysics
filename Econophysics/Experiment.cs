using System;
using System.Collections.Generic;
using System.Text;

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
        internal static Dictionary<int, Agent> _agents=new Dictionary<int,Agent>();
        /// <summary>
        /// 市场情况
        /// </summary>
        internal static Market _market;
        private static int _index;
        private static int _turn;
        private static ExperimentState _state;
        private static DateTime _startTime;
        private static Random _random = new Random();

        public Experiment(Parameters parameters)
        {
            Parameters = parameters;
            _market = new Market(Parameters.MarketPart);
        }
        public static List<double> GetPriceList()
        {
            return _market.PriceList;
        }
        public static MarketInfo GetMarketInfo()
        {
            return _market.GetInfo();
        }
    }
}
