using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg;
using Svg.DataTypes;
using System.Collections;
using System.Xml;

namespace Econophysics
{
    using Type;
    using DataIO.Mysql;
    internal class Graphic
    {
        public GraphicInfo Info { get { return _info; } }
        private GraphicInfo _info;

        internal Graphic()
        {
            _info = new GraphicInfo 
            { 
                Width = Experiment.Parameters.Graphic.Init.Width,
                Height = Experiment.Parameters.Graphic.Init.Height,
                Url = Experiment.Parameters.Graphic.Init.Url,
                Count = Experiment.Parameters.Graphic.Init.Count 
            };
        }
        internal void Draw()
        {
            draw();
        }
        private void draw()
        {
            double[] priceRange = new double[2];
            PointF origin = new PointF(_info.Width - 50, _info.Height / 2 - 30 / 2 + 20 / 2);
            PointF unitVector;
            int priceGapNumber;
            int basePrice;
            List<double> priceList = Experiment._market.PriceList;/*new List<double>();
            for (int i = 0; i < 50; i++)
            {
                priceList.Add(100 * Math.Sin(i));
            }*/
            SvgColourServer blackPen = new SvgColourServer(Color.Black);
            SvgUnit smallFont = new SvgUnit(SvgUnitType.Point, 16);
            SvgUnit bigFont = new SvgUnit(SvgUnitType.Point, 18);
            string fontFamily = "微软雅黑";
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
                priceGapNumber = (priceGapNumber > 20 ? priceGapNumber : 300);
                tmp = Convert.ToInt32((priceRange[0] + priceRange[1]) / 2);
                basePrice = tmp + (tmp % 5 > 2 ? 5 - tmp % 5 : -tmp % 5);
            }
            unitVector = new PointF((Single)(-1.0*(_info.Width - 57) / (priceList.Count - 1)), (Single)(-1.0*(_info.Height - 50) / priceGapNumber));

            //开始画图
            SvgDocument priceImage = new SvgDocument { Height = _info.Height, Width = _info.Width };
            var group = new SvgGroup();
            priceImage.Children.Add(group);

            //纵坐标
            group.Children.Add(new SvgLine { StartX = origin.X, StartY = 0, EndX = origin.X, EndY = _info.Height, Stroke = blackPen, StrokeWidth = 2 });
            //横坐标
            group.Children.Add(new SvgLine { StartX = 0, StartY = origin.Y - priceGapNumber * unitVector.Y / 2, EndX = _info.Width, EndY = origin.Y - priceGapNumber * unitVector.Y / 2, Stroke = blackPen, StrokeWidth = 2 });
            group.Children.Add(new SvgText { Text = "价格", FontFamily = fontFamily, FontSize = smallFont, FontWeight = SvgFontWeight.bold, X = origin.X, Y = origin.Y - priceGapNumber * unitVector.Y / 2+25 });
            //纵坐标标记
            {
                int tmp = priceGapNumber / 8;
                tmp = tmp + 5 - tmp % 5;
                for (int i = 0; i <= priceGapNumber / 2; i+=tmp/5)
                {
                    group.Children.Add(new SvgLine { StartX = origin.X - (i % 5 == 0 ? 8 : 4), StartY = origin.Y - i * unitVector.Y, EndX = origin.X, EndY = origin.Y - i * unitVector.Y, Stroke = blackPen, StrokeWidth = 2 });
                    group.Children.Add(new SvgLine { StartX = origin.X - (i % 5 == 0 ? 8 : 4), StartY = origin.Y + i * unitVector.Y, EndX = origin.X, EndY = origin.Y + i * unitVector.Y, Stroke = blackPen, StrokeWidth = 2 });

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
            SvgPolyline pricePath = new SvgPolyline { Fill = SvgPaintServer.None, Stroke = new SvgColourServer(Color.Blue), StrokeWidth = 2, Points = new SvgUnitCollection() };
            group.Children.Add(pricePath);
            for (int i = 0; i < priceList.Count; i++)
            {
                pricePath.Points.Add(origin.X + i * unitVector.X);
                pricePath.Points.Add((Single)(origin.Y + (priceList[priceList.Count - i-1] - basePrice) * unitVector.Y));
            }

            //标记点和价格
            for (int i = 0; i < pricePath.Points.Count / 2; i++)
            {
                if (i != pricePath.Points.Count / 2 - 1)
                {
                    if (i != 0)//以前价格点
                        group.Children.Add(new SvgEllipse { CenterX = pricePath.Points[2 * i], CenterY = pricePath.Points[2 * i + 1], RadiusX = 3, RadiusY = 3, Fill = redBrush, Stroke = SvgPaintServer.None });
                    else//现在价格点
                        group.Children.Add(new SvgEllipse { CenterX = pricePath.Points[2 * i], CenterY = pricePath.Points[2 * i + 1], RadiusX = 8, RadiusY = 8, Fill = redBrush, Stroke = SvgPaintServer.None });
                    if (i % 5 == 0 && i != 0)//横坐标标记
                        group.Children.Add(new SvgText { Text = i.ToString(), FontFamily = fontFamily, FontSize = bigFont, X = origin.X + i * unitVector.X - 12, Y = origin.Y - priceGapNumber * unitVector.Y / 2 + 25 });
                }
                else
                {
                    group.Children.Add(new SvgText { Text = "轮次", FontFamily = fontFamily, FontSize = smallFont, FontWeight = SvgFontWeight.bold, X = origin.X + i * unitVector.X - 8, Y = origin.Y - priceGapNumber * unitVector.Y / 2 + 25 });
                }

                //横坐标线
                group.Children.Add(new SvgLine { StartX = origin.X + i * unitVector.X, StartY = origin.Y - priceGapNumber * unitVector.Y / 2, EndX = origin.X + i * unitVector.X, EndY = origin.Y - priceGapNumber * unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4), Stroke = blackPen, StrokeWidth = 2 });
            }
            //本轮价格
            group.Children.Add(new SvgText { Text = string.Format("本轮价格：{0:F2}", priceList[priceList.Count - 1]), FontFamily = fontFamily, FontSize = new SvgUnit(SvgUnitType.Point, 20), FontWeight = SvgFontWeight.bold, X = origin.X + priceList.Count * unitVector.X / 2 - 100, Y = origin.Y - 200 });

            //存储图像
            priceImage.Write(_info.Url);

        }

        #region 旧版画图库
        //private void oldDraw()
        //{
        //    double[] priceRange = new double[2];
        //    Point origin = new Point(_width - 50, _height / 2 - 30 / 2 + 20 / 2);
        //    Point unitVector;
        //    int priceGapNumber;
        //    int basePrice;
        //    List<double> priceList=Experiment._market.PriceList;
        //    Pen blackPen = new Pen(Color.Black, 2);
        //    Font smallFont = new Font(new FontFamily("Microsoft YaHei"), 16, FontStyle.Bold);
        //    Font bigFont = new Font(new FontFamily("Microsoft YaHei"), 18);
        //    SolidBrush blackBrush = new SolidBrush(Color.Black);
        //    SolidBrush redBrush = new SolidBrush(Color.Red);

        //    _price.Clear(Color.Transparent);
        //    //获取最大最小价格
        //    priceRange[0] = priceList[0];
        //    priceRange[1] = priceList[0];
        //    for (int i = 1; i < priceList.Count; i++)
        //    {
        //        priceRange[0] = (priceList[i] > priceRange[0] ? priceList[i] : priceRange[0]);
        //        priceRange[1] = (priceList[i] < priceRange[1] ? priceList[i] : priceRange[1]);
        //    }

        //    //获取尺度
        //    {
        //        int tmp = Convert.ToInt32(priceRange[0] - priceRange[1]);
        //        priceGapNumber = (tmp + tmp % 2 + 10);
        //        priceGapNumber = (priceGapNumber > 20 ? priceGapNumber : 20);
        //        tmp = Convert.ToInt32((priceRange[0] + priceRange[1]) / 2);
        //        basePrice = tmp + (tmp % 5 > 2 ? 5 - tmp % 5 : -tmp % 5);
        //    }
        //    unitVector = new Point(-(_width - 57) / (priceList.Count - 1), -(_height - 50) / priceGapNumber);

        //    //生成价格点
        //    Point[] pricePoints = new Point[priceList.Count];
        //    for (int i = 0; i < priceList.Count; i++)
        //    {
        //        pricePoints[i] = new Point(origin.X + i * unitVector.X, Convert.ToInt32(origin.Y + (priceList[priceList.Count - 1 - i] - basePrice) * unitVector.Y));
        //    }

        //    //参考线 Price=100
        //    //priceGraphics.DrawLine(new Pen(Color.Black, 2), new Point(0, originPoint.Y), new Point(originPoint.X, originPoint.Y));
        //    //纵坐标
        //    _price.DrawLine(blackPen, new Point(origin.X, 0), new Point(origin.X, _height));
        //    ////横坐标
        //    _price.DrawLine(blackPen, new Point(0, origin.Y - priceGapNumber * unitVector.Y / 2), new Point(_width, origin.Y - priceGapNumber * unitVector.Y / 2));
        //    _price.DrawString("价格", smallFont, blackBrush, new Point(origin.X, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
        //    //纵坐标标记
        //    {
        //        int tmp = priceGapNumber / 8;
        //        tmp = tmp + 5 - tmp % 5;
        //        for (int i = 0; i <= priceGapNumber / 2; i++)
        //        {
        //            _price.DrawLine(blackPen, new Point(origin.X - (i % 5 == 0 ? 8 : 4), origin.Y - i * unitVector.Y), new Point(origin.X, origin.Y - i * unitVector.Y));
        //            _price.DrawLine(blackPen, new Point(origin.X - (i % 5 == 0 ? 8 : 4), origin.Y + i * unitVector.Y), new Point(origin.X, origin.Y + i * unitVector.Y));
        //            if (i % tmp == 0 && i != priceGapNumber / 2 && i != 0)
        //            {
        //                _price.DrawString((basePrice + i).ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y + i * unitVector.Y + 10));
        //                _price.DrawString((basePrice - i).ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y - i * unitVector.Y + 10));
        //            }
        //            else if (i == 0)
        //            {
        //                _price.DrawString(basePrice.ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y + i * unitVector.Y + 10));
        //            }
        //        }
        //    }

        //    //价格线
        //    _price.DrawLines(new Pen(Color.Blue, 2), pricePoints);

        //    //标记点和价格
        //    {
        //        Point point;
        //        for (int i = 0; i < pricePoints.Length; i++)
        //        {
        //            point = pricePoints[i];

        //            if (i != pricePoints.Length - 1)
        //            {
        //                if (i != 0)//以前价格点
        //                    _price.FillEllipse(redBrush, Convert.ToInt32(point.X - 3), Convert.ToInt32(point.Y - 3), 6, 6);
        //                else//现在价格点
        //                    _price.FillEllipse(redBrush, Convert.ToInt32(point.X - 8), Convert.ToInt32(point.Y - 8), 16, 16);
        //                if (i % 5 == 0 && i != 0)//横坐标标记
        //                    _price.DrawString(i.ToString(), bigFont, blackBrush, new Point(origin.X + i * unitVector.X - 12, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
        //            }
        //            else
        //            {
        //                _price.DrawString("轮次", smallFont, blackBrush, new Point(origin.X + i * unitVector.X - 12, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
        //            }

        //            //横坐标线
        //            _price.DrawLine(blackPen, new Point(origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2), new Point(origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4)));
        //        }
        //    }
        //    //本轮价格
        //    _price.DrawString(string.Format("本轮价格：{0:F2}", priceList[priceList.Count - 1]), new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold), blackBrush, new PointF(origin.X + priceList.Count * unitVector.X / 2 - 100, origin.Y - 200));
        //}
        //private void store()
        //{
        //    StreamWriter _sw = new StreamWriter(_url);
        //    //_sw.Write(_price.WriteSVGString());
        //    _sw.Close();
        //}
        #endregion
    }
}
