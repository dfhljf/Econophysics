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
        internal Agent(int id,AgentInfo init)
        {
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
        internal void Trade(int tradeStocks,double price,double tradeFee)
        {
            lock (_lockThis)
            {

                if (_isTrade)
                {
                    throw ErrorList.TradeTwice;
                }
                if (Now.Stocks+tradeStocks>=0&&!updateCash(tradeStocks,price,tradeFee))
                {
                    throw ErrorList.CashOut;
                }
                if (!updateStocks(tradeStocks))
                {
                    throw ErrorList.InsufficientStocks;
                }
                updateEndowment(price);
                _now.TradeStocks = tradeStocks;
                _isTrade = true;
            }
        }

        internal void syncUpdate(double price)
        {
            _now.Cash += Now.Dividend * Now.Stocks;
            
            while (Now.Cash < 0 && Now.Stocks > 0)
            {
                _now.Cash += price;
                _now.Stocks--;
            }
            _now.Cash = Math.Round(Now.Cash, 1);
            updateEndowment(price);
        }
        internal void store(ExperimentInfo info)
        {
            _agentIO.Write(getKey(info), Now);
        }
        internal void clear(int turn,Market market,Parameters parameters)
        {
            _now.TradeStocks = 0;
            _isTrade = false;
            if ((turn % parameters.Agent.PeriodOfUpdateDividend) == 0)
            {
                setDividend(market,parameters);
            }
        }
        private bool updateCash(int tradeStocks,double price,double tradeFee)
        {
            double tmp = Now.Cash - tradeStocks * price - Math.Abs(tradeStocks) * tradeFee;
            if (tmp >= 0)
            {
                _now.Cash = tmp;
                _now.Cash = Math.Round(Now.Cash, 1);
                return true;
            }
            return false;
        }
        private bool updateStocks(int tradeStocks)
        {
            int tmp = Now.Stocks + tradeStocks;
            if (tmp >= 0)
            {
                _now.Stocks = tmp;
                return true;
            }
            return false;
        }
        private void updateEndowment(double price)
        {
            _now.Endowment = Now.Cash + Now.Stocks * price;
            _now.Endowment = Math.Round(Now.Endowment, 1);
        }
        private void setDividend(Market market,Parameters parameters)
        {
            List<double> priceList = market.PriceList;
            Type.Para.Market marketPara = parameters.Market;
            double dividend = parameters.Agent.Dividend;
            switch (marketPara.Leverage)
            {
                case LeverageEffect.Null:
                    _now.Dividend = dividend * (( Random.GetDouble()< marketPara.TransP) ? (-1) : (1)) *
                        ((market.Now.State == MarketState.Active ^ Random.GetDouble() > marketPara.PDividend) ?
                        ((Random.GetDouble() < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.Leverage:
                    _now.Dividend = dividend * ((Random.GetDouble() < marketPara.TransP) ? (-1) : (1)) *
                        (((market.Now.State == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] <= 0)
                        ^ Random.GetDouble() > marketPara.PDividend) ? ((Random.GetDouble() < marketPara.P) ? (-1) : (1)) : (0));
                    break;
                case LeverageEffect.AntiLeverage:
                    _now.Dividend = dividend * ((Random.GetDouble() < marketPara.TransP) ? (-1) : (1)) *
                        (((market.Now.State == MarketState.Active && priceList[priceList.Count - 1] - priceList[priceList.Count - 1 - marketPara.TimeWindow] >= 0)
                        ^ Random.GetDouble() > marketPara.PDividend) ? ((Random.GetDouble() < marketPara.P) ? (-1) : (1)) : (0));
                    break;
            }
        }
        private AgentKey getKey(ExperimentInfo info)
        {
            AgentKey agentKey;
            agentKey.ExperimentId = info.Index;
            agentKey.Turn = info.Turn;
            agentKey.Id = Index;
            return agentKey;
        }
    }
}
