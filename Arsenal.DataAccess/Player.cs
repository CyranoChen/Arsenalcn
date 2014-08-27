using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class Player
    {
        public static DataRow GetPlayerByID(Guid playerGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_Player WHERE PlayerGuid = @playerGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@playerGuid", playerGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdatePlayer(Guid playerGuid, string firstName, string lastName, string displayName, string printingName, string position, int squadNumber, string faceURL, string photoURL, int offset, Boolean isLegend, Boolean isLoan, DateTime? birthday, string born, int starts, int subs, int apps, int goals, DateTime? joinDate, string joined, string left, string debut, string firstGoal, string previousClubs, string profile)
        {
            string sql = @"UPDATE dbo.Arsenal_Player SET FirstName = @firstName, LastName = @lastName, DisplayName = @displayName, PrintingName = @printingName, Position = @position, SquadNumber = @squadNumber, FaceURL = @faceURL, PhotoURL = @photoURL, Offset = @offset, IsLegend = @isLegend, IsLoan = @isLoan,
                               Birthday = @birthday, Born = @born, Starts = @starts, Subs = @subs, Apps = @apps, Goals = @goals, JoinDate = @joinDate, Joined = @joined, [Left] = @left, Debut = @debut, FirstGoal = @firstGoal, PreviousClubs = @previousClubs, Profile = @profile WHERE PlayerGuid = @playerGuid";

            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter("@playerGuid", playerGuid);
            para[1] = new SqlParameter("@firstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : (object)firstName);
            para[2] = new SqlParameter("@lastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : (object)lastName);
            para[3] = new SqlParameter("@displayName", displayName);
            para[4] = new SqlParameter("@printingName", string.IsNullOrEmpty(printingName) ? (object)DBNull.Value : (object)printingName);
            para[5] = new SqlParameter("@position", position);
            para[6] = new SqlParameter("@squadNumber", squadNumber);
            para[7] = new SqlParameter("@faceURL", faceURL);
            para[8] = new SqlParameter("@photoURL", photoURL);
            para[9] = new SqlParameter("@offset", offset);
            para[10] = new SqlParameter("@isLegend", isLegend);
            para[11] = new SqlParameter("@isLoan", isLoan);
            para[12] = new SqlParameter("@birthday", !birthday.HasValue ? (object)DBNull.Value : (object)birthday.Value);
            para[13] = new SqlParameter("@born", born);
            para[14] = new SqlParameter("@starts", starts);
            para[15] = new SqlParameter("@subs", subs);
            para[16] = new SqlParameter("@apps", apps);
            para[17] = new SqlParameter("@goals", goals);
            para[18] = new SqlParameter("@joinDate", !joinDate.HasValue ? (object)DBNull.Value : (object)joinDate.Value);
            para[19] = new SqlParameter("@joined", joined);
            para[20] = new SqlParameter("@left", left);
            para[21] = new SqlParameter("@debut", debut);
            para[22] = new SqlParameter("@firstGoal", firstGoal);
            para[23] = new SqlParameter("@previousClubs", previousClubs);
            para[24] = new SqlParameter("@profile", profile);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertPlayer(Guid playerGuid, string firstName, string lastName, string displayName, string printingName, string position, int squadNumber, string faceURL, string photoURL, int offset, Boolean isLegend, Boolean isLoan, DateTime? birthday, string born, int starts, int subs, int apps, int goals, DateTime? joinDate, string joined, string left, string debut, string firstGoal, string previousClubs, string profile)
        {
            string sql = @"INSERT INTO dbo.Arsenal_Player (PlayerGuid, FirstName, LastName, DisplayName, PrintingName, Position, SquadNumber, FaceURL, PhotoURL, Offset, IsLegend, IsLoan, Birthday, Born, Starts, Subs, Apps, Goals, JoinDate, Joined, [Left], Debut, FirstGoal, PreviousClubs, Profile) 
                               VALUES (@playerGuid, @firstName, @lastName, @displayName, @printingName, @position, @squadNumber, @faceURL, @photoURL, @offset, @isLegend, @isLoan, @birthday, @born, @starts, @subs, @apps, @goals, @joinDate, @joined, @left, @debut, @firstGoal, @previousClubs, @profile)";

            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter("@playerGuid", playerGuid);
            para[1] = new SqlParameter("@firstName", string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : (object)firstName);
            para[2] = new SqlParameter("@lastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : (object)lastName);
            para[3] = new SqlParameter("@displayName", displayName);
            para[4] = new SqlParameter("@printingName", string.IsNullOrEmpty(printingName) ? (object)DBNull.Value : (object)printingName);
            para[5] = new SqlParameter("@position", position);
            para[6] = new SqlParameter("@squadNumber", squadNumber);
            para[7] = new SqlParameter("@faceURL", faceURL);
            para[8] = new SqlParameter("@photoURL", photoURL);
            para[9] = new SqlParameter("@offset", offset);
            para[10] = new SqlParameter("@isLegend", isLegend);
            para[11] = new SqlParameter("@isLoan", isLoan);
            para[12] = new SqlParameter("@birthday", !birthday.HasValue ? (object)DBNull.Value : (object)birthday.Value);
            para[13] = new SqlParameter("@born", born);
            para[14] = new SqlParameter("@starts", starts);
            para[15] = new SqlParameter("@subs", subs);
            para[16] = new SqlParameter("@apps", apps);
            para[17] = new SqlParameter("@goals", goals);
            para[18] = new SqlParameter("@joinDate", !joinDate.HasValue ? (object)DBNull.Value : (object)joinDate.Value);
            para[19] = new SqlParameter("@joined", joined);
            para[20] = new SqlParameter("@left", left);
            para[21] = new SqlParameter("@debut", debut);
            para[22] = new SqlParameter("@firstGoal", firstGoal);
            para[23] = new SqlParameter("@previousClubs", previousClubs);
            para[24] = new SqlParameter("@profile", profile);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeletePlayer(Guid playerGuid)
        {
            string sql = "DELETE dbo.Arsenal_Player WHERE PlayerGuid = @playerGuid";

            SqlParameter[] para = { new SqlParameter("@playerGuid", playerGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetPlayers()
        {
            string sql = @"SELECT PlayerGuid, FirstName, LastName, DisplayName, PrintingName, Position, SquadNumber, FaceURL, PhotoURL, Offset, IsLegend, IsLoan, 
                                  Birthday, Born, Starts, Subs, Apps, Goals, JoinDate, Joined, [Left], Debut, FirstGoal, PreviousClubs, Profile
                                  FROM dbo.Arsenal_Player ORDER BY IsLegend, IsLoan, SquadNumber, LastName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetPlayerDistColumn(string col, bool order)
        {
            string sql = string.Format("SELECT DISTINCT {0} FROM dbo.Arsenal_Player WHERE {0} <> '' ORDER BY {0} {1}", col, order ? string.Empty : "DESC");

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
