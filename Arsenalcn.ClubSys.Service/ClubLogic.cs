using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;
using Microsoft.ApplicationBlocks.Data;

namespace Arsenalcn.ClubSys.Service
{
    public class ClubLogic
    {
        internal static object ParseNullableParam(object nullable)
        {
            if (nullable == null)
                return DBNull.Value;
            else
                return nullable;
        }

        public static void InsertClub(Club club)
        {
            string sql = @"INSERT INTO dbo.AcnClub_Club VALUES (@fullName, @shortName, @rankLevel, @rankScore, @logo, @slogan, @description, @creatorUid,
		                       @creatorUserName, @managerUid, @managerUserName, @createDate, @updateDate, @isActive,
		                       @isAppliable, @fortune, @memberCredit, @memberFortune, @memberRP, @memberLoyalty)";

            SqlParameter[] para = new SqlParameter[21];

            para[0] = new SqlParameter("@clubID", ParseNullableParam(club.ID));
            para[1] = new SqlParameter("@fullName", club.FullName);
            para[2] = new SqlParameter("@shortName", club.ShortName);
            para[3] = new SqlParameter("@rankLevel", club.RankLevel);
            para[4] = new SqlParameter("@rankScore", club.RankScore);
            para[5] = new SqlParameter("@logo", ParseNullableParam(club.LogoName));
            para[6] = new SqlParameter("@slogan", club.Slogan);
            para[7] = new SqlParameter("@description", ParseNullableParam(club.Description));
            para[8] = new SqlParameter("@creatorUid", club.CreatorUid);
            para[9] = new SqlParameter("@creatorUserName", club.CreatorUserName);
            para[10] = new SqlParameter("@managerUid", club.ManagerUid);
            para[11] = new SqlParameter("@managerUserName", club.ManagerUserName);
            para[12] = new SqlParameter("@createDate", club.CreateDate);
            para[13] = new SqlParameter("@updateDate", club.UpdateDate);
            para[14] = new SqlParameter("@isActive", ParseNullableParam(club.IsActive));
            para[15] = new SqlParameter("@isAppliable", ParseNullableParam(club.IsAppliable));
            para[16] = new SqlParameter("@fortune", ParseNullableParam(club.Fortune));
            para[17] = new SqlParameter("@memberCredit", ParseNullableParam(club.MemberCredit));
            para[18] = new SqlParameter("@memberFortune", ParseNullableParam(club.MemberFortune));
            para[19] = new SqlParameter("@memberRP", ParseNullableParam(club.MemberRP));
            para[20] = new SqlParameter("@memberLoyalty", ParseNullableParam(club.MemberLoyalty));

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        public static void SaveClub(Club club)
        {
            string sql = @"UPDATE dbo.AcnClub_Club SET FullName = @fullName, ShortName = @shortName, RankLevel = @rankLevel, RankScore = @rankScore,
		                        Logo = @logo, Slogan = @slogan, Description = @description, CreatorUid = @creatorUid, CreatorUserName = @creatorUserName, 
                                ManagerUid = @managerUid, ManagerUserName = @managerUserName, CreateDate = @createDate, UpdateDate = @updateDate, 
                                IsActive = @isActive, IsAppliable = @isAppliable, Fortune = @fortune, MemberCredit = @memberCredit, MemberFortune = @memberFortune,
		                        MemberRP = @memberRP, MemberLoyalty = @memberLoyalty WHERE ClubUid = @clubID";

            SqlParameter[] para = new SqlParameter[21];

            para[0] = new SqlParameter("@clubID", ParseNullableParam(club.ID));
            para[1] = new SqlParameter("@fullName", club.FullName);
            para[2] = new SqlParameter("@shortName", club.ShortName);
            para[3] = new SqlParameter("@rankLevel", club.RankLevel);
            para[4] = new SqlParameter("@rankScore", club.RankScore);
            para[5] = new SqlParameter("@logo", ParseNullableParam(club.LogoName));
            para[6] = new SqlParameter("@slogan", club.Slogan);
            para[7] = new SqlParameter("@description", ParseNullableParam(club.Description));
            para[8] = new SqlParameter("@creatorUid", club.CreatorUid);
            para[9] = new SqlParameter("@creatorUserName", club.CreatorUserName);
            para[10] = new SqlParameter("@managerUid", club.ManagerUid);
            para[11] = new SqlParameter("@managerUserName", club.ManagerUserName);
            para[12] = new SqlParameter("@createDate", club.CreateDate);
            para[13] = new SqlParameter("@updateDate", club.UpdateDate);
            para[14] = new SqlParameter("@isActive", ParseNullableParam(club.IsActive));
            para[15] = new SqlParameter("@isAppliable", ParseNullableParam(club.IsAppliable));
            para[16] = new SqlParameter("@fortune", ParseNullableParam(club.Fortune));
            para[17] = new SqlParameter("@memberCredit", ParseNullableParam(club.MemberCredit));
            para[18] = new SqlParameter("@memberFortune", ParseNullableParam(club.MemberFortune));
            para[19] = new SqlParameter("@memberRP", ParseNullableParam(club.MemberRP));
            para[20] = new SqlParameter("@memberLoyalty", ParseNullableParam(club.MemberLoyalty));

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        internal static void SaveClubHistory(ClubHistory ch)
        {
            string sql = "INSERT INTO dbo.AcnClub_LogClub VALUES (@clubID, @actionType, GETDATE(), @actionUserName, @OperatorUserName, @ActionDescription)";

            SqlParameter[] para = new SqlParameter[5];

            para[0] = new SqlParameter("@clubID", ch.ClubID);
            para[1] = new SqlParameter("@actionType", ch.ActionType);
            para[2] = new SqlParameter("@actionUserName", ch.ActionUserName);
            para[3] = new SqlParameter("@operatorUserName", ch.OperatorUserName);
            para[4] = new SqlParameter("@actionDescription", ch.ActionDescription);

            SqlHelper.ExecuteNonQuery(SQLConn.GetConnection(), CommandType.Text, sql, para);
        }

        internal static void SaveUserClub(UserClub uc)
        {
            using (SqlConnection con = SQLConn.GetConnection())
            {

                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "[dbo].[SaveUserClub]";

                com.Parameters.Add(new SqlParameter("@ID", ParseNullableParam(uc.ID)));
                com.Parameters.Add(new SqlParameter("@userID", uc.Userid));
                com.Parameters.Add(new SqlParameter("@userName", uc.UserName));
                com.Parameters.Add(new SqlParameter("@clubUid", uc.ClubUid));
                com.Parameters.Add(new SqlParameter("@responsibility", uc.Responsibility));
                com.Parameters.Add(new SqlParameter("@fromDate", uc.FromDate));
                com.Parameters.Add(new SqlParameter("@toDate", ParseNullableParam(uc.ToDate)));
                com.Parameters.Add(new SqlParameter("@joinClubDate", ParseNullableParam(uc.JoinClubDate)));
                com.Parameters.Add(new SqlParameter("@isActive", uc.IsActive));

                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
        }

        internal static void SaveApplyHistory(ApplyHistory ah)
        {
            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "[dbo].[SaveApplyHistory]";

                com.Parameters.Add(new SqlParameter("@ID", ParseNullableParam(ah.ID)));
                com.Parameters.Add(new SqlParameter("@userID", ah.Userid));
                com.Parameters.Add(new SqlParameter("@userName", ah.UserName));
                com.Parameters.Add(new SqlParameter("@clubUid", ah.ClubUid));
                com.Parameters.Add(new SqlParameter("@applyDate", ah.ApplyDate));
                com.Parameters.Add(new SqlParameter("@isAccepted", ParseNullableParam(ah.IsAccepted)));

                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
        }

        public static Club GetClubInfo(int id)
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club WHERE ClubUid = @id";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@id", id));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new Club(ds.Tables[0].Rows[0]);
        }

        public static Club GetClubInfo(string fullName)
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club WHERE FullName = @fullName";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@fullName", fullName));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new Club(ds.Tables[0].Rows[0]);
        }

        public static Club GetCreateClubApplicationByUserID(int userID)
        {
            string sql = "SELECT TOP 1 * FROM dbo.AcnClub_Club WHERE ManagerUid = @userID AND IsActive IS NULL";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new Club(ds.Tables[0].Rows[0]);
        }

        internal static ApplyHistory GetActiveApplyHistoryByUserClub(int userID, int clubID)
        {
            string sql = "SELECT TOP 1 * FROM dbo.AcnClub_Application WHERE UserID = @userID AND ClubUid = @clubID AND IsAccepted IS NULL";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID), new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new ApplyHistory(ds.Tables[0].Rows[0]);
        }

        public static ApplyHistory GetApplyHistory(int id)
        {
            string sql = "SELECT * FROM dbo.AcnClub_Application WHERE ID = @id";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@id", id));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new ApplyHistory(ds.Tables[0].Rows[0]);
        }

        public static UserClub GetActiveUserClub(int userID, int clubID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_RelationUserClub	WHERE UserID = @userID AND ClubUid = @clubID AND IsActive = 1";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID), new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return new UserClub(ds.Tables[0].Rows[0]);
        }

        public static List<Club> GetUserManagedClubs(int userID)
        {
            string sql = @"SELECT DISTINCT c.*, uc.Responsibility AS additional FROM dbo.AcnClub_RelationUserClub uc 
                               INNER JOIN dbo.AcnClub_Club c ON uc.ClubUid = c.ClubUid 
                               WHERE c.IsActive = 1 AND uc.UserID = @userId AND uc.IsActive = 1 AND Responsibility <20";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Club club = new Club(dr);

                    if (club.AdditionalData != null && club.AdditionalData != DBNull.Value)
                        club.AdditionalData = TranslateResponsibility((int)club.AdditionalData);
                    else
                        club.AdditionalData = TranslateResponsibility(Responsibility.Manager);

                    list.Add(club);
                }
                return list;
            }
        }

        public static List<Club> GetActiveUserClubs(int userID)
        {
            string sql = @"	SELECT c.* FROM dbo.AcnClub_RelationUserClub uc
	                            INNER JOIN dbo.AcnClub_Club c ON uc.ClubUid = c.ClubUid
	                            WHERE UserID = @userId AND uc.IsActive = 1 AND c.IsActive = 1";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Club(dr));
                }
                return list;
            }
        }

        public static List<ClubHistory> GetClubHistory()
        {
            string sql = "SELECT * FROM dbo.AcnClub_LogClub ORDER BY ActionDate DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<ClubHistory>();
            else
            {
                List<ClubHistory> list = new List<ClubHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ClubHistory(dr));
                }
                return list;
            }
        }

        public static List<ClubHistory> GetClubHistory(int clubID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_LogClub WHERE ClubID = @clubID ORDER BY ActionDate DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<ClubHistory>();
            else
            {
                List<ClubHistory> list = new List<ClubHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ClubHistory(dr));
                }
                return list;
            }
        }

        public static List<ClubHistory> GetUserClubHistory(string userName)
        {
            string sql = "SELECT * FROM dbo.AcnClub_LogClub WHERE ActionUserName = @userName OR OperatorUserName = @userName ORDER BY ActionDate DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userName", userName));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<ClubHistory>();
            else
            {
                List<ClubHistory> list = new List<ClubHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ClubHistory(dr));
                }
                return list;
            }
        }

        public static List<UserClub> GetClubMembers(int clubID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_RelationUserClub WHERE ClubUid = @clubID AND isActive = 1 ORDER BY JoinClubDate";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<UserClub>();
            else
            {
                List<UserClub> list = new List<UserClub>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new UserClub(dr));
                }
                return list;
            }
        }

        public static List<UserClub> GetClubLeads(int clubID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_RelationUserClub WHERE ClubUid = @clubID AND isActive = 1 AND Responsibility < 20 ORDER BY Responsibility";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<UserClub>();
            else
            {
                List<UserClub> list = new List<UserClub>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new UserClub(dr));
                }
                return list;
            }
        }

        public static List<ApplyHistory> GetClubApplications(int clubID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_Application WHERE ClubUid = @clubId AND IsAccepted IS NULL";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<ApplyHistory>();
            else
            {
                List<ApplyHistory> list = new List<ApplyHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ApplyHistory(dr));
                }
                return list;
            }
        }

        public static List<ApplyHistory> GetActiveUserApplications(int userID)
        {
            string sql = "SELECT * FROM dbo.AcnClub_Application WHERE UserID = @userID AND IsAccepted IS NULL";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@userID", userID));

            if (ds.Tables[0].Rows.Count == 0)
                return new List<ApplyHistory>();
            else
            {
                List<ApplyHistory> list = new List<ApplyHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ApplyHistory(dr));
                }
                return list;
            }
        }

        public static List<Club> GetActiveClubs()
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club WhERE IsActive = 1 ORDER BY RankScore DESC, RankLevel DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Club club = new Club(dr);

                    if (club.IsActive.HasValue && club.IsActive.Value)
                        list.Add(club);
                }
                return list;
            }
        }

        public static List<Club> GetAllClubs()
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club WhERE IsActive = 1 ORDER BY RankScore DESC, RankLevel DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Club(dr));
                }
                return list;
            }
        }

        public static List<Club> GetTopRankClubs()
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club	WHERE IsActive = 1 ORDER BY RankScore DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();

                int rank = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Club club = new Club(dr);

                    club.AdditionalData = string.Format("Top{0}", rank);

                    list.Add(club);
                }
                return list;
            }
        }

        public static List<Club> GetTopLvClubs()
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club ORDER BY RankLevel DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();

                int rank = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Club club = new Club(dr);

                    club.AdditionalData = string.Format("Top{0}", rank);

                    list.Add(club);
                }
                return list;
            }
        }

        public static List<Club> GetTopFortuneClubs()
        {
            string sql = "SELECT * FROM dbo.AcnClub_Club ORDER BY Fortune DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();

                int rank = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Club club = new Club(dr);

                    club.AdditionalData = string.Format("Top{0}", rank);

                    list.Add(club);
                }
                return list;
            }
        }

        public static List<Club> GetLatestClubs()
        {
            string sql = "SELECT TOP 5 *	FROM dbo.AcnClub_Club WHERE IsActive = 1 ORDER BY CreateDate DESC";

            DataSet ds = SqlHelper.ExecuteDataset(SQLConn.GetConnection(), CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count == 0)
                return new List<Club>();
            else
            {
                List<Club> list = new List<Club>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Club(dr));
                }
                return list;
            }
        }

        public static int GetActiveUserCount()
        {
            string sql = "SELECT COUNT(DISTINCT UserID) FROM dbo.AcnClub_RelationUserClub uc	WHERE uc.IsActive = 1";

            object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql);

            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return int.MinValue;
        }

        public static int GetActiveClubCount()
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnClub_Club WHERE IsActive = 1";

            object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql);

            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return int.MinValue;
        }

        public static UserClubStatus GetUserClubStatus(int userID, int clubID)
        {
            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "[dbo].[GetUserClubStatus]";

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                SqlParameter returnPara = new SqlParameter();
                returnPara.Direction = ParameterDirection.ReturnValue;
                returnPara.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(returnPara);

                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                return (UserClubStatus)returnPara.Value;
            }
        }

        public static int GetClubMemberCount(int clubID)
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnClub_RelationUserClub	WHERE ClubUid = @clubID AND IsActive = 1";

            object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return int.MinValue;
        }

        public static int GetClubMemberQuota(int clubID)
        {
            string sql = "SELECT r.MaxMember FROM dbo.AcnClub_Club c INNER JOIN dbo.AcnClub_ConfigRank r ON c.RankLevel = r.RankLevelID WHERE c.ClubUid = @clubID";

            object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return int.MinValue;
        }

        public static int GetClubExecutorQuota(int clubID)
        {
            string sql = "SELECT r.MaxExecutor FROM dbo.AcnClub_Club c INNER JOIN dbo.AcnClub_ConfigRank r ON c.RankLevel = r.RankLevelID WHERE c.ClubUid = @clubID";

            object obj = SqlHelper.ExecuteScalar(SQLConn.GetConnection(), CommandType.Text, sql, new SqlParameter("@clubID", clubID));

            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return int.MinValue;
        }

        //internal static string SavePic(HttpPostedFile file)
        //{
        //    FileInfo fi = new FileInfo(file.FileName);

        //    string fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);

        //    string fullPath = Path.Combine(ConfigGlobal.ClubLogoPath, fileName);

        //    file.SaveAs(fullPath);

        //    return fileName;
        //}

        public static void ApplyClub(string fullName, string shortName, string slogan, string desc, int creatorUid, string creatorUserName)
        {
            Club club = new Club();
            club.FullName = fullName;
            club.ShortName = shortName;
            club.Slogan = slogan;
            club.LogoName = ConfigGlobal.DefaultClubLogoName;
            club.RankLevel = ConfigGlobal.ClubDefaultRankLevel;
            club.RankScore = 0;
            club.Description = desc;
            club.CreatorUid = creatorUid;
            club.CreatorUserName = creatorUserName;
            club.ManagerUid = creatorUid;
            club.ManagerUserName = creatorUserName;
            club.CreateDate = DateTime.Now;
            club.UpdateDate = DateTime.Now;
            club.Fortune = 0;
            club.MemberCredit = 0;
            club.MemberFortune = 0;
            club.MemberLoyalty = 0;

            InsertClub(club);

            club = GetClubInfo(fullName);
            if (club != null)
            {
                ClubSysPrivateMessage.SendMessage(club.ID.Value, creatorUserName, ClubSysMessageType.ApplyClub);
            }
        }

        public static void UpdateApplyClub(int clubID, string fullName, string shortName, string slogan, string desc, int creatorUid, string creatorUserName)
        {
            Club club = GetClubInfo(clubID);
            club.FullName = fullName;
            club.ShortName = shortName;
            club.Slogan = slogan;
            club.RankLevel = ConfigGlobal.ClubDefaultRankLevel;
            club.Description = desc;
            club.CreatorUid = creatorUid;
            club.CreatorUserName = creatorUserName;
            club.ManagerUid = creatorUid;
            club.ManagerUserName = creatorUserName;
            club.CreateDate = DateTime.Now;
            club.UpdateDate = DateTime.Now;
            club.Fortune = 0;
            club.MemberCredit = 0;
            club.MemberFortune = 0;
            club.MemberLoyalty = 0;

            SaveClub(club);
        }

        public static void ApproveClub(int clubID, bool approve)
        {
            Club club = ClubLogic.GetClubInfo(clubID);

            if (club != null)
            {
                club.IsActive = approve;
                club.UpdateDate = DateTime.Now;

                if (approve)
                    club.IsAppliable = true;

                SaveClub(club);

                UserClub uc = new UserClub();
                uc.ClubUid = clubID;
                uc.JoinClubDate = DateTime.Now;
                uc.FromDate = DateTime.Now;
                uc.IsActive = true;
                uc.Responsibility = (int)Responsibility.Manager;
                uc.Userid = club.ManagerUid;
                uc.UserName = club.ManagerUserName;

                SaveUserClub(uc);

                ClubSysPrivateMessage.SendMessage(clubID, string.Empty, ClubSysMessageType.ApproveClub);
            }
            else
                throw new Exception("Club not exist.");
        }

        public static void UpdateClubInfo(int clubID, HttpPostedFile logo, string slogan, string description, bool isAppliable, int? fortune)
        {
            Club club = ClubLogic.GetClubInfo(clubID);

            if (club != null)
            {
                //if (logo.ContentLength != 0)
                //{
                //    club.LogoName = SavePic(logo);
                //}

                club.Slogan = slogan;
                club.Description = description;
                club.IsAppliable = isAppliable;

                if (fortune != null)
                    club.Fortune = fortune;

                club.UpdateDate = DateTime.Now;

                SaveClub(club);
            }
            else
                throw new Exception("Club not exist.");
        }

        public static string TranslateResponsibility(int res)
        {
            return TranslateResponsibility((Responsibility)res);
        }

        public static string TranslateResponsibility(Responsibility res)
        {
            switch (res)
            {
                case Responsibility.Manager:
                    return "会长";
                case Responsibility.Executor:
                    return "干事";
                case Responsibility.Member:
                    return "会员";
                default:
                    return "会员";
            }
        }

        public static string TranslateClubHistoryActionType(int actionType)
        {
            return TranslateClubHistoryActionType((ClubHistoryActionType)actionType);
        }

        public static string TranslateClubHistoryActionType(ClubHistoryActionType actionType)
        {
            switch (actionType)
            {
                case ClubHistoryActionType.JoinClub:
                    return "审核通过";
                case ClubHistoryActionType.RejectJoinClub:
                    return "审核不通过";
                case ClubHistoryActionType.LeaveClub:
                    return "退会";
                case ClubHistoryActionType.MandatoryLeaveClub:
                    return "退会";
                case ClubHistoryActionType.Nominated:
                    return "任命";
                case ClubHistoryActionType.Dismiss:
                    return "解职";
                case ClubHistoryActionType.LuckyPlayer:
                    return "幸运球员";
                default:
                    return string.Empty;
            }
        }

        public static string BuildClubHistoryActionDesc(int actionType, params string[] para)
        {
            return BuildClubHistoryActionDesc((ClubHistoryActionType)actionType, para);
        }

        public static string BuildClubHistoryActionDesc(ClubHistoryActionType actionType, params string[] para)
        {
            switch (actionType)
            {
                case ClubHistoryActionType.JoinClub:
                    return "加入球会审核通过";
                case ClubHistoryActionType.RejectJoinClub:
                    return "加入球会审核不通过";
                case ClubHistoryActionType.LeaveClub:
                    return "自行退会";
                case ClubHistoryActionType.MandatoryLeaveClub:
                    return "强行退会";
                case ClubHistoryActionType.Nominated:
                    return string.Format("任命为{0}", para[0]);
                case ClubHistoryActionType.Dismiss:
                    return string.Format("解除{0}职务", para[0]);
                case ClubHistoryActionType.LuckyPlayer:
                    return string.Format("为球会获得枪手币:{0}(幸运球员:{1})", para[0], para[1]);
                case ClubHistoryActionType.TransferExtcredit:
                    return string.Format("转账给{0} {2}:{1}", para[0], para[1], para[2]);
                default:
                    return string.Empty;
            }
        }
    }
}
