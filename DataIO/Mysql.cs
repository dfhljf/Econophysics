using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using CommonType;
using Newtonsoft.Json;
using System.Collections;

namespace DataIO
{
    namespace Mysql
    {
        internal abstract class MysqlIO
        {
            protected internal MySqlConnectionStringBuilder _connStr;
            protected internal MySqlConnection _conn;
            protected internal MySqlCommand _sql;
        }
    }

}
