using System.Configuration;
using System.Data.SqlClient;

namespace Arsenalcn.Common
{
    public class SQLConn
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Arsenalcn.ConnectionString"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
