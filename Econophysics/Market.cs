using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Econophysics
{
    using Type;
    using DataIO.Mysql;
    /// <summary>
    /// 市场
    /// </summary>
    public class Market
    {
        /// <summary>
        /// 当前市场状态
        /// </summary>
        public MarketInfo Now
        {
            get
            {
                _now.NumberOfPeople = _agents.Count;
                return _now;
            }
        }
        /// <summary>
        /// 市场中的代理人
        /// </summary>
        public ConcurrentDictionary<int, Agent> Agents { get { return _agents; } }
        /// <summary>
        /// 价格列表，数量由<see cref="Type.Para.Market.Count"/>确定
        /// </summary>
        internal List<double> PriceList { get { return _priceList; } }
        internal ConcurrentDictionary<int, Agent> _agents;
        private Graphic _graph;
        private MarketInfo _now;
        private List<double> _priceList;
        private MarketIO _marketIO;

        internal Market()
        {
            _agents = new ConcurrentDictionary<int, Agent>();
            _graph = new Graphic();
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
            updateMarket();

            getAgentsOrder();
            getAverageEndowment();
            store();
            _graph.Draw();
        }

        private void getAgentsOrder()
        {
            var orderedAgents = _agents.OrderByDescending(p => p.Value.Now.Endowment);
            int i = 1;
            foreach (var agent in orderedAgents)
            {
                _agents[agent.Key].Now.Order = i++;
            }
        }

        private void getAverageEndowment()
        {
            if (!_agents.IsEmpty)
            {
                _now.AverageEndowment = _agents.Average(p => p.Value.Now.Endowment);
            }  
        }
        private void store()
        {
            _now.NumberOfPeople = _agents.Count;
            _marketIO.Write(getKey(),Now);
            foreach (Agent agent in _agents.Values)
            {
                agent.store();
            }
        }
        private void updateMarket()
        {
            _now.Returns = _agents.Sum(p => p.Value.Now.TradeStocks);
            _now.Volume = _agents.Sum(p => Math.Abs(p.Value.Now.TradeStocks));
            _priceList.RemoveAt(0);
            _now.Price = Math.Round(_now.Price * Math.Exp(Experiment.Parameters.Market.Lambda * _now.Returns / _agents.Count), 2);
            _priceList.Add(_now.Price);
            updateState();
            foreach (Agent agent in _agents.Values)
            {
                agent.syncUpdate();
            }
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
