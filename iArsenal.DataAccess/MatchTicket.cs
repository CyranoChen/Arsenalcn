using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class MatchTicket
    {
        public static DataRow GetMatchTicketByID(Guid matchGuid)
        {
            string sql = "SELECT * FROM dbo.iArsenal_MatchTicket WHERE MatchGuid = @matchGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@matchGuid", matchGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateMatchTicket(Guid matchGuid, string productCode, DateTime deadline, int? allowMemberClass, bool isActive, string remark)
        {
            string sql = @"UPDATE dbo.iArsenal_MatchTicket SET ProductCode = @productCode, Deadline = @deadline, AllowMemberClass = @allowMemberClass,
                                 IsActive = @isActive, Remark = @remark WHERE MatchGuid = @matchGuid";

            SqlParameter[] para = {
                                      new SqlParameter("@matchGuid", matchGuid),
                                      new SqlParameter("@productCode", productCode),
                                      new SqlParameter("@deadline", deadline),
                                      new SqlParameter("@allowMemberClass", !allowMemberClass.HasValue ? (object)DBNull.Value : (object)allowMemberClass.Value),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertMatchTicket(Guid matchGuid, string productCode, DateTime deadline, int? allowMemberClass, bool isActive, string remark)
        {
            string sql = @"INSERT INTO dbo.iArsenal_MatchTicket (MatchGuid, ProductCode, Deadline, AllowMemberClass, IsActive, Remark) 
                               VALUES (@matchGuid, @productCode, @deadline, @allowMemberClass, @isActive, @remark)";

            SqlParameter[] para = {
                                      new SqlParameter("@matchGuid", matchGuid),
                                      new SqlParameter("@productCode", productCode),
                                      new SqlParameter("@deadline", deadline),
                                      new SqlParameter("@allowMemberClass", !allowMemberClass.HasValue ? (object)DBNull.Value : (object)allowMemberClass.Value),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteMatchTicket(Guid matchGuid)
        {
            string sql = "DELETE dbo.iArsenal_MatchTicket WHERE MatchGuid = @matchGuid";

            SqlParameter[] para = { new SqlParameter("@matchGuid", matchGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetMatchTickets()
        {
            string sql = @"SELECT MatchGuid, ProductCode, Deadline, AllowMemberClass, IsActive, Remark 
                                 FROM iArsenal_MatchTicket ORDER BY Deadline DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
