using System;

namespace Econophysics
{
    namespace Type
    {
        using Para;
        namespace Para
        {
            /// <summary>
            /// 代理人相关参数
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
                /// 代理人初始信息，其中的分红是分红的尺度
                /// </summary>
                public AgentInfo Init;
                /// <summary>
                /// 每股交易费用
                /// </summary>
                public double TradeFee;
            }
            /// <summary>
            /// 市场相关参数
            /// </summary>
            public struct Market
            {
                /// <summary>
                /// 市场的初始值
                /// </summary>
                public MarketInfo Init;
                /// <summary>
                /// 存储市场价格的数量，画图需要
                /// </summary>
                public int Count;
                /// <summary>
                /// 杠杆效应
                /// </summary>
                public LeverageEffect Leverage;
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
                /// 分红存在概率
                /// </summary>
                // 0.8
                public double PDividend;
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
            /// <summary>
            /// 图像相关参数
            /// </summary>
            public struct Graphic
            {
                /// <summary>
                /// 图像初始化数据
                /// </summary>
                public GraphicInfo Init;
            }
            /// <summary>
            /// 实验相关参数
            /// </summary>
            public struct Experiment
            {
                /// <summary>
                /// 从第几轮开始
                /// </summary>
                public int StartTurn;
                /// <summary>
                /// 每轮实验秒数
                /// </summary>
                public int PeriodOfTurn;
                /// <summary>
                /// 最大的轮数
                /// </summary>
                public int MaxTurn;
                /// <summary>
                /// 实验开始时间
                /// </summary>
                public DateTime StartTime;
                /// <summary>
                /// 实验注释
                /// </summary>
                public string Comments;
            }
        }
        /// <summary>
        /// 实验使用的参数，定义为结构防止更改，在每次试验中保持
        /// 参数不变
        /// </summary>
        public struct Parameters
        {
            /// <summary>
            /// 代理人参数
            /// </summary>
            public Agent Agent;
            /// <summary>
            /// 市场参数
            /// </summary>
            public Market Market;
            /// <summary>
            /// 画图参数
            /// </summary>
            public Graphic Graphic;
            /// <summary>
            /// 实验参数
            /// </summary>
            public Experiment Experiment;
        }
    }
}