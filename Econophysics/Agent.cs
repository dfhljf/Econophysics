using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    using Type;
    using DataIO.Mysql;
    /// <summary>
    /// 代理人模型
    /// </summary>
    public class Agent
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
        /// 正在交易，仅供程序检查是否可以进入下一轮
        /// </summary>
        internal bool _isTrading;
        /// <summary>
        /// 当前代理人的信息
        /// </summary>
        public AgentInfo Now { get { return _now; } }
        internal AgentInfo _now;
        private AgentIO _agentIO;

        internal Agent(int id)
        {
            AgentInfo init = Experiment.Parameters.Agent.Init;
            _now = new AgentInfo 
            { 
                Cash = init.Cash,
                Stocks = init.Stocks,
                Endowment = init.Endowment,
                Order = init.Order,
                Dividend = init.Dividend,
                TradeStocks = init.TradeStocks 
            };
            
            _index = id;
            _isTrade = false;
            _isTrading = false;
            _agentIO = new AgentIO();
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
            _now.TradeStocks = tradeStocks;
            _isTrade = true;
            _isTrading = false;
        }
        internal void GetDividend()
        {
            _now.Cash += _now.Dividend * _now.Stocks;
            while (_now.Cash < 0 && _now.Stocks > 0)
            {
                _now.Cash += Experiment._market._price;
                _now.Stocks--;
            }
            updateEndowment();
        }
        internal void Store()
        {
            _agentIO.Write(getKey(),_now);
            clear();
        }
        private void clear()
        {
            _now.TradeStocks = 0;
            _isTrade = false;
            if ((Experiment.Turn % Experiment.Parameters.Agent.PeriodOfUpdateDividend) == 0)
            {
                setDividend();
            }
        }
        private bool updateCash(int tradeStocks)
        {
            double tmp = _now.Cash - tradeStocks * Experiment._market._price - Math.Abs(tradeStocks) * Experiment.Parameters.Agent.TradeFee;
            if (tmp >= 0)
            {
                _now.Cash = tmp;
                return true;
            }
            return false;
        }
        private bool updateStocks(int tradeStocks)
        {
            int tmp = _now.Stocks + tradeStocks;
            if (tmp >= 0)
            {
                _now.Stocks = tmp;
                return true;
            }
            return false;
        }
        private void updateEndowment()
        {
            _now.Endowment = _now.Cash + _now.Stocks * Experiment._market._price;
        }
        
        private void setDividend()
        {
            Market market = Experiment._market;
            List<double> priceList = Experiment._market.PriceList;
            Type.Para.Market marketPara = Experiment.Parameters.Market;
            double dividend = Experiment.Parameters.Agent.Dividend;
            switch (marketPara.Leverage)
            {
                case LeverageEffect.Null:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        ((market._state == MarketState.Active ^ Experiment.Random > marketPara.PDividend) ?
                        ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.Leverage:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        (((market._state == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] <= 0)
                        ^ Experiment.Random > marketPara.PDividend) ? ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.AntiLeverage:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
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
