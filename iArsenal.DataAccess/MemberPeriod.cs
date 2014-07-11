using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class MemberPeriod
    {
        public static DataRow GetMemberPeriodByID(int mpID)
        {
            string sql = "SELECT * FROM dbo.iArsenal_MemberPeriod WHERE ID = @mpID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@mpID", mpID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateMemberPeriod(int mpID, int memberID, string memberName, string memberCardNo, int memberClass, int? orderID, DateTime startDate, DateTime endDate, Boolean isActive, string description, string remark)
        {
            string sql = @"UPDATE dbo.iArsenal_MemberPeriod SET MemberID = @memberID, MemberName = @memberName, MemberCardNo = @memberCardNo, MemberClass = @memberClass,
                                OrderID = @orderID, StartDate = @startDate, EndDate = @endDate, IsActive = @isActive, [Description] = @description, Remark = @remark WHERE ID = @mpID";

            SqlParameter[] para = { 
                                      new SqlParameter("@mpID", mpID), 
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@memberCardNo", memberCardNo),
                                      new SqlParameter("@memberClass", memberClass),
                                      new SqlParameter("@orderID", !orderID.HasValue ? (object)DBNull.Value : (object)orderID.Value),
                                      new SqlParameter("@startDate", startDate),
                                      new SqlParameter("@endDate", endDate),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertMemberPeriod(int mpID, int memberID, string memberName, string memberCardNo, int memberClass, int? orderID, DateTime startDate, DateTime endDate, Boolean isActive, string description, string remark)
        {
            string sql = @"INSERT INTO dbo.iArsenal_MemberPeriod (MemberID, MemberName, MemberCardNo, MemberClass, OrderID,  StartDate, EndDate, IsActive, [Description], Remark) 
                               VALUES (@memberID, @memberName, @memberCardNo, @memberClass, @orderID, @startDate, @endDate, @isActive, @description, @remark)";

            SqlParameter[] para = { 
                                      new SqlParameter(), 
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@memberCardNo", memberCardNo),
                                      new SqlParameter("@memberClass", memberClass),
                                      new SqlParameter("@orderID", !orderID.HasValue ? (object)DBNull.Value : (object)orderID.Value),
                                      new SqlParameter("@startDate", startDate),
                                      new SqlParameter("@endDate", endDate),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteMemberPeriod(int mpID)
        {
            string sql = "DELETE dbo.iArsenal_MemberPeriod WHERE ID = @mpID";

            SqlParameter[] para = { new SqlParameter("@mpID", mpID) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetMemberPeriods()
        {
            string sql = @"SELECT ID, MemberID, MemberName, MemberCardNo, MemberClass, OrderID, StartDate, EndDate, IsActive, Description, Remark FROM iArsenal_MemberPeriod ORDER BY ID DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetMemberPeriods(int memberID)
        {
            string sql = @"SELECT ID, MemberID, MemberName, MemberCardNo, MemberClass, OrderID, StartDate, EndDate, IsActive, Description, Remark FROM iArsenal_MemberPeriod WHERE MemberID = @memberID ORDER BY ID DESC";

            SqlParameter[] para = { new SqlParameter("@memberID", memberID) };

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
