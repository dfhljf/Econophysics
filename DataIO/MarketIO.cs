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
        public class MarketIO:Mysql.MysqlIO
        {
            private int _exp;
            public MarketIO(int expId)
            {
                _exp = expId;
                _connStr = Users.Market;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            public void Write(int turn, MarketInfo marketInfo)
            {
                string sql = string.Format("insert into Market values({0},{1},{2},{3},{4},{5})", 
                    _exp, turn, marketInfo.Price, (int)marketInfo.State, marketInfo.Returns,marketInfo.NumberOfPeople);
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                _sql.ExecuteNonQuery();
                _conn.Close();
            }
            public Hashtable Read(string sql)
            {
                throw new NotImplementedException();
            }
        }
    }

}
