using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using SvgNet.SvgGdi;
using CommonType;

namespace Econophysics
{
    internal class Graphic
    {
        private int _width;
        private int _height;
        private SvgGraphics _price;
        private Point _origin;
        private Point _unitVector;
        private double[] _priceRange = new double[2];
        private int _priceGapNumber;
        private int _basePrice;
        private string _url;

        internal Graphic(GraphicInfo init)
        {
            _url = init.Url;
        }
        internal void Draw()
        {
            init();
            draw();
            store();
        }
        private void  init()
        {
            _width = 900;
            _height = 500;
            _origin = new Point(_width - 50, _height / 2 - 30 / 2 + 20 / 2);
            _price = new SvgGraphics();
        }
        private void draw()
        {
            List<double> priceList=Experiment._market.PriceList;
            _price.Clear(Color.Transparent);
            //获取最大最小价格
            _priceRange[0] = priceList[0];
            _priceRange[1] = priceList[0];
            for (int i = 1; i < priceList.Count; i++)
            {
                _priceRange[0] = (priceList[i] > _priceRange[0] ? priceList[i] : _priceRange[0]);
                _priceRange[1] = (priceList[i] < _priceRange[1] ? priceList[i] : _priceRange[1]);
            }

            //获取尺度
            {
                int tmp = Convert.ToInt32(_priceRange[0] - _priceRange[1]);
                _priceGapNumber = (tmp + tmp % 2 + 10);
                _priceGapNumber = (_priceGapNumber > 20 ? _priceGapNumber : 20);
                tmp = Convert.ToInt32((_priceRange[0] + _priceRange[1]) / 2);
                _basePrice = tmp + (tmp % 5 > 2 ? 5 - tmp % 5 : -tmp % 5);
            }
            _unitVector = new Point(-(_width - 57) / (priceList.Count - 1), -(_height - 50) / _priceGapNumber);

            //生成价格点
            Point[] pricePoints = new Point[priceList.Count];
            for (int i = 0; i < priceList.Count; i++)
            {
                pricePoints[i] = new Point(_origin.X + i * _unitVector.X, Convert.ToInt32(_origin.Y + (priceList[priceList.Count - 1 - i] - _basePrice) * _unitVector.Y));
            }

            //参考线 Price=100
            //priceGraphics.DrawLine(new Pen(Color.Black, 2), new Point(0, originPoint.Y), new Point(originPoint.X, originPoint.Y));
            //纵坐标
            _price.DrawLine(new Pen(Color.Black, 2), new Point(_origin.X, 0), new Point(_origin.X, _height));
            //横坐标
            _price.DrawLine(new Pen(Color.Black, 2), new Point(0, _origin.Y - _priceGapNumber * _unitVector.Y / 2), new Point(_width, _origin.Y - _priceGapNumber * _unitVector.Y / 2));
            _price.DrawString("价格", new Font(new FontFamily("Microsoft YaHei"), 16, FontStyle.Bold), new SolidBrush(Color.Black), new Point(_origin.X, _origin.Y - _priceGapNumber * _unitVector.Y / 2 + 25));
            //纵坐标标记
            {
                int tmp = _priceGapNumber / 8;
                tmp = tmp + 5 - tmp % 5;
                for (int i = 0; i <= _priceGapNumber / 2; i++)
                {
                    _price.DrawLine(new Pen(Color.Black, 2), new Point(_origin.X - (i % 5 == 0 ? 8 : 4), _origin.Y - i * _unitVector.Y), new Point(_origin.X, _origin.Y - i * _unitVector.Y));
                    _price.DrawLine(new Pen(Color.Black, 2), new Point(_origin.X - (i % 5 == 0 ? 8 : 4), _origin.Y + i * _unitVector.Y), new Point(_origin.X, _origin.Y + i * _unitVector.Y));
                    if (i % tmp == 0 && i != _priceGapNumber / 2 && i != 0)
                    {
                        _price.DrawString((_basePrice + i).ToString(), new Font(new FontFamily("Microsoft YaHei"), 18), new SolidBrush(Color.Black), new Point(_origin.X + 2, _origin.Y + i * _unitVector.Y + 10));
                        _price.DrawString((_basePrice - i).ToString(), new Font(new FontFamily("Microsoft YaHei"), 18), new SolidBrush(Color.Black), new Point(_origin.X + 2, _origin.Y - i * _unitVector.Y + 10));
                    }
                    else if (i == 0)
                    {
                        _price.DrawString(_basePrice.ToString(), new Font(new FontFamily("Microsoft YaHei"), 18), new SolidBrush(Color.Black), new Point(_origin.X + 2, _origin.Y + i * _unitVector.Y + 10));
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
                            _price.FillEllipse(new SolidBrush(Color.Red), Convert.ToInt32(point.X - 3), Convert.ToInt32(point.Y - 3), 6, 6);
                        else//现在价格点
                            _price.FillEllipse(new SolidBrush(Color.Red), Convert.ToInt32(point.X - 8), Convert.ToInt32(point.Y - 8), 16, 16);
                        if (i % 5 == 0 && i != 0)//横坐标标记
                            _price.DrawString(i.ToString(), new Font(new FontFamily("Microsoft YaHei"), 18), new SolidBrush(Color.Black), new Point(_origin.X + i * _unitVector.X - 12, _origin.Y - _priceGapNumber * _unitVector.Y / 2 + 25));
                    }
                    else
                    {
                        _price.DrawString("轮次", new Font(new FontFamily("Microsoft YaHei"), 16, FontStyle.Bold), new SolidBrush(Color.Black), new Point(_origin.X + i * _unitVector.X - 12, _origin.Y - _priceGapNumber * _unitVector.Y / 2 + 25));
                    }

                    //横坐标线
                    _price.DrawLine(new Pen(Color.Black, 2), new Point(_origin.X + i * _unitVector.X, _origin.Y - _priceGapNumber * _unitVector.Y / 2), new Point(_origin.X + i * _unitVector.X, _origin.Y - _priceGapNumber * _unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4)));
                }
            }
            //本轮价格
            _price.DrawString(string.Format("本轮价格：{0:F2}", priceList[priceList.Count-1]), new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(_origin.X + priceList.Count * _unitVector.X / 2 - 100, _origin.Y - 200));
        }
        private void store()
        {
            StreamWriter sw = new StreamWriter(_url);
            sw.Write(_price.WriteSVGString());
            sw.Close();
        }
    }
}
