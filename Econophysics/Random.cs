using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Econophysics
{
    /// <summary>
    /// 随机数生成器
    /// </summary>
    public static class Random
    {
        private static System.Random _random=new System.Random();
        /// <summary>
        /// 生成一个0-1的随机小数
        /// </summary>
        /// <returns>0-1的随机数</returns>
        public static double GetDouble()
        {
            return _random.NextDouble();
        }
        /// <summary>
        /// 生成一定范围内的随机整数
        /// </summary>
        /// <param name="max">最大的整数</param>
        /// <returns>随机整数</returns>
        public static int GetInt(int max)
        {
            return _random.Next(max);
        }
    }
}
