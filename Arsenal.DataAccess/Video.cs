using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace Arsenal.DataAccess
{
    public class Video
    {
        public static DataRow GetVideoByID(Guid videoGuid)
        {
            string sql = "SELECT * FROM dbo.Arsenal_Video WHERE VideoGuid = @videoGuid";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@videoGuid", videoGuid));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0].Rows[0];
        }

        public static void UpdateVideo(Guid videoGuid, string fileName, Guid? arsenalMatchGuid, Guid? goalPlayerGuid, string goalPlayerName, Guid? assistPlayerGuid, string assistPlayerName, string goalRank, string teamworkRank, int videoLength, int videoWidth, int videoHeight, string goalYear, string opponent)
        {
            string sql = @"UPDATE dbo.Arsenal_Video SET FileName = @fileName, ArsenalMatchGuid = @arsenalMatchGuid, GoalPlayerGuid = @goalPlayerGuid, GoalPlayerName = @goalPlayerName, AssistPlayerGuid = @assistPlayerGuid, AssistPlayerName = @assistPlayerName, 
                               GoalRank = @goalRank, TeamworkRank = @teamworkRank, VideoLength = @videoLength, VideoWidth = @videoWidth, VideoHeight = @videoHeight, GoalYear = @goalYear, Opponent = @opponent WHERE VideoGuid = @videoGuid";

            SqlParameter[] para = new SqlParameter[14];

            para[0] = new SqlParameter("@videoGuid", videoGuid);
            para[1] = new SqlParameter("@fileName", fileName);
            para[2] = new SqlParameter("@arsenalMatchGuid", !arsenalMatchGuid.HasValue ? (object)DBNull.Value : (object)arsenalMatchGuid.Value);
            para[3] = new SqlParameter("@goalPlayerGuid", !goalPlayerGuid.HasValue ?  (object)DBNull.Value : (object)goalPlayerGuid.Value);
            para[4] = new SqlParameter("@goalPlayerName", string.IsNullOrEmpty(goalPlayerName) ? (object)DBNull.Value : (object)goalPlayerName);
            para[5] = new SqlParameter("@assistPlayerGuid", !assistPlayerGuid.HasValue ? (object)DBNull.Value : (object)assistPlayerGuid.Value);
            para[6] = new SqlParameter("@assistPlayerName", string.IsNullOrEmpty(assistPlayerName) ? (object)DBNull.Value : (object)assistPlayerName);
            para[7] = new SqlParameter("@goalRank", goalRank);
            para[8] = new SqlParameter("@teamworkRank", teamworkRank);
            para[9] = new SqlParameter("@videoLength", videoLength);
            para[10] = new SqlParameter("@videoWidth", videoWidth);
            para[11] = new SqlParameter("@videoHeight", videoHeight);
            para[12] = new SqlParameter("@goalYear", goalYear);
            para[13] = new SqlParameter("@opponent", opponent);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertVideo(Guid videoGuid, string fileName, Guid? arsenalMatchGuid, Guid? goalPlayerGuid, string goalPlayerName, Guid? assistPlayerGuid, string assistPlayerName, string goalRank, string teamworkRank, int videoLength, int videoWidth, int videoHeight, string goalYear, string opponent)
        {
            string sql = @"INSERT INTO dbo.Arsenal_Video (VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent) 
                               VALUES (@videoGuid, @fileName, @arsenalMatchGuid, @goalPlayerGuid, @goalPlayerName, @assistPlayerGuid, @assistPlayerName, @goalRank, @teamworkRank, @videoLength, @videoWidth, @videoHeight, @goalYear, @opponent)";

            SqlParameter[] para = new SqlParameter[14];

            para[0] = new SqlParameter("@videoGuid", videoGuid);
            para[1] = new SqlParameter("@fileName", fileName);
            para[2] = new SqlParameter("@arsenalMatchGuid", !arsenalMatchGuid.HasValue ? (object)DBNull.Value : (object)arsenalMatchGuid.Value);
            para[3] = new SqlParameter("@goalPlayerGuid", !goalPlayerGuid.HasValue ? (object)DBNull.Value : (object)goalPlayerGuid.Value);
            para[4] = new SqlParameter("@goalPlayerName", string.IsNullOrEmpty(goalPlayerName) ? (object)DBNull.Value : (object)goalPlayerName);
            para[5] = new SqlParameter("@assistPlayerGuid", !assistPlayerGuid.HasValue ? (object)DBNull.Value : (object)assistPlayerGuid.Value);
            para[6] = new SqlParameter("@assistPlayerName", string.IsNullOrEmpty(assistPlayerName) ? (object)DBNull.Value : (object)assistPlayerName);
            para[7] = new SqlParameter("@goalRank", goalRank);
            para[8] = new SqlParameter("@teamworkRank", teamworkRank);
            para[9] = new SqlParameter("@videoLength", videoLength);
            para[10] = new SqlParameter("@videoWidth", videoWidth);
            para[11] = new SqlParameter("@videoHeight", videoHeight);
            para[12] = new SqlParameter("@goalYear", goalYear);
            para[13] = new SqlParameter("@opponent", opponent);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void DeleteVideo(Guid videoGuid)
        {
            string sql = "DELETE dbo.Arsenal_Video WHERE VideoGuid = @videoGuid";

            SqlParameter[] para = { new SqlParameter("@videoGuid", videoGuid) };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static DataTable GetVideos()
        {
            string sql = @"SELECT VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent 
                                FROM dbo.Arsenal_Video ORDER BY GoalYear DESC, GoalRank DESC, TeamworkRank DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }

        public static DataTable GetVideoDistColumn(string col, bool order)
        {
            string sql = string.Format("SELECT DISTINCT {0} FROM dbo.Arsenal_Video WHERE {0} <> '' ORDER BY {0} {1}", col, order ? string.Empty : "DESC");

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds.Tables[0];
        }
    }
}
