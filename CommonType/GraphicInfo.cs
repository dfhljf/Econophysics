using System;
using System.Collections.Generic;
using System.Text;

namespace Type
{
    /// <summary>
    /// 图像信息
    /// </summary>
    public class GraphicInfo
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 数据点数目
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 图像所在路径
        /// </summary>
        public string Url { get; set; }
    }
}
