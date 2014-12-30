using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using CommonType;

namespace DataIO
{
    namespace Mysql
    {
        /// <summary>
        /// Mysql实现代理人写入
        /// </summary>
        public class AgentIO : Mysql.MysqlIO
        {
            private int _exp;
            public AgentIO(int expId)
            {
                _exp = expId;
                _connStr = Users.Agent;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            public void Write(int turn, AgentInfo agentInfo)
            {
                string sql = string.Format("insert into Agents values({0},{1},{2},{3},{4},{5},{6},{7})",
                    agentInfo.Id, turn, _exp, agentInfo.Cash, agentInfo.Stocks, agentInfo.Dividend, agentInfo.TradeStocks, agentInfo.Endowment);
                _conn.Open();
                _sql = new MySqlCommand(sql, _conn);
                _sql.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }

}
