using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    /// <summary>
    /// 代理人模型
    /// </summary>
    internal class Agent
    {
        /// <summary>
        /// 编号，每个代理人都有唯一的编号
        /// </summary>
        public int Index { get; set; }
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
        /// 分红数目
        /// </summary>
        public double Dividend { get; set; }
        /// <summary>
        /// 交易股票数目
        /// </summary>
        public int TradeStocks { get { return tradeStocks; } }
        /// <summary>
        /// 本轮是否交易过，限制一轮交易一次
        /// </summary>
        public bool IsTrade { get; set; }
        /// <summary>
        /// 内部变量，限制访问
        /// >0->buy,<0->sell,==0->null
        /// </summary>
        private int tradeStocks;

    }
}
