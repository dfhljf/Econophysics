﻿using CommonType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    internal class Market
    {
        /// <summary>
        /// 价格列表，数量由<see cref="Parameters.GraphicPart.Count"></see>确定
        /// </summary>
        internal List<double> PriceList { get { return _priceList; } }
        private List<double> _priceList;
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

        internal Market(Parameters.Market market)
        {
            _price = market.Init.Price;
            _state = market.Init.State;
            _returns = market.Init.Returns;
            for(int i=0;i<market.Count;i++)
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
    }
}
