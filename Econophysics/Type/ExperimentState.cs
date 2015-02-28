using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    namespace Type
    {
        /// <summary>
        /// 实验状态
        /// </summary>
        public enum ExperimentState
        {
            /// <summary>
            /// 还未建立实验，实验者不得加入和退出
            /// </summary>
            Unbuilded,
            /// <summary>
            /// 实验已建立，实验者可以开始加入实验，不得进行操作
            /// </summary>
            Builded,
            /// <summary>
            /// 实验运行中，实验者可以加入实验并进行操作
            /// </summary>
            Running,
            /// <summary>
            /// 实验挂起，处理当前轮次的交易
            /// </summary>
            Suspend,
            /// <summary>
            /// 实验暂停中，实验者可以加入实验，不可以操作
            /// </summary>
            Pause,
            /// <summary>
            /// 实验结束，实验者全部退出实验，不可以操作
            /// </summary>
            End
        }
    }
}
