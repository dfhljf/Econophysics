using System;
using System.Collections.Generic;
using System.Text;

namespace CommonType
{
    /// <summary>
    /// 错误列表，包含所有错误信息
    /// </summary>
    public class ErrorList
    {
        /// <summary>
        /// 错误：现金不足
        /// </summary>
        public static Exception CashOut = new Exception(ErrorMessage.CashOut);
        /// <summary>
        /// 错误：股票不足
        /// </summary>
        public static Exception InsufficientStocks = new Exception(ErrorMessage.InsufficientStocks);
        /// <summary>
        /// 错误：现金、股票均不足
        /// </summary>
        public static Exception UserRuin = new Exception(ErrorMessage.UserRuin);
        /// <summary>
        /// 错误：本轮交易多次
        /// </summary>
        public static Exception TradeTwice = new Exception(ErrorMessage.TradeTwice);
        /// <summary>
        /// 错误：用户不存在
        /// </summary>
        public static Exception UserNotExist = new Exception(ErrorMessage.UserNotExist);
        /// <summary>
        /// 错误：实验没在运行
        /// </summary>
        public static Exception ExperimentNotRun = new Exception(ErrorMessage.ExperimentNotRun);
        /// <summary>
        /// 错误：交易数量溢出
        /// </summary>
        public static Exception TradeStocksOut = new Exception(ErrorMessage.TradeStocksOut);
        /// <summary>
        /// 错误：不允许开始实验
        /// </summary>
        public static Exception NotAllowToStart = new Exception(ErrorMessage.NotAllowToStart);
        /// <summary>
        /// 错误：不允许转到下一轮
        /// </summary>
        public static Exception NotAllowToPauseOrNext = new Exception(ErrorMessage.NotAllowToPauseOrNext);
    }
}
