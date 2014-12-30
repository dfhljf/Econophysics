using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using CommonType;
using Newtonsoft.Json;

namespace DataIO
{
    namespace Mysql
    {
        internal abstract class Mysql : IDataIO
        {
            private MySqlConnectionStringBuilder _connStr;
            private MySqlConnection _conn;
            private MySqlCommand _sql;

            public abstract void Write(object obj);
            public abstract object Read();
        }
    }

}
