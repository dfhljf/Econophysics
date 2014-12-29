using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    /// <summary>
    /// 代理人模型
    /// </summary>
    internal class Agent
    {
        /// <summary>
        /// 编号，每个代理人都有唯一的编号
        /// </summary>
        public int Index { get { return _index; } }
        /// <summary>
        /// 本轮是否交易过，限制一轮交易一次
        /// </summary>
        public bool IsTrade { get { return _isTrade; } }
        /// <summary>
        /// 现金数量
        /// </summary>
        private double _cash;
        /// <summary>
        /// 股票数目
        /// </summary>
        private int _stocks;
        /// <summary>
        /// 当前的总资产=股票数目*市场价格+现金
        /// </summary>
        private double _endowment;
        /// <summary>
        /// 分红数目
        /// </summary>
        private double _dividend;
        /// <summary>
        /// 交易股票数目
        /// >0->buy,<0->sell,==0->null
        /// </summary>
        private int _tradeStocks;
        private bool _isTrade;
        private int _index;

        public Agent(int index, AgentInfo init)
        {
            _index = index;
            _isTrade = false;
            _cash = init.Cash;
            _stocks = init.Stocks;
            _endowment = init.Endowment;
            getDividend();
            _tradeStocks = 0;
        }
        public bool Trade(int tradeStocks)
        {
            if (_isTrade)
            {
                throw 
                return false;
            }

        }

        private void getDividend()
        {
            _dividend=0;
            List<double> priceList = Experiment.GetPriceList();
            MarketInfo marketInfo = Experiment.GetMarketInfo();
            Parameters.Market market = Experiment.Parameters.MarketPart;
            double dividend = Experiment.Parameters.AgentPart.Init.Dividend;
            if (marketInfo.State && (priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - market.TimeWindow] == 0 ||
                ((priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - market.TimeWindow] > 0) ^ market.Leverage)))
            {
                _dividend = dividend * (Experiment.Random > market.TransP ? (Experiment.Random < market.P ? 1 : -1):
                    (Experiment.Random < market.P ? -1 : 1));
            }
        }
    }
}
