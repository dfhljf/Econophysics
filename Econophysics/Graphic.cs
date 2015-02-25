using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Xml;

namespace Econophysics
{
    using DataIO.Mysql;
    using Type;
    internal class Graphic
    {
        /// <summary>
        /// 图像信息
        /// </summary>
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
            List<double> priceList = Experiment.Market.PriceList;
            string gridLineStyle = "'fill:none;stroke:Black;stroke-width:2;'";
            string gridTextStyle = "'font-weight:bold;fill:Black;font-size:16pt;font-family:微软雅黑;'";
            string priceLineStyle = "'fill:none;stroke:Blue;stroke-width:2;marker-mid:url(#marker);'";
            string priceTextStyle = "'fill:Black;font-size:18pt;font-family:微软雅黑;'";
            string priceStyle = "'font-weight:bold;fill:Black;font-size:20pt;font-family:微软雅黑;'";

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
            unitVector = new PointF((Single)(-1.0 * (_info.Width - 57) / (priceList.Count - 1)), (Single)(-1.0 * (_info.Height - 50) / priceGapNumber));

            //开始画图
            StreamWriter priceImage = new StreamWriter(_info.Url);
            priceImage.Write("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
            priceImage.Write("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"[");
            priceImage.Write(string.Format("<!ENTITY GLS {0}>", gridLineStyle));
            priceImage.Write(string.Format("<!ENTITY GTS {0}>", gridTextStyle));
            priceImage.Write(string.Format("<!ENTITY PLS {0}>", priceLineStyle));
            priceImage.Write(string.Format("<!ENTITY PTS {0}>", priceTextStyle));
            priceImage.Write(string.Format("<!ENTITY PS {0}>", priceStyle));
            priceImage.Write("]>");
            priceImage.Write("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">");
            priceImage.Write("<g>");

            //纵坐标
            priceImage.Write(drawShape("line", origin.X, 0, origin.X, _info.Height, "&GLS;"));
            //横坐标
            priceImage.Write(drawShape("line", 0, origin.Y - priceGapNumber * unitVector.Y / 2, _info.Width, origin.Y - priceGapNumber * unitVector.Y / 2, "&GLS;"));
            priceImage.Write(drawShape("text", origin.X, origin.Y - priceGapNumber * unitVector.Y / 2 + 25, "价格", "&GTS;"));

            //纵坐标标记
            {
                int tmp = priceGapNumber / 8;
                tmp = tmp + 5 - tmp % 5;
                for (int i = 0; i <= priceGapNumber / 2; i += tmp / 5)
                {
                    priceImage.Write(drawShape("line", origin.X - (i % 5 == 0 ? 8 : 4), origin.Y - i * unitVector.Y, origin.X, origin.Y - i * unitVector.Y, "&GLS;"));
                    priceImage.Write(drawShape("line", origin.X - (i % 5 == 0 ? 8 : 4), origin.Y + i * unitVector.Y, origin.X, origin.Y + i * unitVector.Y, "&GLS;"));

                    if (i % tmp == 0 && i != priceGapNumber / 2 && i != 0)
                    {
                        priceImage.Write(drawShape("text", origin.X + 2, origin.Y + i * unitVector.Y + 10, basePrice + i, "&PTS;"));
                        priceImage.Write(drawShape("text", origin.X + 2, origin.Y - i * unitVector.Y + 10, basePrice - i, "&PTS;"));
                    }
                    else if (i == 0)
                    {
                        priceImage.Write(drawShape("text", origin.X + 2, origin.Y + 10, basePrice, "&PTS;"));
                    }
                }
            }
            //价格线
            string pricePoints = "";

            for (int i = 0; i < priceList.Count; i++)
            {
                pricePoints += string.Format("{0:F2},{1:F2} ", origin.X + i * unitVector.X, origin.Y + (priceList[priceList.Count - i - 1] - basePrice) * unitVector.Y);
            }
            priceImage.Write("<marker id=\"marker\" markerWidth=\"4\" markerHeight=\"4\" refX=\"2\" refY=\"2\"><circle cx=\"2\" cy=\"2\" r=\"2\" style=\"fill:Red;stroke:none;\"/></marker>");
            priceImage.Write(drawShape("polyline", pricePoints, "&PLS;"));
            //标记点和价格
            for (int i = 0; i < priceList.Count; i++)
            {
                if (i != priceList.Count - 1)
                {
                    if (i == 0)//现在价格点
                        priceImage.Write(string.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" style=\"fill:Red;stroke:none;\"/>", origin.X, (Single)(origin.Y + (priceList[priceList.Count - 1] - basePrice) * unitVector.Y), 8));
                    if (i % 5 == 0 && i != 0)//横坐标标记
                        priceImage.Write(drawShape("text", origin.X + i * unitVector.X - 12, origin.Y - priceGapNumber * unitVector.Y / 2 + 25, i, "&PTS;"));
                }
                else
                {
                    priceImage.Write(drawShape("text", origin.X + i * unitVector.X - 8, origin.Y - priceGapNumber * unitVector.Y / 2 + 25, "轮次", "&GTS;"));
                }

                //横坐标线
                priceImage.Write(drawShape("line", origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2, origin.X + i * unitVector.X, origin.Y - priceGapNumber * unitVector.Y / 2 - (i % 5 == 0 ? 8 : 4), "&GLS;"));
            }
            //本轮价格
            priceImage.Write(drawShape("text", origin.X + priceList.Count * unitVector.X / 2 - 100, origin.Y - 200, string.Format("本轮价格：{0:F2}", priceList[priceList.Count - 1]), "&PS;"));
            priceImage.Write("</g></svg>");

            //存储图像
            priceImage.Close();
            priceImage.Dispose();
        }
        private string drawShape(string shape, params object[] args)
        {
            string rtn;
            switch (shape)
            {
                case "line":
                    rtn = string.Format("<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" style=\"{4}\"/>", args);
                    break;
                case "text":
                    rtn = string.Format("<text x=\"{0}\" y=\"{1}\" style=\"{3}\">{2}</text>", args);
                    break;
                case "polyline":
                    rtn = string.Format("<polyline points=\"{0}\" style=\"{1}\"/>", args);
                    break;
                default:
                    rtn = "";
                    break;
            }
            return rtn;
        }
    }
}
