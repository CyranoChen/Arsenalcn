using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Common.DataAccess
{
    public class Dictionary
    {
        public static DataRow GetDictionaryByID(int dictID)
        {
            var sql = "SELECT * FROM dbo.Arsenalcn_Dictionary WHERE ID = @dictID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@dictID", dictID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void UpdateDictionary(int dictID, string name, string displayName, string standardLevel,
            string businessField, string standardCode, bool isTreeDictionary, string description)
        {
            var sql =
                @"UPDATE dbo.Arsenalcn_Dictionary SET [Name] = @name, DisplayName = @displayName, StandardLevel = @standardLevel, BusinessField = @businessField, 
                            StandardCode = @standardCode, IsTreeDictionary = @isTreeDictionary, Description = @description WHERE ID = @dictID";

            SqlParameter[] para =
            {
                new SqlParameter("@dictID", dictID), new SqlParameter("@name", name),
                new SqlParameter("@displayName", displayName), new SqlParameter("@standardLevel", standardLevel),
                new SqlParameter("@businessField", businessField), new SqlParameter("@standardCode", standardCode),
                new SqlParameter("@isTreeDictionary", isTreeDictionary), new SqlParameter("@description", description)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertDictionary(string name, string displayName, string standardLevel, string businessField,
            string standardCode, bool isTreeDictionary, string description)
        {
            var sql =
                @"INSERT INTO dbo.Arsenalcn_Dictionary ([Name], DisplayName, StandardLevel, BusinessField, StandardCode, IsTreeDictionary, Description) 
                               VALUES (@name, @displayName, @standardLevel, @businessField, @standardCode, @isTreeDictionary, @description)";

            SqlParameter[] para =
            {
                new SqlParameter("@name", name), new SqlParameter("@displayName", displayName),
                new SqlParameter("@standardLevel", standardLevel), new SqlParameter("@businessField", businessField),
                new SqlParameter("@standardCode", standardCode), new SqlParameter("@isTreeDictionary", isTreeDictionary),
                new SqlParameter("@description", description)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteDictionary(int dictID)
        {
            var sql = "DELETE dbo.Arsenalcn_Dictionary WHERE ID = @dictID";

            SqlParameter[] para = {new SqlParameter("@dictID", dictID)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetDictionaries()
        {
            var sql =
                "SELECT ID, Name, DisplayName, StandardLevel, BusinessField, StandardCode, IsTreeDictionary, Description FROM dbo.Arsenalcn_Dictionary ORDER BY ID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }
}