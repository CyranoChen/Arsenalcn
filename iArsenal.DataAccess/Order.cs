using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class Order
    {
        public static DataRow GetOrderByID(int oID)
        {
            string sql = "SELECT * FROM dbo.iArsenal_Order WHERE ID = @oID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@oID", oID));

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0].Rows[0]; }
        }

        public static void UpdateOrder(int oID, int memberID, string memberName, string orderType, string mobile, string address, string payment, float price, float? sale, float? deposit,
            float postage, int status, int rate, DateTime createTime, DateTime updateTime, Boolean isActive, string description, string remark, SqlTransaction trans = null)
        {
            string sql = @"UPDATE dbo.iArsenal_Order SET MemberID = @memberID, MemberName = @memberName, OrderType = @orderType, Mobile = @mobile, Address = @address, Payment = @payment, 
                                Price = @price, Sale = @sale, Deposit = @deposit, Postage = @postage, Status = @status, Rate = @rate, CreateTime = @createTime, 
                                UpdateTime = @updateTime, IsActive = @isActive, [Description] = @description, Remark = @remark WHERE ID = @oID";

            SqlParameter[] para = {
                                      new SqlParameter("@oID", oID),
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@orderType", orderType),
                                      new SqlParameter("@mobile", mobile),
                                      new SqlParameter("@address", address),
                                      new SqlParameter("@payment", payment),
                                      new SqlParameter("@price", price),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@deposit", !deposit.HasValue ? (object)DBNull.Value : (object)deposit.Value),
                                      new SqlParameter("@postage", postage),
                                      new SqlParameter("@status", status),
                                      new SqlParameter("@rate", rate),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@updateTime", updateTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static int InsertOrder(int oID, int memberID, string memberName, string orderType, string mobile, string address, string payment, float price, float? sale, float? deposit,
            float postage, int status, int rate, DateTime createTime, DateTime updateTime, Boolean isActive, string description, string remark, SqlTransaction trans = null)
        {
            string sql = @"INSERT INTO dbo.iArsenal_Order (MemberID, MemberName, OrderType, Mobile, Address, Payment, Price, Sale, Deposit, Postage, Status, Rate, CreateTime, UpdateTime, IsActive, [Description], Remark) 
                               VALUES (@memberID, @memberName, @orderType, @mobile, @address, @payment, @price, @sale, @deposit, @postage, @status, @rate, @createTime, @updateTime, @isActive, @description, @remark); 
                               SELECT SCOPE_IDENTITY();";

            SqlParameter[] para = {
                                      new SqlParameter(),
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@orderType", orderType),
                                      new SqlParameter("@mobile", mobile),
                                      new SqlParameter("@address", address),
                                      new SqlParameter("@payment", payment),
                                      new SqlParameter("@price", price),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@deposit", !deposit.HasValue ? (object)DBNull.Value : (object)deposit.Value),
                                      new SqlParameter("@postage", postage),
                                      new SqlParameter("@status", status),
                                      new SqlParameter("@rate", rate),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@updateTime", updateTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            if (trans == null)
            { return Convert.ToInt32(SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, para)); }
            else
            { return Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, para)); }
        }

        public static void DeleteOrder(int oID, SqlTransaction trans = null)
        {
            string sql = "DELETE dbo.iArsenal_Order WHERE ID = @oID";

            SqlParameter[] para = { new SqlParameter("@oID", oID) };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static DataTable GetOrders()
        {
            string sql = @"SELECT ID, MemberID, MemberName, OrderType, Mobile, Address, Payment, Price, Sale, Deposit, Postage, Status, 
                               Rate, CreateTime, UpdateTime, IsActive, Description, Remark FROM dbo.iArsenal_Order ORDER BY ID DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0]; }
        }

        public static DataTable GetOrders(int memberID)
        {
            string sql = @"SELECT ID, MemberID, MemberName, OrderType, Mobile, Address, Payment, Price, Sale, Deposit, Postage, Status, 
                               Rate, CreateTime, UpdateTime, IsActive, Description, Remark FROM dbo.iArsenal_Order WHERE MemberID = @memberID ORDER BY ID DESC";

            SqlParameter[] para = { new SqlParameter("@memberID", memberID) };

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0]; }
        }
    }
}
