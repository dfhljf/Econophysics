using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Econophysics
{
    using Type;
    namespace DataIO
    {
        namespace Mysql
        {
            /// <summary>
            /// Mysql实现市场写入
            /// </summary>
            public class MarketIO : Mysql.MysqlIO
            {
                /// <summary>
                /// 初始化市场读取
                /// </summary>
                public MarketIO()
                {
                    _connStr = Users.Market;
                    _conn = new MySqlConnection(_connStr.ConnectionString);
                }
                /// <summary>
                /// 写入本轮的实验数据
                /// </summary>
                /// <param name="marketKey">轮次</param>
                /// <param name="marketInfo">本轮市场信息</param>
                public void Write(MarketKey marketKey, MarketInfo marketInfo)
                {
                    _sql = new MySqlCommand("insert into Market values(@ExperimentId,@Turn,@Price,@State,@Returns,@NumberOfPeople,@AverageEndowment)", _conn);
                    _conn.Open();
                    _sql.Prepare();
                    _sql.Parameters.AddWithValue("@ExperimentId", marketKey.ExperimentId);
                    _sql.Parameters.AddWithValue("@Turn", marketKey.Turn);
                    _sql.Parameters.AddWithValue("@Price", marketInfo.Price);
                    _sql.Parameters.AddWithValue("@State", (int)marketInfo.State);
                    _sql.Parameters.AddWithValue("@Returns", marketInfo.Returns);
                    _sql.Parameters.AddWithValue("@NumberOfPeople", marketInfo.NumberOfPeople);
                    _sql.Parameters.AddWithValue("@AverageEndowment", marketInfo.AverageEndowment);
                    _sql.ExecuteNonQuery();
                    _conn.Close();
                }
            }
        }

    }
}