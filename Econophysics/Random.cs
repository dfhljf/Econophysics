using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Econophysics
{
    public static class Random
    {
        private static System.Random _random=new System.Random();
        public static double GetDouble()
        {
            return _random.NextDouble();
        }
        public static int GetInt(int max)
        {
            return _random.Next(max);
        }
    }
}
