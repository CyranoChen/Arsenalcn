using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class OrderItem
    {
        public static DataRow GetOrderItemByID(int itemID)
        {
            string sql = "SELECT * FROM dbo.iArsenal_OrderItem WHERE ID = @itemID";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@itemID", itemID));

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0].Rows[0]; }
        }

        public static void UpdateOrderItem(int itemID, int memberID, string memberName, int orderID, Guid productGuid, string code, string productName, string size,
            float unitPrice, int quantity, float? sale, DateTime createTime, Boolean isActive, string remark, SqlTransaction trans = null)
        {
            string sql = @"UPDATE dbo.iArsenal_OrderItem SET MemberID = @memberID, MemberName = @memberName, OrderID = @orderID, ProductGuid = @productGuid, Code = @code, 
                               ProductName = @productName, Size = @size, UnitPrice = @unitPrice, Quantity = @quantity, Sale = @sale, CreateTime = @createTime, IsActive = @isActive, Remark = @remark WHERE ID = @itemID";

            SqlParameter[] para = {
                                      new SqlParameter("@itemID", itemID),
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@orderID", orderID),
                                      new SqlParameter("@productGuid", productGuid),
                                      new SqlParameter("@code", code),
                                      new SqlParameter("@productName", productName),
                                      new SqlParameter("@size", size),
                                      new SqlParameter("@unitPrice", unitPrice),
                                      new SqlParameter("@quantity", quantity),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@remark", remark)
                                  };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static void InsertOrderItem(int itemID, int memberID, string memberName, int orderID, Guid productGuid, string code, string productName, string size,
            float unitPrice, int quantity, float? sale, DateTime createTime, Boolean isActive, string remark, SqlTransaction trans = null)
        {
            string sql = @"INSERT INTO dbo.iArsenal_OrderItem (MemberID, MemberName, OrderID, ProductGuid, Code, ProductName, Size, UnitPrice, Quantity, Sale, CreateTime, IsActive, Remark) 
                               VALUES (@memberID, @memberName, @orderID, @productGuid, @code, @productName, @size, @unitPrice, @quantity, @sale, @createTime, @isActive, @remark)";

            SqlParameter[] para = {
                                      new SqlParameter(),
                                      new SqlParameter("@memberID", memberID),
                                      new SqlParameter("@memberName", memberName),
                                      new SqlParameter("@orderID", orderID),
                                      new SqlParameter("@productGuid", productGuid),
                                      new SqlParameter("@code", code),
                                      new SqlParameter("@productName", productName),
                                      new SqlParameter("@size", size),
                                      new SqlParameter("@unitPrice", unitPrice),
                                      new SqlParameter("@quantity", quantity),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@remark", remark)
                                  };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static void DeleteOrderItem(int itemID, SqlTransaction trans = null)
        {
            string sql = "DELETE dbo.iArsenal_OrderItem WHERE ID = @itemID";

            SqlParameter[] para = { new SqlParameter("@itemID", itemID) };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static void DeleteOrderItemByOrderID(int orderID, SqlTransaction trans = null)
        {
            string sql = "DELETE dbo.iArsenal_OrderItem WHERE OrderID = @orderID";

            SqlParameter[] para = { new SqlParameter("@orderID", orderID) };

            if (trans == null)
            { SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para); }
            else
            { SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, para); }
        }

        public static DataTable GetOrderItems()
        {
            string sql = @"SELECT ID, MemberID, MemberName, OrderID, ProductGuid, Code, ProductName, Size, UnitPrice, Quantity, Sale, CreateTime, IsActive, Remark 
                                FROM dbo.iArsenal_OrderItem ORDER BY ID DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0]; }
        }

        public static DataTable GetOrderItems(int orderID)
        {
            string sql = @"SELECT ID, MemberID, MemberName, OrderID, ProductGuid, Code, ProductName, Size, UnitPrice, Quantity, Sale, CreateTime, IsActive, Remark 
                                FROM dbo.iArsenal_OrderItem WHERE OrderID = @orderID ORDER BY ID DESC";

            SqlParameter[] para = { new SqlParameter("@orderID", orderID) };

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, para);

            if (ds.Tables[0].Rows.Count == 0)
            { return null; }
            else
            { return ds.Tables[0]; }
        }
    }
}
