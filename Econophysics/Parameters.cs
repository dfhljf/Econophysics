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
        public Agent AgentPart;
        public Market MarketPart;
        public Graphic GraphicPart;
        public Experiment ExperimentPart;

        /// <summary>
        /// 代理人参数
        /// </summary>
        public struct Agent
        {
            /// <summary>
            /// 可能交易股票的最大数目
            /// </summary>
            public int MaxStock;
            /// <summary>
            /// 更新分红的周期
            /// </summary>
            public int PeriodOfUpdateDividend;
            /// <summary>
            /// 代理人初始信息
            /// </summary>
            public AgentInfo Init;
            /// <summary>
            /// 每股交易费用
            /// </summary>
            public double TradeFee;
        }
        public struct Market
        {
            /// <summary>
            /// 杠杆效应
            /// </summary>
            public bool Leverage;
            /// <summary>
            /// 价格系数
            /// </summary>
            public double Lambda;
            /// <summary>
            /// 市场状态0->1转变概率
            /// </summary>
            public double P01;
            /// <summary>
            /// 市场状态1->0转变概率
            /// </summary>
            public double P10;
            /// <summary>
            /// 分红正负概率
            /// </summary>
            public double P;
            /// <summary>
            /// 分红正负转变概率
            /// </summary>
            public double TransP;
            /// <summary>
            /// 分红改变的时间窗口
            /// </summary>
            public int TimeWindow;
        }
        public struct Graphic
        {
            /// <summary>
            /// 存储市场价格的数量，画图需要
            /// </summary>
            public int Count;
        }
        public struct Experiment
        {
            /// <summary>
            /// 每轮实验秒数
            /// </summary>
            public int PeriodOfTurn;
            /// <summary>
            /// 最大的轮数
            /// </summary>
            public int MaxTurn;
        }
    }
}
