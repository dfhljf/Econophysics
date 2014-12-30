using System;
using System.Collections.Generic;
using System.Text;

namespace CommonType
{
    public struct AgentInfo
    {
        /// <summary>
        /// 现金数量
        /// </summary>
        public double Cash;
        /// <summary>
        /// 股票数目
        /// </summary>
        public int Stocks;
        /// <summary>
        /// 当前的总资产=股票数目*市场价格+现金
        /// </summary>
        public double Endowment;
        /// <summary>
        /// 分红数目
        /// </summary>
        public double Dividend;
    }
}
