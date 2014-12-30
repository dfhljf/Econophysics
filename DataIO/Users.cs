using MySql.Data.MySqlClient;

namespace DataIO
{
    namespace Mysql
    {
    internal class Users
    {
        internal static MySqlConnectionStringBuilder Agent = new MySqlConnectionStringBuilder();
        internal static MySqlConnectionStringBuilder Market = new MySqlConnectionStringBuilder();
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
