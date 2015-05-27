using System;
using System.Data.SqlClient;

using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Service
{
    public class AdvLog
    {
        public static void LogHistory(int userId, string userName, AdvHistoryType type, string advURL, string clientIP)
        {
            string sql = "INSERT INTO dbo.AcnClub_LogAdv VALUES (@userid, @username, @typeCode, @advURL, @clientIP, getdate());";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userid", userId));
                com.Parameters.Add(new SqlParameter("@username", userName));
                com.Parameters.Add(new SqlParameter("@typeCode", (int)type));
                com.Parameters.Add(new SqlParameter("@advURL", advURL == string.Empty ? (object)DBNull.Value : advURL));
                com.Parameters.Add(new SqlParameter("@clientIP", clientIP == string.Empty ? (object)DBNull.Value : clientIP));

                con.Open();
                com.ExecuteNonQuery();
                //con.Close();
            }
        }
    }

    public enum AdvHistoryType
    {
        GoogleAdv = 1
    }
}
