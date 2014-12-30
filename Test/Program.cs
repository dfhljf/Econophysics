using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using Newtonsoft.Json;
using DataIO.Mysql;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ExperimentIO ei = new ExperimentIO(1);
            MarketIO mi = new MarketIO(1);
            AgentIO ai = new AgentIO(1);

            Parameters p;
            p.AgentPart.Init.Cash=0;
            p.AgentPart.Init.Dividend=0;
            p.AgentPart.Init.Endowment=0;
            p.AgentPart.Init.Id=1;
            p.AgentPart.Init.Stocks=0;
            p.AgentPart.Init.TradeStocks=0;
            p.AgentPart.MaxStock=10;
            p.AgentPart.PeriodOfUpdateDividend=5;
            p.AgentPart.TradeFee=1;

            p.MarketPart.Init.NumberOfPeople=20;
            p.MarketPart.Init.Price=100;
            p.MarketPart.Init.Returns=0;
            p.MarketPart.Init.State=MarketState.Active;
            p.MarketPart.Count=50;
            p.MarketPart.Lambda=0.01;
            p.MarketPart.Leverage=LeverageEffect.AntiLeverage;
            p.MarketPart.P=0.7;
            p.MarketPart.P01=0.25;
            p.MarketPart.P10=0.1;
            p.MarketPart.PDividend=0.8;
            p.MarketPart.TimeWindow=10;
            p.MarketPart.TransP=0.5;
            
            p.GraphicPart.Init.Count=0;
            p.GraphicPart.Init.Height=500;
            p.GraphicPart.Init.Url="";
            p.GraphicPart.Init.Width=900;

            p.ExperimentPart.MaxTurn=1200;
            p.ExperimentPart.PeriodOfTurn=10;
            ei.Write(p, DateTime.Now, "xyz");
            mi.Write(0, p.MarketPart.Init);
            ai.Write(0, p.AgentPart.Init);
        }
    }
}
