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
                _now.NumberOfPeople = Agents.Count(p=>p.Value.IsOnline);
                _now.NumberOfAndroids = Androids.Count;
                return _now;
            }
        }
        /// <summary>
        /// 市场中的代理人
        /// </summary>
        public ConcurrentDictionary<int, Agent> Agents { get { return _agents; } }
        public ConcurrentDictionary<int, Android> Androids { get { return _androids; } }
        /// <summary>
        /// 价格列表，数量由<see cref="Type.Para.Market.Count"/>确定
        /// </summary>
        internal List<double> PriceList { get { return _priceList; } }
        private ConcurrentDictionary<int, Agent> _agents;
        private ConcurrentDictionary<int, Android> _androids;
        private MarketInfo _now;
        private List<double> _priceList;
        private MarketIO _marketIO;

        internal Market(MarketInfo init,int count,List<double> priceList=null)
        {
            _agents = new ConcurrentDictionary<int, Agent>();
            _androids = new ConcurrentDictionary<int, Android>();
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
            if (priceList==null)
            {
                for (int i = 0; i < count; i++)
                {
                    _priceList.Add(Now.Price);
                }
            }
            else
            {
                int tmpc =priceList.Count;
                double tmp = priceList[0];
                for (int i = 0; i < count - tmpc; i++)
                {
                    _priceList.Insert(0, tmp);
                }
                _priceList.AddRange(priceList);
            }
        }

        internal void SyncUpdate(ExperimentInfo info,Parameters parameters)
        {
            androidsTrade(info.Turn,parameters);
            updateMarket(parameters);
            
            getAgentsOrder();
            //getAverageEndowment();
            store(info,parameters);
        }

        private void androidsTrade(int turn,Parameters parameters)
        {
            foreach (Android android in Androids.Values)
            {
                android.Trade(turn==0?0:android.AI(this,parameters), this, parameters);
            }
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
            if (!Agents.IsEmpty)
            {
                _now.AverageEndowment = (Agents.Sum(p => p.Value.Now.Endowment)+Androids.Sum(p=>p.Value.Now.Endowment))/(Agents.Count+Androids.Count);
            }  
        }
        private void store(ExperimentInfo info,Parameters parameters)
        {
            _marketIO.Write(getKey(info),Now);
            foreach (Agent agent in Agents.Values)
            {
                agent.store(info);
                agent.clear(info.Turn,this,parameters);
            }
            foreach (Android android in Androids.Values)
            {
                android.store(info);
                android.clear(info.Turn, this, parameters);
            }
        }
        private void updateMarket(Parameters parameters)
        {
            _now.Returns = Agents.Sum(p => p.Value.Now.TradeStocks) + Androids.Sum(p => p.Value.Now.TradeStocks);
            _now.Volume = Agents.Sum(p => Math.Abs(p.Value.Now.TradeStocks)) + Androids.Sum(p => Math.Abs(p.Value.Now.TradeStocks));
            _priceList.RemoveAt(0);
            _now.Price = Math.Round(Now.Price * Math.Exp(parameters.Market.Lambda * Now.Returns / (Agents.Count +Androids.Count+ 1)), 2);
            _priceList.Add(Now.Price);
            updateState(parameters);
            foreach (Agent agent in Agents.Values)
            {
                agent.syncUpdate(Now.Price);
            }
            foreach (Android android in Androids.Values)
            {
                android.syncUpdate(Now.Price);
            }
        }
        private void updateState(Parameters parameters)
        {
            switch (Now.State)
            {
                case MarketState.Active:
                    _now.State = (Random.GetDouble() < parameters.Market.P10) ? MarketState.Inactive : MarketState.Active;
                    break;
                case MarketState.Inactive:
                    _now.State = (Random.GetDouble() < parameters.Market.P01) ? MarketState.Active : MarketState.Inactive;
                    break;
            }
        }
        private MarketKey getKey(ExperimentInfo info)
        {
            MarketKey marketKey;
            marketKey.ExperimentId =info.Index;
            marketKey.Turn =info.Turn;
            return marketKey;
        }
    }
}
