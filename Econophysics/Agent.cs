using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using DataIO.Mysql;

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
        internal int _index;
        /// <summary>
        /// 本轮是否交易过，限制一轮交易一次
        /// </summary>
        internal bool _isTrade;
        /// <summary>
        /// 交易股票数目
        /// &gt;0-&gt;buy,&lt;0-&gt;sell,==0->null
        /// </summary>
        internal int _tradeStocks;
        /// <summary>
        /// 正在交易，仅供程序检查是否可以进入下一轮
        /// </summary>
        internal bool _isTrading;
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
        internal double _endowment;
        /// <summary>
        /// 分红数目
        /// </summary>
        private double _dividend;
        private AgentIO _agentIO;

        internal Agent(int id)
        {
            AgentInfo init = Experiment.Parameters.AgentPart.Init;
            _index = id;
            _isTrade = false;
            _isTrading = false;
            _cash = init.Cash;
            _stocks = init.Stocks;
            _dividend = init.Dividend;
            _endowment = init.Endowment;
            _tradeStocks = init.TradeStocks;
            _agentIO = new AgentIO();
        }
        internal AgentInfo GetInfo()
        {
            AgentInfo agentInfo;
            agentInfo.Cash = _cash;
            agentInfo.Dividend = _dividend;
            agentInfo.Endowment = _endowment;
            agentInfo.Stocks = _stocks;
            agentInfo.TradeStocks = _tradeStocks;
            return agentInfo;
        }
        internal void Trade(int tradeStocks)
        {
            _isTrading = true;
            if (_isTrade)
            {
                _isTrading = false;
                throw ErrorList.TradeTwice;
            }
            if (!updateCash(tradeStocks))
            {
                _isTrading = false;
                throw ErrorList.CashOut;
            }
            if (!updateStocks(tradeStocks))
            {
                _isTrading = false;
                throw ErrorList.InsufficientStocks;
            }
            updateEndowment();
            _tradeStocks = tradeStocks;
            _isTrade = true;
            _isTrading = false;
        }
        internal void SyncUpdate()
        {
            getDividend();
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
            _agentIO.Write(getKey(),GetInfo());
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
        private void getDividend()
        {
            _cash += _dividend * _stocks;
            while (_cash < 0 && _stocks > 0)
            {
                _cash += Experiment._market._price;
                _stocks--;
            }
            updateEndowment();
        }
        private void setDividend()
        {
            Market market = Experiment._market;
            List<double> priceList = Experiment._market.PriceList;
            Parameters.Market marketPara = Experiment.Parameters.MarketPart;
            double dividend = Experiment.Parameters.AgentPart.Init.Dividend;
            switch (marketPara.Leverage)
            {
                case LeverageEffect.Null:
                    _dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        ((market._state == MarketState.Active ^ Experiment.Random > marketPara.PDividend) ?
                        ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.Leverage:
                    _dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        (((market._state == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] <= 0)
                        ^ Experiment.Random > marketPara.PDividend) ? ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.AntiLeverage:
                    _dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        (((market._state == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] >= 0)
                        ^ Experiment.Random > marketPara.PDividend) ? ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
            }
        }
        private AgentKey getKey()
        {
            AgentKey agentKey;
            agentKey.ExperimentId = Experiment.Index;
            agentKey.Turn = Experiment.Turn;
            agentKey.Id = _index;
            return agentKey;
        }
    }
}
