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
            /// <summary>
            /// 初始化代理人数据库交互
            /// </summary>
            /// <param name="expId">实验编号</param>
            public AgentIO()
            {
                _connStr = Users.Agent;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            /// <summary>
            /// 写入一轮的代理人数据
            /// </summary>
            /// <param name="agentKey">轮次</param>
            /// <param name="agentInfo">本轮代理人信息</param>
            public void Write(AgentKey agentKey, AgentInfo agentInfo)
            {
                _sql = new MySqlCommand("insert into Agents values(@ExperimentId,@Turn,@Id,@Cash,@Stocks,@Endowment,@Dividend,@TradeStocks)", _conn);
                _sql.Prepare();
                _sql.Parameters.AddWithValue("@ExperimentId",agentKey.ExperimentId);
                _sql.Parameters.AddWithValue("@Turn",agentKey.Turn);
                _sql.Parameters.AddWithValue("@Id", agentKey.Id);
                _sql.Parameters.AddWithValue("@Cash", agentInfo.Cash);
                _sql.Parameters.AddWithValue("@Stocks", agentInfo.Stocks);
                _sql.Parameters.AddWithValue("@Endowment", agentInfo.Endowment);
                _sql.Parameters.AddWithValue("@Dividend", agentInfo.Dividend);
                _sql.Parameters.AddWithValue("@TradeStocks", agentInfo.TradeStocks);
                _conn.Open();
                _sql.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }

}
