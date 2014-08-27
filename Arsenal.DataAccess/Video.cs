using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.Common;
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

        public static void UpdateVideo(Guid videoGuid, string fileName, Guid? arsenalMatchGuid, Guid? goalPlayerGuid, string goalPlayerName, Guid? assistPlayerGuid, string assistPlayerName, string goalRank, string teamworkRank, string videoType, int videoLength, int videoWidth, int videoHeight, string goalYear, string opponent)
        {
            string sql = @"UPDATE dbo.Arsenal_Video SET FileName = @fileName, ArsenalMatchGuid = @arsenalMatchGuid, GoalPlayerGuid = @goalPlayerGuid, GoalPlayerName = @goalPlayerName, AssistPlayerGuid = @assistPlayerGuid, AssistPlayerName = @assistPlayerName, 
                               GoalRank = @goalRank, TeamworkRank = @teamworkRank, VideoType = @videoType, VideoLength = @videoLength, VideoWidth = @videoWidth, VideoHeight = @videoHeight, GoalYear = @goalYear, Opponent = @opponent WHERE VideoGuid = @videoGuid";

            SqlParameter[] para = { 
                                      new SqlParameter("@videoGuid", videoGuid),
                                      new SqlParameter("@fileName", fileName),
                                      new SqlParameter("@arsenalMatchGuid", !arsenalMatchGuid.HasValue ? (object)DBNull.Value : (object)arsenalMatchGuid.Value),
                                      new SqlParameter("@goalPlayerGuid", !goalPlayerGuid.HasValue ? (object)DBNull.Value : (object)goalPlayerGuid.Value),
                                      new SqlParameter("@goalPlayerName", string.IsNullOrEmpty(goalPlayerName) ? (object)DBNull.Value : (object)goalPlayerName),
                                      new SqlParameter("@assistPlayerGuid", !assistPlayerGuid.HasValue ? (object)DBNull.Value : (object)assistPlayerGuid.Value),
                                      new SqlParameter("@assistPlayerName", string.IsNullOrEmpty(assistPlayerName) ? (object)DBNull.Value : (object)assistPlayerName),
                                      new SqlParameter("@goalRank", goalRank),
                                      new SqlParameter("@teamworkRank", teamworkRank),
                                      new SqlParameter("@videoType", videoType),
                                      new SqlParameter("@videoLength", videoLength),
                                      new SqlParameter("@videoWidth", videoWidth),
                                      new SqlParameter("@videoHeight", videoHeight),
                                      new SqlParameter("@goalYear", goalYear),
                                      new SqlParameter("@opponent", opponent)
                                  };

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void InsertVideo(Guid videoGuid, string fileName, Guid? arsenalMatchGuid, Guid? goalPlayerGuid, string goalPlayerName, Guid? assistPlayerGuid, string assistPlayerName, string goalRank, string teamworkRank, string videoType, int videoLength, int videoWidth, int videoHeight, string goalYear, string opponent)
        {
            string sql = @"INSERT INTO dbo.Arsenal_Video (VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName, GoalRank, TeamworkRank, VideoType, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent) 
                               VALUES (@videoGuid, @fileName, @arsenalMatchGuid, @goalPlayerGuid, @goalPlayerName, @assistPlayerGuid, @assistPlayerName, @goalRank, @teamworkRank, @videoType, @videoLength, @videoWidth, @videoHeight, @goalYear, @opponent)";

            SqlParameter[] para = { 
                                      new SqlParameter("@videoGuid", videoGuid),
                                      new SqlParameter("@fileName", fileName),
                                      new SqlParameter("@arsenalMatchGuid", !arsenalMatchGuid.HasValue ? (object)DBNull.Value : (object)arsenalMatchGuid.Value),
                                      new SqlParameter("@goalPlayerGuid", !goalPlayerGuid.HasValue ? (object)DBNull.Value : (object)goalPlayerGuid.Value),
                                      new SqlParameter("@goalPlayerName", string.IsNullOrEmpty(goalPlayerName) ? (object)DBNull.Value : (object)goalPlayerName),
                                      new SqlParameter("@assistPlayerGuid", !assistPlayerGuid.HasValue ? (object)DBNull.Value : (object)assistPlayerGuid.Value),
                                      new SqlParameter("@assistPlayerName", string.IsNullOrEmpty(assistPlayerName) ? (object)DBNull.Value : (object)assistPlayerName),
                                      new SqlParameter("@goalRank", goalRank),
                                      new SqlParameter("@teamworkRank", teamworkRank),
                                      new SqlParameter("@videoType", videoType),
                                      new SqlParameter("@videoLength", videoLength),
                                      new SqlParameter("@videoWidth", videoWidth),
                                      new SqlParameter("@videoHeight", videoHeight),
                                      new SqlParameter("@goalYear", goalYear),
                                      new SqlParameter("@opponent", opponent)
                                  };

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
            string sql = @"SELECT VideoGuid, FileName, ArsenalMatchGuid, GoalPlayerGuid, GoalPlayerName, AssistPlayerGuid, AssistPlayerName,
                                              GoalRank, TeamworkRank, VideoType, VideoLength, VideoWidth, VideoHeight, GoalYear, Opponent 
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
