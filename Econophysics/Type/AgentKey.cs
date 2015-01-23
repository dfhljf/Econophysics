using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    namespace Type
    {
        /// <summary>
        /// 代理人键
        /// </summary>
        public struct AgentKey
        {
            /// <summary>
            /// 实验编号
            /// </summary>
            public int ExperimentId;
            /// <summary>
            /// 实验轮次
            /// </summary>
            public int Turn;
            /// <summary>
            /// 代理人编号
            /// </summary>
            public int Id;
        }
    }
}