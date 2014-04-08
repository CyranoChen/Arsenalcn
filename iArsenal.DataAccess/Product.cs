using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace iArsenal.DataAccess
{
    public class Product
    {
        public static DataRow GetProductByID(Guid productGuid)
        {
            string sql = "SELECT * FROM dbo.iArsenal_Product WHERE ProductGuid = @productGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@productGuid", productGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateProduct(Guid productGuid, string code, string name, string displayName, int productType, string imageURL, string material, string colour, string size, string currency, float price, float? sale, DateTime createTime, int stock, Boolean isActive, string description, string remark)
        {
            string sql = @"UPDATE dbo.iArsenal_Product SET Code = @code, [Name] = @name, DisplayName = @displayName, ProductType = @productType, ImageURL = @imageURL, Material = @material, Colour = @colour, Size = @size, Currency = @currency, 
                               Price = @price, Sale = @sale, CreateTime = @createTime, Stock = @stock, IsActive = @isActive, [Description] = @description, Remark = @remark WHERE ProductGuid = @productGuid";

            SqlParameter[] para = {
                                      new SqlParameter("@productGuid", productGuid),
                                      new SqlParameter("@code", code),
                                      new SqlParameter("@name", name),
                                      new SqlParameter("@displayName", displayName),
                                      new SqlParameter("@productType", productType),
                                      new SqlParameter("@imageURL", imageURL),
                                      new SqlParameter("@material", material),
                                      new SqlParameter("@colour", colour),
                                      new SqlParameter("@size", size),
                                      new SqlParameter("@currency", currency),
                                      new SqlParameter("@price", price),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@stock", stock),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertProduct(Guid productGuid, string code, string name, string displayName, int productType, string imageURL, string material, string colour, string size, string currency, float price, float? sale, DateTime createTime, int stock, Boolean isActive, string description, string remark)
        {
            string sql = @"INSERT INTO dbo.iArsenal_Product (Code, [Name], DisplayName, ProductType, ImageURL, Material, Colour, Size, Currency, Price, Sale, CreateTime, Stock, IsActive, [Description], Remark) 
                               VALUES (@code, @name, @displayName, @productType, @imageURL, @material, @colour, @size, @currency, @price, @sale, @createTime, @stock, @isActive, @description, @remark)";

            SqlParameter[] para = {
                                      new SqlParameter("@productGuid", productGuid),
                                      new SqlParameter("@code", code),
                                      new SqlParameter("@name", name),
                                      new SqlParameter("@displayName", displayName),
                                      new SqlParameter("@productType", productType),
                                      new SqlParameter("@imageURL", imageURL),
                                      new SqlParameter("@material", material),
                                      new SqlParameter("@colour", colour),
                                      new SqlParameter("@size", size),
                                      new SqlParameter("@currency", currency),
                                      new SqlParameter("@price", price),
                                      new SqlParameter("@sale", !sale.HasValue ? (object)DBNull.Value : (object)sale.Value),
                                      new SqlParameter("@createTime", createTime),
                                      new SqlParameter("@stock", stock),
                                      new SqlParameter("@isActive", isActive),
                                      new SqlParameter("@description", description),
                                      new SqlParameter("@remark", remark)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteProduct(Guid productGuid)
        {
            string sql = "DELETE dbo.iArsenal_Product WHERE productGuid = @productGuid";

            SqlParameter[] para = { new SqlParameter("@productGuid", productGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetProducts()
        {
            string sql = @"SELECT ProductGuid, Code, Name, DisplayName, ProductType, ImageURL, Material, Colour, Size, Currency, Price, Sale, CreateTime, Stock, IsActive, Description, Remark FROM iArsenal_Product ORDER BY Code";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
