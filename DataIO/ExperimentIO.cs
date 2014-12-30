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
    class ExperimentIO:Mysql.MysqlIO
    {
        private int _exp;
        public ExperimentIO(int expId)
        {
            _exp = expId;
            _connStr = Users.Experiment;
            _conn = new MySqlConnection(_connStr.ConnectionString);
        }
        public void Write(Parameters parameters,DateTime startTime,string comments)
        {
            string sql = string.Format("insert into Parameters values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},'{12}')",
               _exp, parameters.ExperimentPart.MaxTurn, parameters.ExperimentPart.PeriodOfTurn, parameters.MarketPart.Leverage,
               parameters.MarketPart.Lambda, parameters.MarketPart.P01, parameters.MarketPart.P10, parameters.AgentPart.TradeFee,
               parameters.MarketPart.PDividend, parameters.MarketPart.P, parameters.MarketPart.TransP, DateTime.Now,comments);
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
