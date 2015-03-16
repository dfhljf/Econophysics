using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Econophysics.Type
{
    /// <summary>
    /// 实验信息
    /// </summary>
    public class ExperimentInfo
    {
        /// <summary>
        /// 实验编号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 当前轮数
        /// </summary>
        public int Turn { get; set; }
        /// <summary>
        /// 实验状态
        /// </summary>
        public ExperimentState State { get; set; }
    }
}
