using System.Configuration;
using System.Data.SqlClient;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class SQLConn
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;

            return new SqlConnection(connectionString);
        }

        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;
        }
    }
}
