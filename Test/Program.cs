using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using Newtonsoft.Json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AgentInfo x = new AgentInfo();
            x.Cash = 10;
            x.Dividend = 10;
            x.Endowment = 10;
            x.Stocks = 10;
            string str = JsonConvert.SerializeObject(x);
            AgentInfo y = (AgentInfo)JsonConvert.DeserializeObject(str, typeof(AgentInfo));
        }
    }
}
