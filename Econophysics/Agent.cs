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
        public int Index { get { lock (_lockThis) { return _index; } } }
        /// <summary>
        /// 是否已经交易
        /// </summary>
        public bool IsTrade { get { lock (_lockThis) { return _isTrade; } } }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get { lock (_lockThis) { return _isOnline; } } }
        /// <summary>
        /// 当前代理人的信息
        /// </summary>
        public AgentInfo Now { get { lock (_lockThis) { return _now; } } }
        private AgentInfo _now;
        private AgentIO _agentIO;
        private int _index;
        private bool _isTrade;
        private bool _isOnline;
        private object _lockThis;
        /// <summary>
        /// 登陆
        /// </summary>
        public void Login()
        {
            lock (_lockThis)
            {
                _isOnline = true;
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        public void Logout()
        {
            lock (_lockThis)
            {
                _isOnline = false;
            }
        }
        /// <summary>
        /// 创建一个代理人对象实例
        /// </summary>
        /// <param name="id">代理人编号</param>
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
            _isOnline = false;
            _lockThis = new object();
            _agentIO = new AgentIO();
        }
        /// <summary>
        /// 代理人交易
        /// </summary>
        /// <param name="tradeStocks">交易股票数量，大于0买，小于0卖</param>
        internal void Trade(int tradeStocks)
        {
            lock (_lockThis)
            {

                if (_isTrade)
                {
                    throw ErrorList.TradeTwice;
                }
                if (!updateCash(tradeStocks))
                {
                    throw ErrorList.CashOut;
                }
                if (!updateStocks(tradeStocks))
                {
                    throw ErrorList.InsufficientStocks;
                }
                updateEndowment();
                _now.TradeStocks = tradeStocks;
                _isTrade = true;
            }
        }

        internal void syncUpdate()
        {
            _now.Cash += _now.Dividend * _now.Stocks;
            while (_now.Cash < 0 && _now.Stocks > 0)
            {
                _now.Cash += Experiment.Market.Now.Price;
                _now.Stocks--;
            }
            updateEndowment();
        }
        internal void store()
        {
            _agentIO.Write(getKey(), Now);
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
            double tmp = _now.Cash - tradeStocks * Experiment.Market.Now.Price - Math.Abs(tradeStocks) * Experiment.Parameters.Agent.TradeFee;
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
            _now.Endowment = _now.Cash + _now.Stocks * Experiment.Market.Now.Price;
        }
        private void setDividend()
        {
            Market market = Experiment.Market;
            List<double> priceList = Experiment.Market.PriceList;
            Type.Para.Market marketPara = Experiment.Parameters.Market;
            double dividend = Experiment.Parameters.Agent.Dividend;
            switch (marketPara.Leverage)
            {
                case LeverageEffect.Null:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        ((market.Now.State == MarketState.Active ^ Experiment.Random > marketPara.PDividend) ?
                        ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.Leverage:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        (((market.Now.State == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] <= 0)
                        ^ Experiment.Random > marketPara.PDividend) ? ((Experiment.Random < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.AntiLeverage:
                    _now.Dividend = dividend * ((Experiment.Random < marketPara.TransP) ? (-1) : (1)) *
                        (((market.Now.State == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] >= 0)
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
