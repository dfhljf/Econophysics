using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using Econophysics;
using Econophysics.Type;
using Econophysics.DataIO.Mysql;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            testGraphic();
        }
        private static void testConcurrent()
        {

        }
        private static void testGraphic()
        {

        }
        /*private static void testGraphic()
        {
            int _width = 900;
            int _height = 500;
            double[] priceRange = new double[2];
            Point origin = new Point(_width - 50, _height / 2 - 30 / 2 + 20 / 2);
            Point unitVector;
            int priceGapNumber;
            int basePrice;
            List<double> priceList = new List<double>();
            for (int i = 0; i < 50; i++)
            {
                priceList.Add(100*Math.Sin(i));
            }
            SvgColourServer blackPen = new SvgColourServer(Color.Black);
            SvgUnit smallFont = new SvgUnit(SvgUnitType.Point, 16);
            SvgUnit bigFont = new SvgUnit(SvgUnitType.Point, 18);
            string fontFamily="微软雅黑";
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SvgColourServer redBrush = new SvgColourServer(Color.Red);

            //获取最大最小价格
            priceRange[0] = priceList[0];
            priceRange[1] = priceList[0];
            for (int i = 1; i < priceList.Count; i++)
            {
                priceRange[0] = (priceList[i] > priceRange[0] ? priceList[i] : priceRange[0]);
                priceRange[1] = (priceList[i] < priceRange[1] ? priceList[i] : priceRange[1]);
            }

            //获取尺度
            {
                int tmp = Convert.ToInt32(priceRange[0] - priceRange[1]);
                priceGapNumber = (tmp + tmp % 2 + 10);
                priceGapNumber = (priceGapNumber > 20 ? priceGapNumber : 20);
                tmp = Convert.ToInt32((priceRange[0] + priceRange[1]) / 2);
                basePrice = tmp + (tmp % 5 > 2 ? 5 - tmp % 5 : -tmp % 5);
            }
            unitVector = new Point(-(_width - 57) / (priceList.Count - 1), -(_height - 50) / priceGapNumber);

            SvgDocument s = new SvgDocument { Height = 500, Width = 900 };
            var group=new SvgGroup();
            s.Children.Add(group);

            //纵坐标
            group.Children.Add(new SvgLine { StartX = origin.X, StartY = 0, EndX = origin.X, EndY = _height, Stroke = blackPen,StrokeWidth=2 });
            //横坐标
            group.Children.Add(new SvgLine { StartX = 0, StartY = origin.Y - priceGapNumber * unitVector.Y / 2, EndX = _width, EndY = origin.Y - priceGapNumber * unitVector.Y / 2, Stroke = blackPen,StrokeWidth=2 });
            group.Children.Add(new SvgText { Text = "价格", FontFamily = fontFamily, FontSize = smallFont, FontWeight = SvgFontWeight.bold, X = origin.X, Y = origin.Y - priceGapNumber * unitVector.Y / 2 + 25 });
            //纵坐标标记
            {
                int tmp = priceGapNumber / 8;
                tmp = tmp + 5 - tmp % 5;
                for (int i = 0; i <= priceGapNumber / 2; i++)
                {
                    group.Children.Add(new SvgLine { StartX = origin.X - (i % 5 == 0 ? 8 : 4), StartY = origin.Y - i * unitVector.Y, EndX = origin.X, EndY = origin.Y - i * unitVector.Y, Stroke = blackPen ,StrokeWidth=2});
                    group.Children.Add(new SvgLine { StartX = origin.X - (i % 5 == 0 ? 8 : 4), StartY = origin.Y + i * unitVector.Y, EndX = origin.X, EndY = origin.Y + i * unitVector.Y, Stroke = blackPen ,StrokeWidth=2});
                  
                    if (i % tmp == 0 && i != priceGapNumber / 2 && i != 0)
                    {
                        group.Children.Add(new SvgText { Text = (basePrice + i).ToString(), FontFamily = fontFamily, FontSize = bigFont, X = origin.X + 2, Y = origin.Y + i * unitVector.Y + 10 });
                        group.Children.Add(new SvgText { Text = (basePrice - i).ToString(), FontFamily = fontFamily, FontSize = bigFont, X = origin.X + 2, Y = origin.Y - i * unitVector.Y + 10 });
                    }
                    else if (i == 0)
                    {
                        group.Children.Add(new SvgText { Text = basePrice.ToString(), FontFamily = fontFamily, FontSize = bigFont, X = origin.X + 2, Y = origin.Y + 10 });
                    }
                }
            }
            //价格线
            SvgPolyline pricePath = new SvgPolyline { Fill=SvgPaintServer.None,Stroke = new SvgColourServer(Color.Blue), StrokeWidth = 2, Points = new SvgUnitCollection() };
            group.Children.Add(pricePath);
            for (int i = 1; i <= priceList.Count; i++)
            {
                pricePath.Points.Add(origin.X + i * unitVector.X);
                pricePath.Points.Add((Single)(origin.Y + (priceList[priceList.Count - i] - basePrice) * unitVector.Y));
            }

            //标记点和价格
                for (int i = 0; i < pricePath.Points.Count/2; i++)
                {
                    if (i != pricePath.Points.Count / 2 - 1)
                    {
                        if (i != 0)//以前价格点
                            group.Children.Add(new SvgEllipse { CenterX = pricePath.Points[2 * i] - 3, CenterY = pricePath.Points[2 * i + 1] - 3, RadiusX = 6, RadiusY = 6, Fill = redBrush,Stroke=SvgPaintServer.None });
                        else//现在价格点
                            group.Children.Add(new SvgEllipse { CenterX = pricePath.Points[2 * i] - 8, CenterY = pricePath.Points[2 * i + 1] - 8, RadiusX = 16, RadiusY = 16, Fill = redBrush, Stroke = SvgPaintServer.None });
                        if (i % 5 == 0 && i != 0)//横坐标标记
                            group.Children.Add(new SvgText { Text = i.ToString(), FontFamily = fontFamily, FontSize = bigFont, X = origin.X + i * unitVector.X - 12, Y = origin.Y - priceGapNumber * unitVector.Y / 2 + 25 });
                    }
                    else
                    {
                        group.Children.Add(new SvgText { Text = "轮次", FontFamily = fontFamily, FontSize = smallFont, FontWeight = SvgFontWeight.bold, X = origin.X + i * unitVector.X - 12, Y = origin.Y - priceGapNumber * unitVector.Y / 2 + 25 });
                    }

                    //横坐标线
                    group.Children.Add(new SvgLine { StartX = origin.X + i * unitVector.X, StartY = origin.Y - priceGapNumber * unitVector.Y / 2, EndX = origin.X + i * unitVector.X, EndY = origin.Y - priceGapNumber * unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4), Stroke = blackPen, StrokeWidth = 2 });
                }
            //本轮价格
                group.Children.Add(new SvgText { Text = string.Format("本轮价格：{0:F2}", priceList[priceList.Count - 1]), FontFamily = fontFamily, FontSize = new SvgUnit(SvgUnitType.Point, 20), FontWeight = SvgFontWeight.bold, X = origin.X + priceList.Count * unitVector.X / 2 - 100, Y = origin.Y - 200 });
           
            string x=s.GetXML();
        }*/
        private static void testExperiment()
        {
            Parameters p;

            p.Agent.Init = new AgentInfo
            {
                Cash = 10000,
                Dividend = 0,
                Endowment = 20000,
                Stocks = 100,
                TradeStocks = 0,
                Order = 0
            };
            p.Agent.MaxStock = 10;
            p.Agent.PeriodOfUpdateDividend = 5;
            p.Agent.TradeFee = 1;
            p.Agent.Dividend = 1;

            p.Market.Init = new MarketInfo();
            p.Market.Init.NumberOfPeople = 0;
            p.Market.Init.Price = 100;
            p.Market.Init.Returns = 0;
            p.Market.Init.State = MarketState.Active;
            p.Market.Init.AverageEndowment = 0;
            p.Market.Init.Volume = 0;
            p.Market.Count = 50;
            p.Market.Lambda = 0.01;
            p.Market.Leverage = LeverageEffect.AntiLeverage;
            p.Market.P = 0.7;
            p.Market.P01 = 0.25;
            p.Market.P10 = 0.1;
            p.Market.PDividend = 0.8;
            p.Market.TimeWindow = 10;
            p.Market.TransP = 0.5;

            p.Graphic.Init = new GraphicInfo();
            p.Graphic.Init.Count = 50;
            p.Graphic.Init.Height = 500;
            p.Graphic.Init.Url = "priceimage.svg";
            p.Graphic.Init.Width = 900;
            
            
            p.Experiment.StartTurn = 0;
            p.Experiment.MaxTurn = 1200;
            p.Experiment.PeriodOfTurn = 10;
            p.Experiment.Comments = "";
            p.Experiment.StartTime = DateTime.Now;

            Experiment.Build(p);
            
            Experiment.AddAgent(1);
            Experiment.AddAgent(2);
            Experiment.AddAgent(3);
            Experiment.Start();
            Experiment.Trade(1, 5);
            Experiment.Trade(2, 3);
            Experiment.Trade(3, 7);
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
            AgentInfo init=new AgentInfo();
            init.Cash = 10000;
            init.Dividend = 1;
            init.Endowment = 10000;
            init.Stocks = 1000;
            init.TradeStocks = 10;
            init.Order = 0;
            
            Parameters para = (Parameters)eht[1];
            para.Agent.Init = init;
        }
        private static void testDataIOWrite()
        {
            ExperimentIO ei = new ExperimentIO();
            MarketIO mi = new MarketIO();
            AgentIO ai = new AgentIO();

            Parameters p=new Parameters();

            p.Agent.Init = new AgentInfo();
            p.Agent.Init.Cash = 10000;
            p.Agent.Init.Dividend = 1;
            p.Agent.Init.Endowment = 20000;
            p.Agent.Init.Stocks = 100;
            p.Agent.Init.TradeStocks = 0;
            p.Agent.Init.Order = 0;
            p.Agent.MaxStock = 10;
            p.Agent.PeriodOfUpdateDividend = 5;
            p.Agent.TradeFee = 1;
            p.Agent.Dividend = 1;

            p.Market.Init = new MarketInfo();
            p.Market.Init.NumberOfPeople = 20;
            p.Market.Init.Price = 100;
            p.Market.Init.Returns = 0;
            p.Market.Init.State = MarketState.Active;
            p.Market.Init.AverageEndowment = 0;
            p.Market.Init.Volume = 0;
            p.Market.Count = 50;
            p.Market.Lambda = 0.01;
            p.Market.Leverage = LeverageEffect.AntiLeverage;
            p.Market.P = 0.7;
            p.Market.P01 = 0.25;
            p.Market.P10 = 0.1;
            p.Market.PDividend = 0.8;
            p.Market.TimeWindow = 10;
            p.Market.TransP = 0.5;

            p.Graphic.Init = new GraphicInfo();
            p.Graphic.Init.Count = 50;
            p.Graphic.Init.Height = 500;
            p.Graphic.Init.Url = "priceimage.svg";
            p.Graphic.Init.Width = 900;


            p.Experiment.StartTurn = 0;
            p.Experiment.MaxTurn = 1200;
            p.Experiment.PeriodOfTurn = 10;
            p.Experiment.Comments = "";
            p.Experiment.StartTime = DateTime.Now;

            MarketKey mk;
            mk.ExperimentId = 1;
            mk.Turn = 0;
            AgentKey ak;
            ak.ExperimentId = 1;
            ak.Id = 1;
            ak.Turn = 0;
            ei.Write(1, p);
            mi.Write(mk, p.Market.Init);
            ai.Write(ak, p.Agent.Init);
        }
    }
}
