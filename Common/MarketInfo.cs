using System;
using System.Collections.Generic;
using System.Text;

namespace CommonType
{
    public struct MarketInfo
    {
        /// <summary>
        /// 市场价格
        /// </summary>
        public double Price;
        /// <summary>
        /// 市场状态
        /// </summary>
        public MarketState State;
        /// <summary>
        /// 收益率
        /// </summary>
        public int Returns;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int NumberOfPeople;
    }
}
