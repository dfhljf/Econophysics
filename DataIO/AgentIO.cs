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
            /// <summary>
            /// 初始化代理人数据库交互
            /// </summary>
            /// <param name="expId">实验编号</param>
            public AgentIO(int expId)
            {
                _exp = expId;
                _connStr = Users.Agent;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            /// <summary>
            /// 写入一轮的代理人数据
            /// </summary>
            /// <param name="turn">轮次</param>
            /// <param name="agentInfo">本轮代理人信息</param>
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
