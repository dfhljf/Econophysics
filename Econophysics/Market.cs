using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Econophysics
{
    using Type;
    using DataIO.Mysql;

    internal class Market
    {
        /// <summary>
        /// 价格列表，数量由<see cref="Parameters.Market.Count"></see>确定
        /// </summary>
        internal List<double> PriceList { get { return _priceList; } }
        public MarketInfo Now 
        { 
            get 
            { 
                _now.NumberOfPeople = _agents.Count; 
                return _now; 
            } 
        }
        /// <summary>
        /// 所有代理人
        /// </summary>
        internal static ConcurrentDictionary<int, Agent> _agents;
        private static Graphic _Graph;
        private MarketInfo _now;
        private List<double> _priceList;
        private MarketIO _marketIO;

        internal Market()
        {
            _agents = new ConcurrentDictionary<int, Agent>();
            _Graph = new Graphic();
            MarketInfo init = Experiment.Parameters.Market.Init;
            _now = new MarketInfo
            {
                Price = init.Price,
                Returns = init.Returns,
                NumberOfPeople = init.NumberOfPeople,
                State = init.State,
                AverageEndowment = init.AverageEndowment,
                Volume = init.Volume
            };
            _marketIO = new MarketIO();
            _priceList=new List<double>();
            for(int i=0;i<Experiment.Parameters.Market.Count;i++)
            {
                _priceList.Add(_now.Price);
            }
        }

        internal void SyncUpdate()
        {
            updatePrice();
            updateState();

            foreach (Agent agent in _agents.Values)
            {
                agent.GetDividend();
            }

            getAgentsOrder();
            getAverageEndowment();
            store();
        }

        private void getAgentsOrder()
        {
            var orderedAgents = _agents.OrderByDescending(p => p.Value._now.Endowment);
            int i = 1;
            foreach (var agent in orderedAgents)
            {
                _agents[agent.Key]._now.Order = i++;
            }
        }

        private void getAverageEndowment()
        {
            if (!_agents.IsEmpty)
            {
                _now.AverageEndowment = _agents.Average(p => p.Value._now.Endowment);
            }  
        }
        private void store()
        {
            _now.NumberOfPeople = _agents.Count;
            _marketIO.Write(getKey(),_now);
            foreach (Agent agent in _agents.Values)
            {
                agent.Store();
            }
        }
        private void updatePrice()
        {
            _now.Returns=getReturns();
            _priceList.RemoveAt(0);
            _now.Price = Math.Round(_now.Price * Math.Exp(Experiment.Parameters.Market.Lambda * _now.Returns / (_agents.Count + 1)), 2);
            _priceList.Add(_now.Price);
        }
        private int getReturns()
        {
            int returns = 0;
            foreach (Agent agent in _agents.Values)
            {
                returns += agent._now.TradeStocks;
            }
            return returns;
        }
        private void updateState()
        {
            switch (_now.State)
            {
                case MarketState.Active:
                    _now.State = (Experiment.Random < Experiment.Parameters.Market.P10) ? MarketState.Inactive : MarketState.Active;
                    break;
                case MarketState.Inactive:
                    _now.State = (Experiment.Random < Experiment.Parameters.Market.P01) ? MarketState.Active : MarketState.Inactive;
                    break;
            }
        }
        private MarketKey getKey()
        {
            MarketKey marketKey;
            marketKey.ExperimentId = Experiment.Index;
            marketKey.Turn = Experiment.Turn;
            return marketKey;
        }
    }
}
