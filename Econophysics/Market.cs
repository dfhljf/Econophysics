using CommonType;
using DataIO.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    internal class Market
    {
        /// <summary>
        /// 价格列表，数量由<see cref="Parameters.Market.Count"></see>确定
        /// </summary>
        internal List<double> PriceList { get { return _priceList; } }
        /// <summary>
        /// 本轮价格
        /// </summary>
        internal double _price;
        /// <summary>
        /// 市场状态
        /// </summary>
        internal MarketState _state;
        /// <summary>
        /// 收益率
        /// </summary>
        internal int _returns;
        private List<double> _priceList;
        private MarketIO _marketIO;

        internal Market()
        {
            MarketInfo init = Experiment.Parameters.MarketPart.Init;
            _price = init.Price;
            _state = init.State;
            _returns = init.Returns;
            _marketIO = new MarketIO(Experiment.Index);
            _priceList=new List<double>();
            for(int i=0;i<Experiment.Parameters.MarketPart.Count;i++)
            {
                _priceList.Add(_price);
            }
        }
        internal MarketInfo GetInfo()
        {
            MarketInfo market;
            market.Price = _price;
            market.Returns = _returns;
            market.State = _state;
            market.NumberOfPeople = Experiment._agents.Count;
            return market;
        }
        internal void SyncUpdate()
        {
            updatePrice();
            updateState();
            foreach (Agent agent in Experiment._agents.Values)
            {
                agent.SyncUpdate();
            }
            store();
        }
        private void store()
        {
            _marketIO.Write(Experiment.Turn,GetInfo());
        }
        private void updatePrice()
        {
            _returns=getReturns();
            _priceList.RemoveAt(0);
            _price = Math.Round(_price * Math.Exp(Experiment.Parameters.MarketPart.Lambda * _returns), 2);
            _priceList.Add(_price);
        }
        private int getReturns()
        {
            int returns = 0;
            foreach (Agent agent in Experiment._agents.Values)
            {
                returns += agent._tradeStocks;
            }
            return returns;
        }
        private void updateState()
        {
            switch (_state)
            {
                case MarketState.Active:
                    _state=(Experiment.Random < Experiment.Parameters.MarketPart.P10)?MarketState.Inactive:MarketState.Active;
                    break;
                case MarketState.Inactive:
                    _state=(Experiment.Random < Experiment.Parameters.MarketPart.P01)?MarketState.Active:MarketState.Inactive;
                    break;
            }
        }
    }
}
