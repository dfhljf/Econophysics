using CommonType;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataIO
{
    namespace Mysql
    {
        /// <summary>
        /// Mysql实现市场写入
        /// </summary>
        public class MarketIO:Mysql.MysqlIO
        {
            private int _exp;
            /// <summary>
            /// 初始化市场读取
            /// </summary>
            /// <param name="expId">实验编号</param>
            public MarketIO(int expId)
            {
                _exp = expId;
                _connStr = Users.Market;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            /// <summary>
            /// 写入本轮的实验数据
            /// </summary>
            /// <param name="turn">轮次</param>
            /// <param name="marketInfo">本轮市场信息</param>
            public void Write(int turn, MarketInfo marketInfo)
            {
                string sql = string.Format("insert into Market values({0},{1},{2},{3},{4},{5})", 
                    _exp, turn, marketInfo.Price, (int)marketInfo.State, marketInfo.Returns,marketInfo.NumberOfPeople);
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                _sql.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }

}
