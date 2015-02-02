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
            List<double> priceList = Experiment.Market.PriceList;/*new List<double>();
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
    }
}
