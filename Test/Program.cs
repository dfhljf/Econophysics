using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using CommonType;
using DataIO.Mysql;
using System.Collections;
using Econophysics;
using SvgNet;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;
using SvgNet.SvgGdi;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            testGraphic();
        }
        private static void testGraphic()
        {
            SvgGraphics price = new SvgGraphics();
            price.DrawLine(new Pen(Color.Black,2),new PointF(0,0),new PointF(10,10));
            string x = price.ToString();
        }
        private static void testExperiment()
        {
            Parameters p;
            p.AgentPart.Init.Cash = 10000;
            p.AgentPart.Init.Dividend = 1;
            p.AgentPart.Init.Endowment = 20000;
            p.AgentPart.Init.Stocks = 100;
            p.AgentPart.Init.TradeStocks = 0;
            p.AgentPart.Init.Order = 0;
            p.AgentPart.MaxStock = 10;
            p.AgentPart.PeriodOfUpdateDividend = 5;
            p.AgentPart.TradeFee = 1;

            p.MarketPart.Init.NumberOfPeople = 20;
            p.MarketPart.Init.Price = 100;
            p.MarketPart.Init.Returns = 0;
            p.MarketPart.Init.State = MarketState.Active;
            p.MarketPart.Init.AverageEndowment = 0;
            p.MarketPart.Count = 50;
            p.MarketPart.Lambda = 0.01;
            p.MarketPart.Leverage = LeverageEffect.AntiLeverage;
            p.MarketPart.P = 0.7;
            p.MarketPart.P01 = 0.25;
            p.MarketPart.P10 = 0.1;
            p.MarketPart.PDividend = 0.8;
            p.MarketPart.TimeWindow = 10;
            p.MarketPart.TransP = 0.5;

            p.GraphicPart.Init.Count = 50;
            p.GraphicPart.Init.Height = 500;
            p.GraphicPart.Init.Url = "priceimage.svg";
            p.GraphicPart.Init.Width = 900;

            p.ExperimentPart.StartTurn = 0;
            p.ExperimentPart.MaxTurn = 1200;
            p.ExperimentPart.PeriodOfTurn = 10;

            Experiment.Build(p);
            
            Experiment.AddAgent(1);
            Experiment.AddAgent(2);
            Experiment.AddAgent(3);
            Experiment.Start();
            Experiment.Trade(1, 5);
            Experiment.Trade(2, 3);
            Experiment.Trade(3, 7);
            Experiment.nextTurn();
            Experiment.Trade(1, -3);
            Experiment.Trade(2, -7);
            Experiment.Trade(3, -5);
            Experiment.nextTurn();
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
            init.Order = 0;
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
            p.AgentPart.Init.Order = 0;
            p.AgentPart.MaxStock = 10;
            p.AgentPart.PeriodOfUpdateDividend = 5;
            p.AgentPart.TradeFee = 1;

            p.MarketPart.Init.NumberOfPeople = 20;
            p.MarketPart.Init.Price = 100;
            p.MarketPart.Init.Returns = 0;
            p.MarketPart.Init.State = MarketState.Active;
            p.MarketPart.Init.AverageEndowment = 0;
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

            p.ExperimentPart.StartTurn = 0;
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
