using System;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    namespace Type
    {
        /// <summary>
        /// 杠杆效应
        /// </summary>
        public enum LeverageEffect
        {
            /// <summary>
            /// 无杠杆效应
            /// </summary>
            Null = 0,
            /// <summary>
            /// 杠杆效应
            /// </summary>
            Leverage = 1,
            /// <summary>
            /// 反杠杆效应
            /// </summary>
            AntiLeverage = -1
        }
    }
}
