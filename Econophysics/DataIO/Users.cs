using MySql.Data.MySqlClient;

namespace Econophysics
{
    namespace DataIO
    {
        namespace Mysql
        {
            /// <summary>
            /// 所有可用的用户
            /// </summary>
            internal class Users
            {
                /// <summary>
                /// 提供给代理人写入数据库
                /// </summary>
                internal static MySqlConnectionStringBuilder Agent = new MySqlConnectionStringBuilder();
                /// <summary>
                /// 提供给市场数据写入数据库
                /// </summary>
                internal static MySqlConnectionStringBuilder Market = new MySqlConnectionStringBuilder();
                /// <summary>
                /// 提供对整个数据库的读写
                /// </summary>
                internal static MySqlConnectionStringBuilder Experiment = new MySqlConnectionStringBuilder();
                static Users()
                {
                    Agent.Database = "econophysics";
                    Agent.UserID = "agent";
                    Agent.Password = "123456";
                    Agent.Server = "127.0.0.1";

                    Market.Database = "econophysics";
                    Market.UserID = "market";
                    Market.Password = "123456";
                    Market.Server = "127.0.0.1";

                    Experiment.Database = "econophysics";
                    Experiment.UserID = "experiment";
                    Experiment.Password = "123456";
                    Experiment.Server = "127.0.0.1";
                }
            }
        }

    }
}