using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    namespace Type
    {
        /// <summary>
        /// 市场信息，对所有代理人公开
        /// </summary>
        public class MarketInfo
        {
            /// <summary>
            /// 市场价格
            /// </summary>
            public double Price { get; set; }
            /// <summary>
            /// 市场状态
            /// </summary>
            public MarketState State { get; set; }
            /// <summary>
            /// 收益率
            /// </summary>
            public int Returns { get; set; }
            /// <summary>
            /// 在线人数
            /// </summary>
            public int NumberOfPeople { get; set; }
            /// <summary>
            /// 机器人数目
            /// </summary>
            public int NumberOfAndroids { get; set; }
            /// <summary>
            /// 代理人的平均资产
            /// </summary>
            public double AverageEndowment { get; set; }
            /// <summary>
            /// 交易量
            /// </summary>
            public int Volume { get; set; }
        }
    }
}