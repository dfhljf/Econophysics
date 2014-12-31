﻿using System;
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
        // TODO: 防止SQL注入攻击
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
