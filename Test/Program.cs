using System;
using System.Collections.Generic;
using System.Text;
using CommonType;
using Newtonsoft.Json;
using DataIO.Mysql;
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            testDataIORead();
        }

        private static void testDataIORead()
        {
            ExperimentIO ei = new ExperimentIO();
            Hashtable eht = new Hashtable();
            Hashtable mht = new Hashtable();
            Hashtable aht = new Hashtable();
            eht = ei.Read("select * from parameters");
            mht = ei.Read("select * from market");
            aht = ei.Read("select * from agents");
            AgentInfo init;
            init.Cash = 10000;
            init.Dividend = 1;
            init.Endowment = 10000;
            init.Stocks = 1000;
            init.TradeStocks = 10;
            Parameters para = (Parameters)eht[1];
            para.AgentPart.Init = init;
        }
        private static void testDataIOWrite()
        {
            ExperimentIO ei = new ExperimentIO();
            MarketIO mi = new MarketIO();
            AgentIO ai = new AgentIO();

            Parameters p;
            p.AgentPart.Init.Cash = 0;
            p.AgentPart.Init.Dividend = 0;
            p.AgentPart.Init.Endowment = 0;
            p.AgentPart.Init.Stocks = 0;
            p.AgentPart.Init.TradeStocks = 0;
            p.AgentPart.MaxStock = 10;
            p.AgentPart.PeriodOfUpdateDividend = 5;
            p.AgentPart.TradeFee = 1;

            p.MarketPart.Init.NumberOfPeople = 20;
            p.MarketPart.Init.Price = 100;
            p.MarketPart.Init.Returns = 0;
            p.MarketPart.Init.State = MarketState.Active;
            p.MarketPart.Count = 50;
            p.MarketPart.Lambda = 0.01;
            p.MarketPart.Leverage = LeverageEffect.AntiLeverage;
            p.MarketPart.P = 0.7;
            p.MarketPart.P01 = 0.25;
            p.MarketPart.P10 = 0.1;
            p.MarketPart.PDividend = 0.8;
            p.MarketPart.TimeWindow = 10;
            p.MarketPart.TransP = 0.5;

            p.GraphicPart.Init.Count = 0;
            p.GraphicPart.Init.Height = 500;
            p.GraphicPart.Init.Url = "";
            p.GraphicPart.Init.Width = 900;

            p.ExperimentPart.MaxTurn = 1200;
            p.ExperimentPart.PeriodOfTurn = 10;
            MarketKey mk;
            mk.ExperimentId = 1;
            mk.Turn = 0;
            AgentKey ak;
            ak.ExperimentId = 1;
            ak.Id = 1;
            ak.Turn = 0;
            ei.Write(1, p, "xyz");
            mi.Write(mk, p.MarketPart.Init);
            ai.Write(ak, p.AgentPart.Init);
        }
    }
}
