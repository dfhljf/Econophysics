using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Econophysics
{
    /// <summary>
    /// 实验使用的参数，定义为结构防止更改，在每次试验中保持
    /// 参数不变
    /// </summary>
    public struct Parameters
	{
        /// <summary>
        /// 代理人参数
        /// </summary>
        public struct Agent
        {
            /// <summary>
            /// 可能交易股票的最大数目
            /// </summary>
            int MaxStock;
            /// <summary>
            /// 更新分红的周期
            /// </summary>
            int PeriodOfUpdateDividend;
            /// <summary>
            /// 代理人初始的现金
            /// </summary>
            double InitialCash;
            /// <summary>
            /// 代理人初始的股票数目
            /// </summary>
            int InitialStocks;
            /// <summary>
            /// 每股交易费用
            /// </summary>
            double TradeFee;
            /// <summary>
            /// 分红数量
            /// </summary>
            double Dividend;
        }

        public struct Market
        {
            /// <summary>
            /// 杠杆效应
            /// </summary>
            bool Leverage;
            /// <summary>
            /// 价格系数
            /// </summary>
            double Lambda;
            /// <summary>
            /// 市场状态0->1转变概率
            /// </summary>
            double P01;
            /// <summary>
            /// 市场状态1->0转变概率
            /// </summary>
            double P10;
            /// <summary>
            /// 分红正负概率
            /// </summary>
            double P;
            /// <summary>
            /// 分红正负转变概率
            /// </summary>
            double TransP;
            /// <summary>
            /// 分红改变的时间窗口
            /// </summary>
            int TimeWindow;
        }
        public struct Graphic
        {
            /// <summary>
            /// 存储市场价格的数量，画图需要
            /// </summary>
            int Count;
        }
        public struct Experiment
        {
            /// <summary>
            /// 每轮实验秒数
            /// </summary>
            int PeriodOfTurn;
            /// <summary>
            /// 最大的轮数
            /// </summary>
            int MaxTurn;
        }
	}
}
