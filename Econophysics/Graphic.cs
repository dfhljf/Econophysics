using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using SvgNet;
using SvgNet.SvgGdi;
using CommonType;

namespace Econophysics
{
    internal class Graphic
    {
        private int _width;
        private int _height;
        private SvgGraphics _price;
        private string _url;
        
        

        internal Graphic()
        {
            _width = 900;
            _height = 500;
            _price = new SvgGraphics();
            _url = Experiment.Parameters.GraphicPart.Init.Url;
        }
        internal void Draw()
        {
            draw();
            store();
        }
        internal GraphicInfo getInfo()
        {
            GraphicInfo graphicInfo;
            graphicInfo.Count = Experiment._market.PriceList.Count;
            graphicInfo.Height = _height;
            graphicInfo.Width = _width;
            graphicInfo.Url = _url;
            return graphicInfo;
        }
        private void draw()
        {
            double[] priceRange = new double[2];
            Point origin = new Point(_width - 50, _height / 2 - 30 / 2 + 20 / 2);
            Point unitVector;
            int priceGapNumber;
            int basePrice;
            List<double> priceList=Experiment._market.PriceList;
            Pen blackPen = new Pen(Color.Black, 2);
            Font smallFont = new Font(new FontFamily("Microsoft YaHei"), 16, FontStyle.Bold);
            Font bigFont = new Font(new FontFamily("Microsoft YaHei"), 18);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush redBrush = new SolidBrush(Color.Red);

            _price.Clear(Color.Transparent);
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

            //生成价格点
            Point[] pricePoints = new Point[priceList.Count];
            for (int i = 0; i < priceList.Count; i++)
            {
                pricePoints[i] = new Point(origin.X + i * unitVector.X, Convert.ToInt32(origin.Y + (priceList[priceList.Count - 1 - i] - basePrice) * unitVector.Y));
            }

            //参考线 Price=100
            //priceGraphics.DrawLine(new Pen(Color.Black, 2), new Point(0, originPoint.Y), new Point(originPoint.X, originPoint.Y));
            //纵坐标
            _price.DrawLine(blackPen, new Point(origin.X, 0), new Point(origin.X, _height));
            ////横坐标
            _price.DrawLine(blackPen, new Point(0, origin.Y - priceGapNumber * unitVector.Y / 2), new Point(_width, origin.Y - priceGapNumber * unitVector.Y / 2));
            _price.DrawString("价格", smallFont, blackBrush, new Point(origin.X, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
            //纵坐标标记
            {
                int tmp = priceGapNumber / 8;
                tmp = tmp + 5 - tmp % 5;
                for (int i = 0; i <= priceGapNumber / 2; i++)
                {
                    _price.DrawLine(blackPen, new Point(origin.X - (i % 5 == 0 ? 8 : 4), origin.Y - i * unitVector.Y), new Point(origin.X, origin.Y - i * unitVector.Y));
                    _price.DrawLine(blackPen, new Point(origin.X - (i % 5 == 0 ? 8 : 4), origin.Y + i * unitVector.Y), new Point(origin.X, origin.Y + i * unitVector.Y));
                    if (i % tmp == 0 && i != priceGapNumber / 2 && i != 0)
                    {
                        _price.DrawString((basePrice + i).ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y + i * unitVector.Y + 10));
                        _price.DrawString((basePrice - i).ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y - i * unitVector.Y + 10));
                    }
                    else if (i == 0)
                    {
                        _price.DrawString(basePrice.ToString(), bigFont, blackBrush, new Point(origin.X + 2, origin.Y + i * unitVector.Y + 10));
                    }
                }
            }

            //价格线
            _price.DrawLines(new Pen(Color.Blue, 2), pricePoints);

            //标记点和价格
            {
                Point point;
                for (int i = 0; i < pricePoints.Length; i++)
                {
                    point = pricePoints[i];

                    if (i != pricePoints.Length - 1)
                    {
                        if (i != 0)//以前价格点
                            _price.FillEllipse(redBrush, Convert.ToInt32(point.X - 3), Convert.ToInt32(point.Y - 3), 6, 6);
                        else//现在价格点
                            _price.FillEllipse(redBrush, Convert.ToInt32(point.X - 8), Convert.ToInt32(point.Y - 8), 16, 16);
                        if (i % 5 == 0 && i != 0)//横坐标标记
                            _price.DrawString(i.ToString(), bigFont, blackBrush, new Point(origin.X + i * unitVector.X - 12, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
                    }
                    else
                    {
                        _price.DrawString("轮次", smallFont, blackBrush, new Point(origin.X + i * unitVector.X - 12, origin.Y - priceGapNumber * unitVector.Y / 2 + 25));
                    }

                    //横坐标线
                    _price.DrawLine(blackPen, new Point(origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2), new Point(origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4)));
                }
            }
            //本轮价格
            _price.DrawString(string.Format("本轮价格：{0:F2}", priceList[priceList.Count - 1]), new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold), blackBrush, new PointF(origin.X + priceList.Count * unitVector.X / 2 - 100, origin.Y - 200));
        }
        private void store()
        {
            StreamWriter _sw = new StreamWriter(_url);
            _sw.Write(_price.WriteSVGString());
            _sw.Close();
        }
    }
}
