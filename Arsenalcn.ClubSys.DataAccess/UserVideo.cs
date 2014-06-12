using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.DataAccess
{
    public class UserVideo
    {
        public static void SetUserVideoPublic(int id, bool current)
        {
            string sql = "UPDATE dbo.AcnClub_RelationUserVideo SET IsPublic = @current WHERE [ID] = @id";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@id", id));
                com.Parameters.Add(new SqlParameter("@current", current));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }

        public static DataTable GetUserVideo(int userID)
        {
            string sql = "SELECT * FROM vw_AcnClub_UserVideoInfo WHERE [UserID] = @userID ORDER BY ActiveDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                return dt;
            }
        }

        public static DataView GetUserPublicVideo(int userID)
        {
            DataTable dt = GetUserVideo(userID);

            DataView dv = dt.DefaultView;
            dv.RowFilter = "IsPublic = 1";

            return dv;
        }

        public static DataTable GetUserVideoByClubID(int clubID)
        {
            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "[dbo].[GetUserClubVideo]";

                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                SqlDataAdapter sda = new SqlDataAdapter(com);

                DataTable dtClubVideo = new DataTable();

                con.Open();
                sda.Fill(dtClubVideo);
                con.Close();

                if (dtClubVideo.Rows.Count == 0)
                    return null;
                else
                    return dtClubVideo;
            }
        }

        public static DataRow GetVideoInfoByUserVideoID(int userVideoID)
        {
            string sql = "SELECT * FROM vw_AcnClub_UserVideoInfo WHERE [ID] = @userVideoID";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userVideoID", userVideoID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return dt.Rows[0];
                }
            }
        }

        public static DataRow GetVideoInfoByVideoGuid(Guid guid)
        {
            string sql = "SELECT * FROM vw_Arsenal_VideoInfo WHERE [VideoGuid] = @videoGuid";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@videoGuid", guid));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return dt.Rows[0];
                }
            }
        }

        public static Guid? GetRandomVideo(int userID, int minLv, int maxLv, bool? isLegend)
        {
            string sql = "SELECT TOP 1 VideoGuid FROM dbo.vw_Arsenal_VideoInfo WHERE VideoGuid NOT IN (SELECT VideoGuid FROM AcnClub_RelationUserVideo WHERE UserID = @userID) AND GoalRank BETWEEN @minLv AND @maxLv AND IsLegend=@isLegend ORDER BY NEWID() ";

            if (!isLegend.HasValue)
            {
                sql = "SELECT TOP 1 VideoGuid FROM dbo.vw_Arsenal_VideoInfo WHERE GoalRank BETWEEN @minLv AND @maxLv ORDER BY NEWID()";
            }

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@minLv", minLv));
                com.Parameters.Add(new SqlParameter("@maxLv", maxLv));

                if (isLegend.HasValue)
                {
                    com.Parameters.Add(new SqlParameter("@isLegend", isLegend));
                    com.Parameters.Add(new SqlParameter("@userID", userID));
                }

                con.Open();

                Guid? guid = null;

                try
                {
                    guid = (Guid)com.ExecuteScalar();
                }
                catch
                { }

                con.Close();

                return guid;
            }
        }

        public static string ActiveVideo(int userID, string userName, int userNumberID)
        {
            Guid? guid = GetRandomVideo(userID, 1, 3, true);

            if (!guid.HasValue)
                throw new Exception("No Video Available.");

            string sql = "INSERT INTO AcnClub_RelationUserVideo (UserID, UserName, VideoGuid, ActiveDate, UserDesc, IsPublic) VALUES (@userID, @userName, @guid, getdate(), null, 0) ;DELETE FROM AcnClub_Card WHERE [ID] = @userNumberID ;UPDATE dbo.AcnClub_Player SET Shirt = Shirt - 5, Shorts = Shorts - 5, Sock = Sock - 5 WHERE UserID = @userID;";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userName", userName));
                com.Parameters.Add(new SqlParameter("@guid", guid.Value));
                com.Parameters.Add(new SqlParameter("@userNumberID", userNumberID));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }

            DataRow dr = GetVideoInfoByVideoGuid(guid.Value);

            return dr["PhotoUrl"].ToString();
        }

        public static void ConsolidateCards(int userID, string userName, int userNumID1, int userNumID2)
        {
            string sql = "DELETE FROM AcnClub_Card WHERE [ID] = @userNum1 OR [ID] = @userNum2 ; INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, 0, 0, getdate(), null, null);";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userName", userName));
                com.Parameters.Add(new SqlParameter("@userNum1", userNumID1));
                com.Parameters.Add(new SqlParameter("@userNum2", userNumID2));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }

        public static void InsertActiveVideo(int userid, string username, Guid videoGuid)
        {
            string sql = "INSERT INTO AcnClub_RelationUserVideo (UserID, UserName, VideoGuid, ActiveDate, UserDesc, IsPublic) VALUES (@userID, @userName, @guid, getdate(), null, 0) ;";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userid));
                com.Parameters.Add(new SqlParameter("@userName", username));
                com.Parameters.Add(new SqlParameter("@guid", videoGuid));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }

        public static Guid? SetDailyVideo()
        {
            Guid? guid = UserVideo.GetRandomVideo(-1, 4, 5, null);
            if ((guid.HasValue) && (ConfigGlobal.DailyVideoActive == true))
            {
                if (ConfigGlobal.DailyVideoGuid == guid.Value.ToString())
                    guid = UserVideo.GetRandomVideo(-1, 4, 5, null);

                return guid;
            }
            else
            {
                return null;
            }
        }
    }
}
