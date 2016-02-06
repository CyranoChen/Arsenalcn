using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.Common.DataAccess
{
    public class DictionaryItem
    {
        public static DataRow GetDictionaryItemByID(int itemID)
        {
            var sql = "SELECT * FROM dbo.Arsenalcn_DictionaryItem WHERE ID = @itemID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql,
                new SqlParameter("@itemID", itemID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0].Rows[0];
        }

        public static void UpdateDictionaryItem(int itemID, int dictID, string code, string name, string description,
            string customCode, string spell, string shortSpell, int parentID, int orderNum)
        {
            var sql =
                @"UPDATE dbo.Arsenalcn_DictionaryItem SET DictionaryID = @dictID, Code = @code, [Name] = @name, Description = @description, CustomCode = @customCode,
                            Spell = @spell, ShortSpell = @shortSpell, ParentID = @parentID, OrderNum = @orderNum WHERE ID = @itemID";

            SqlParameter[] para =
            {
                new SqlParameter("@itemID", itemID), new SqlParameter("@dictID", dictID),
                new SqlParameter("@code", code), new SqlParameter("@name", name),
                new SqlParameter("@description", description), new SqlParameter("@customCode", customCode),
                new SqlParameter("@spell", spell), new SqlParameter("@shortSpell", shortSpell),
                new SqlParameter("parentID", parentID), new SqlParameter("@orderNum", orderNum)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertDictionaryItem(int dictID, string code, string name, string description,
            string customCode, string spell, string shortSpell, int parentID, int orderNum)
        {
            var sql =
                @"INSERT INTO dbo.Arsenalcn_DictionaryItem (DictionaryID, Code, [Name], Description, CustomCode, Spell, ShortSpell, ParentID, OrderNum) 
                               VALUES (@dictID, @code, @name, @description, @customCode, @spell, @shortSpell, @parentID, @orderNum)";

            SqlParameter[] para =
            {
                new SqlParameter("@dictID", dictID), new SqlParameter("@code", code),
                new SqlParameter("@name", name), new SqlParameter("@description", description),
                new SqlParameter("@customCode", customCode), new SqlParameter("@spell", spell),
                new SqlParameter("@shortSpell", shortSpell), new SqlParameter("parentID", parentID),
                new SqlParameter("@orderNum", orderNum)
            };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteDictionaryItem(int itemID)
        {
            var sql = "DELETE dbo.Arsenalcn_DictionaryItem WHERE ID = @itemID";

            SqlParameter[] para = {new SqlParameter("@itemID", itemID)};

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetDictionaryItems()
        {
            var sql =
                "SELECT ID, DictionaryID, Code, Name, Description, CustomCode, Spell, ShortSpell, ParentID, OrderNum FROM Arsenalcn_DictionaryItem ORDER BY ID";

            var ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            return ds.Tables[0];
        }
    }
}