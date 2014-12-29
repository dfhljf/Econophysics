using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    static class Experiment
    {
        public static Parameters Parameters;
        public static int Index { get { return _index; } }//实验编号
        public static int Turn { get { return _turn; } }//当前轮数
        public static ExperimentState State { get { return _state; } }
        public static DateTime StartTime { get { return _startTime; } }
        public static double Random { get { return _random.NextDouble(); } }
        internal static Dictionary<int, Agent> _agents=new Dictionary<int,Agent>();
        internal static Market _market=new Market();
        private static int _index;
        private static int _turn;
        private static ExperimentState _state;
        private static DateTime _startTime;
        private static Random _random = new Random();

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
