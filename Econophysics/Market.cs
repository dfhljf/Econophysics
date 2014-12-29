using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    internal class Market
    {
        internal List<double> PriceList { get { return _priceList; } }
        private List<double> _priceList;
        internal double _price;
        internal bool _state;
        internal int _returns;

        internal MarketInfo GetInfo()
        {
            MarketInfo market;
            market.Price = _price;
            market.Returns = _returns;
            market.State = _state;
            return market;
        }
    }
}
