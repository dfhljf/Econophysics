using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    internal class Market
    {
        public List<double> PriceList { get { return _priceList; } }
        private List<double> _priceList;
        private double _price;
        private bool _state;
        private int _returns;

        public MarketInfo GetInfo()
        {
            MarketInfo market;
            market.Price = _price;
            market.Returns = _returns;
            market.State = _state;
            return market;
        }
    }
}
