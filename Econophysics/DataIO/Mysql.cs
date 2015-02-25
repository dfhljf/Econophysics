using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Econophysics
{
    namespace DataIO
    {
        namespace Mysql
        {
            /// <summary>
            /// Mysql的抽象实现
            /// </summary>
            public abstract class MysqlIO
            {
                /// <summary>
                /// 数据库连接字符串
                /// </summary>
                protected internal MySqlConnectionStringBuilder _connStr;
                /// <summary>
                /// 数据库连接
                /// </summary>
                protected internal MySqlConnection _conn;
                /// <summary>
                /// 数据库命令
                /// </summary>
                protected internal MySqlCommand _sql;
            }
        }

    }
}