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
        public AgentInfo Now { get { return _now; } }
        private AgentInfo _now;
        private AgentIO _agentIO;

        internal Agent(int id)
        {
            
            AgentInfo init = Experiment.Parameters.Agent.Init;
            _now = new AgentInfo();
            _now
            _index = id;
            _isTrade = false;
            _isTrading = false;
            _now
            _agentIO = new AgentIO();
        }
        internal AgentInfo GetInfo()
        {
            AgentInfo agentInfo=new AgentInfo();
            
            agentInfo.Cash = _cash;
            agentInfo.Dividend = _dividend;
            agentInfo.Endowment = _endowment;
            agentInfo.Stocks = _stocks;
            agentInfo.TradeStocks = _tradeStocks;
            agentInfo.Order = _order;
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
        internal void GetDividend()
        {
            _cash += _dividend * _stocks;
            while (_cash < 0 && _stocks > 0)
            {
                _cash += Experiment._market._price;
                _stocks--;
            }
            updateEndowment();
        }
        internal void Store()
        {
            _agentIO.Write(getKey(),GetInfo());
            clear();
        }
        private void clear()
        {
            _tradeStocks = 0;
            _isTrade = false;
            if ((Experiment.Turn % Experiment.Parameters.Agent.PeriodOfUpdateDividend) == 0)
            {
                setDividend();
            }
        }
        private bool updateCash(int tradeStocks)
        {
            double tmp = _cash - tradeStocks * Experiment._market._price - Math.Abs(tradeStocks) * Experiment.Parameters.Agent.TradeFee;
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
        
        private void setDividend()
        {
            Market market = Experiment._market;
            List<double> priceList = Experiment._market.PriceList;
            Para.Market marketPara = Experiment.Parameters.Market;
            double dividend = Experiment.Parameters.Agent.Init.Dividend;
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
