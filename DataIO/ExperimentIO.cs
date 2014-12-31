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
        /// Mysql实现数据库全局读写
        /// </summary>
        public class ExperimentIO : Mysql.MysqlIO
        {
            private int _exp;
            /// <summary>
            /// 初始化数据库全局读写
            /// </summary>
            /// <param name="expId">实验编号</param>
            public ExperimentIO(int expId)
            {
                _exp = expId;
                _connStr = Users.Experiment;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            /// <summary>
            /// 写入实验参数
            /// </summary>
            /// <param name="parameters">实验参数</param>
            /// <param name="startTime">实验开始时间</param>
            /// <param name="comments">实验注释</param>
            public void Write(Parameters parameters, DateTime startTime, string comments)
            {
                string sql = string.Format("insert into Parameters values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'{11}','{12}')",
                   _exp, parameters.ExperimentPart.MaxTurn, parameters.ExperimentPart.PeriodOfTurn, (int)parameters.MarketPart.Leverage,
                   parameters.MarketPart.Lambda, parameters.MarketPart.P01, parameters.MarketPart.P10, parameters.AgentPart.TradeFee,
                   parameters.MarketPart.PDividend, parameters.MarketPart.P, parameters.MarketPart.TransP, startTime, comments);
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                _sql.ExecuteNonQuery();
                _conn.Close();
            }
            /// <summary>
            /// 读取数据库
            /// </summary>
            /// <param name="sql">sql命令</param>
            /// <returns>键值为实验编号，值为<see cref="Parameters"></see></returns>
            public Hashtable Read(string sql)
            {
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                MySqlDataReader record=_sql.ExecuteReader();
                _conn.Close();
                string tmp=sql.ToLower();
                if(tmp.Contains("agents"))
                {
                    return convertToAgent(record);
                }
                else if(tmp.Contains("market"))
                {
                    return convertToMarket(record);
                }
                else if(tmp.Contains("parameters"))
                {
                    return convertToParameters(record);
                }
                return null;
            }

            /// <summary>
            /// 读取数据库中最大的实验编号
            /// </summary>
            /// <returns>最大的实验编号，不存在返回0</returns>
            public int Read()
            {
                string sql = "select max(exp) from parameters";
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                object index=_sql.ExecuteScalar();
                _conn.Close();
                return (index == DBNull.Value ? 0 :  Convert.ToInt32(index));
            }
            private Hashtable convertToParameters(MySqlDataReader record)
            {
                //Hashtable parameters = new Hashtable();
                //while (record.Read())
                //{

                    //parameters.Add();
                //}
                throw new NotImplementedException();
            }

            private Hashtable convertToMarket(MySqlDataReader record)
            {
                throw new NotImplementedException();
            }

            private Hashtable convertToAgent(MySqlDataReader record)
            {
                throw new NotImplementedException();
            }
        }
    }

}
