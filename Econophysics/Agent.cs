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
            _dividend = 0;
            _endowment = init.Endowment;
            _tradeStocks = 0;
        }
        public void Trade(int tradeStocks)
        {
            if (_isTrade)
            {
                throw new Exception(ErrorMessage.TradeTwice);
            }

            if (!updateCash(tradeStocks))
            {
                throw new Exception(ErrorMessage.CashOut);
            }
            if (!updateStocks(tradeStocks))
            {
                throw new Exception(ErrorMessage.InsufficientStocks);
            }
            updateEndowment();
            _tradeStocks = tradeStocks;
            _isTrade = true;
        }
        public void SyncUpdate()
        {
            if (!getDividend())
            {
                throw new Exception(ErrorMessage.Ruin);
            }
            store();
            _tradeStocks = 0;
            _isTrade = false;
            if ((Experiment.Turn % Experiment.Parameters.AgentPart.PeriodOfUpdateDividend) == 0)
            {
                setDividend();
            }
        }
        private void store()
        {

        }
        private bool updateCash(int tradeStocks)
        {
            double tmp = _cash - tradeStocks * Experiment._market._price - Math.Abs(tradeStocks) * Experiment.Parameters.AgentPart.TradeFee;
            if (tmp >= 0)
            {
                _cash = tmp;
                return true;
            }
            return false;
        }
        private bool updateStocks(int tradeStocks)
        {
            int tmp = _stocks + tradeStocks;
            if (tmp >= 0)
            {
                _stocks = tmp;
                return true;
            }
            return false;
        }
        private void updateEndowment()
        {
            _endowment = _cash + _stocks * Experiment._market._price;
        }
        private bool getDividend()
        {
            _cash += _dividend * _stocks;
            while (_cash < 0 && _stocks > 0)
            {
                _cash += Experiment._market._price;
                _stocks--;
            }
            if (_cash < 0)
            {
                return false;
            }
            updateEndowment();
            return true;
        }
        private void setDividend()
        {
            //_dividend = 0;
            //List<double> priceList = Experiment.GetPriceList();
            //Parameters.Market market = Experiment.Parameters.MarketPart;
            //double dividend = Experiment.Parameters.AgentPart.Init.Dividend;
            //if (Experiment._market._state && (priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - market.TimeWindow] == 0 ||
            //    ((priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - market.TimeWindow] > 0) ^ market.Leverage)))
            //{
            //    _dividend = dividend * (Experiment.Random > market.TransP ? (Experiment.Random < market.P ? 1 : -1) :
            //        (Experiment.Random < market.P ? -1 : 1));
            //}
            Market market=Experiment._market;
            List<double> priceList = Experiment._market.PriceList;
            Parameters.Market marketPara = Experiment.Parameters.MarketPart;
            double dividend = Experiment.Parameters.AgentPart.Init.Dividend;
            if (marketPara.Leverage==LeverageEffect.Null)
            {
                if (market._state==MarketState.Active)
                {
                    
                }
            }
        }
    }
}
