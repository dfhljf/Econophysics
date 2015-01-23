using System;
using System.Collections.Generic;
using System.Text;

namespace Type
{
    /// <summary>
    /// 代理人信息
    /// </summary>
    public class AgentInfo
    {
        /// <summary>
        /// 现金数量
        /// </summary>
        public double Cash { get; set; }
        /// <summary>
        /// 股票数目
        /// </summary>
        public int Stocks { get; set; }
        /// <summary>
        /// 当前的总资产=股票数目*市场价格+现金
        /// </summary>
        public double Endowment { get; set; }
        /// <summary>
        /// 每个人的排名
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 分红数目
        /// </summary>
        public double Dividend { get; set; }
        /// <summary>
        /// 交易数量
        /// </summary>
        public int TradeStocks { get; set; }
    }
}
