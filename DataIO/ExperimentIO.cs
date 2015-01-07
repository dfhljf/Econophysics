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
            /// <summary>
            /// 初始化数据库全局读写
            /// </summary>
            public ExperimentIO()
            {
                _connStr = Users.Experiment;
                _conn = new MySqlConnection(_connStr.ConnectionString);
            }
            /// <summary>
            /// 写入实验参数
            /// </summary>
            /// <param name="experimentId">实验编号</param>
            /// <param name="parameters">实验参数</param>
            /// <param name="comments">实验注释</param>
            public void Write(int experimentId,Parameters parameters, string comments)
            {
                _sql = new MySqlCommand("insert into Parameters values(@Id,@MaxStock,"+
                    "@PeriodOfUpdateDividend,@TradeFee,@Count,@Leverage,@Lambda," +
                    "@P01,@P10,@PDividend,@P,@TransP,@TimeWindow,@PeriodOfTurn," +
                    "@MaxTurn,@DateTime,@Comments)", _conn);
                _conn.Open();
                _sql.Prepare();
                _sql.Parameters.AddWithValue("@Id",experimentId);
                _sql.Parameters.AddWithValue("@MaxStock", parameters.AgentPart.MaxStock);
                _sql.Parameters.AddWithValue("@PeriodOfUpdateDividend", parameters.AgentPart.PeriodOfUpdateDividend);
                _sql.Parameters.AddWithValue("@TradeFee", parameters.AgentPart.TradeFee);
                _sql.Parameters.AddWithValue("@Count",parameters.MarketPart.Count);
                _sql.Parameters.AddWithValue("@Leverage",(int)parameters.MarketPart.Leverage);
                _sql.Parameters.AddWithValue("@Lambda", parameters.MarketPart.Lambda);
                _sql.Parameters.AddWithValue("@P01", parameters.MarketPart.P01);
                _sql.Parameters.AddWithValue("@P10", parameters.MarketPart.P10);
                _sql.Parameters.AddWithValue("@PDividend", parameters.MarketPart.PDividend);
                _sql.Parameters.AddWithValue("@P", parameters.MarketPart.P);
                _sql.Parameters.AddWithValue("@TransP", parameters.MarketPart.TransP);
                _sql.Parameters.AddWithValue("@TimeWindow", parameters.MarketPart.TimeWindow);
                _sql.Parameters.AddWithValue("@PeriodOfTurn", parameters.ExperimentPart.PeriodOfTurn);
                _sql.Parameters.AddWithValue("@MaxTurn", parameters.ExperimentPart.MaxTurn);
                _sql.Parameters.AddWithValue("@DateTime", DateTime.Now);
                _sql.Parameters.AddWithValue("@Comments", comments);
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
                Hashtable rtn = new Hashtable();
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                MySqlDataReader record=_sql.ExecuteReader();
                string tmp=sql.ToLower();
                if(tmp.Contains("agents"))
                {
                    rtn = convertToAgent(record);
                }
                else if(tmp.Contains("market"))
                {
                    rtn = convertToMarket(record);
                }
                else if(tmp.Contains("parameters"))
                {
                    rtn = convertToParameters(record);
                }
                record.Close();
                _conn.Close();
                return rtn;
            }
            /// <summary>
            /// 读取数据库中最大的实验编号
            /// </summary>
            /// <returns>最大的实验编号，不存在返回0</returns>
            public int Read()
            {
                string sql = "select max(Id) from parameters";
                _sql = new MySqlCommand(sql, _conn);
                _conn.Open();
                object index=_sql.ExecuteScalar();
                _conn.Close();
                return (index == DBNull.Value ? 0 :  Convert.ToInt32(index));
            }
            private Hashtable convertToParameters(MySqlDataReader record)
            {
                Hashtable parameters = new Hashtable();
                while (record.Read())
                {
                    Parameters para = new Parameters();
                    para.AgentPart.MaxStock = record.GetInt32("MaxStock");
                    para.AgentPart.PeriodOfUpdateDividend = record.GetInt32("PeriodOfUpdateDividend");
                    para.AgentPart.TradeFee = record.GetDouble("TradeFee");
                    para.MarketPart.Count = record.GetInt32("Count");
                    para.MarketPart.Lambda = record.GetDouble("Lambda");
                    para.MarketPart.Leverage = (LeverageEffect)record.GetInt32("Leverage");
                    para.MarketPart.P = record.GetDouble("P");
                    para.MarketPart.P01 = record.GetDouble("P01");
                    para.MarketPart.P10 = record.GetDouble("P10");
                    para.MarketPart.PDividend = record.GetDouble("PDividend");
                    para.MarketPart.TimeWindow = record.GetInt32("TimeWindow");
                    para.MarketPart.TransP = record.GetDouble("TransP");
                    para.ExperimentPart.MaxTurn = record.GetInt32("MaxTurn");
                    para.ExperimentPart.PeriodOfTurn = record.GetInt32("PeriodOfTurn");
                    parameters.Add(record.GetInt32("Id"),para);
                }
                return parameters;
            }
            private Hashtable convertToMarket(MySqlDataReader record)
            {
                Hashtable market = new Hashtable();
                while(record.Read())
                {
                    MarketKey mk;
                    MarketInfo mi;
                    mk.ExperimentId = record.GetInt32("ExperimentId");
                    mk.Turn = record.GetInt32("Turn");
                    mi.NumberOfPeople = record.GetInt32("NumberOfPeople");
                    mi.Price = record.GetDouble("Price");
                    mi.Returns = record.GetInt32("Returns");
                    mi.State = (MarketState)record.GetInt32("State");
                    mi.AverageEndowment = record.GetDouble("AverageEndowment");
                    market.Add(mk, mi);
                }
                return market;
            }
            private Hashtable convertToAgent(MySqlDataReader record)
            {
                Hashtable agent = new Hashtable();
                while (record.Read())
                {
                    AgentKey ak;
                    AgentInfo ai;
                    ak.ExperimentId = record.GetInt32("ExperimentId");
                    ak.Turn = record.GetInt32("Turn");
                    ak.Id = record.GetInt32("Id");
                    ai.Cash = record.GetDouble("Cash");
                    ai.Dividend = record.GetDouble("Dividend");
                    ai.Endowment = record.GetDouble("Endowment");
                    ai.Stocks = record.GetInt32("Stocks");
                    ai.TradeStocks = record.GetInt32("TradeStocks");
                    ai.Order = record.GetInt32("OrderNum");
                    agent.Add(ak, ai);
                }
                return agent;
            }
        }
    }

}
