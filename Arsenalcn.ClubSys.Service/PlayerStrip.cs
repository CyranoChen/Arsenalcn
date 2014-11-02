using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Service
{
    public class PlayerStrip
    {
        public static bool CheckUserNumActiveCondition(int userid, int userNumID)
        {
            Card un = PlayerStrip.GetUserNumber(userNumID);

            if (un == null)
                return false;

            if (un.IsActive)
                return false;

            if (un.UserID != userid)
                return false;

            Player player = PlayerStrip.GetPlayerInfo(userid);

            if (player == null)
                return false;

            if (player.Shirt < 5 || player.Shorts < 5 || player.Sock < 5)
            {
                return false;
            }

            return true;
        }

        public static int GetClubMemberEquipmentCount(int clubID)
        {
            int totalCount = 0;

            List<UserClub> ucs = ClubLogic.GetClubMembers(clubID);

            foreach (UserClub uc in ucs)
            {
                Player player = PlayerStrip.GetPlayerInfo(uc.Userid.Value);
                List<Card> numbers = PlayerStrip.GetMyNumbers(uc.Userid.Value);

                if (player != null)
                    totalCount += (player.Shirt + player.Shorts + player.Sock);

                totalCount += numbers.Count;

                foreach (Card number in numbers)
                {
                    if (number.IsActive)
                        totalCount += 15;
                }
                totalCount += (Entity.UserVideo.GetUserVideosByUserID(uc.Userid.Value).Count * 16);
            }

            return totalCount;
        }

        public static int GetClubMemberCardCount(int clubID)
        {
            int totalCount = 0;

            List<UserClub> ucs = ClubLogic.GetClubMembers(clubID);

            foreach (UserClub uc in ucs)
            {
                Player player = PlayerStrip.GetPlayerInfo(uc.Userid.Value);
                List<Card> numbers = PlayerStrip.GetMyNumbers(uc.Userid.Value);

                if (player != null)
                    totalCount += numbers.Count;
            }

            return totalCount;
        }

        public static int GetClubMemberVideoCount(int clubID)
        {
            int totalCount = 0;

            List<UserClub> ucs = ClubLogic.GetClubMembers(clubID);

            foreach (UserClub uc in ucs)
            {
                Player player = PlayerStrip.GetPlayerInfo(uc.Userid.Value);
                //DataTable dt = UserVideo.GetUserVideo(uc.Userid.Value);

                if (player != null)
                    totalCount += Entity.UserVideo.GetUserVideosByUserID(uc.Userid.Value).Count;
            }

            return totalCount;
        }

        public static float CalcPlayerContributionBonusRate(int userID)
        {
            Player player = GetPlayerInfo(userID);

            if (player != null)
            {
                int lv = ConfigGlobal.PlayerMaxLv;

                if (lv > player.Shirt)
                    lv = player.Shirt;

                if (lv > player.Shorts)
                    lv = player.Shorts;

                if (lv > player.Sock)
                    lv = player.Sock;

                return 0.1f * lv;
            }
            else
            {
                return 0;
            }
        }

        public static string CalcFlashRate(int userID)
        {
            ShortUserInfo info = AdminUsers.GetShortUserInfo(userID);

            double result = Math.Log10((double)info.Extcredits2);

            decimal deci = decimal.Round(decimal.Parse(result.ToString()), 1);

            return deci.ToString();
        }

        public static int GetUserBingoPlayCountThisHour(int userID)
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnClub_LogBingo WHERE UserID = @userID AND ActionDate BETWEEN @fromDate AND @toDate";
            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Now.ToString("yyyy-MM-dd HH:59:59")));

                con.Open();

                try
                {
                    count = (int)com.ExecuteScalar();
                }
                catch
                { }

                con.Close();
            }

            return count;
        }

        public static List<Card> GetMyNumbers(int userId)
        {
            List<Card> list = new List<Card>();

            string sql = "SELECT * FROM dbo.AcnClub_Card WHERE UserID = @userID ORDER BY GainDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userId));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Card c = new Card(dr);

                    list.Add(c);
                }
            }

            return list;
        }

        public static List<Card> GetMyCards(int userId)
        {
            List<Card> list = new List<Card>();

            string sql = "SELECT * FROM dbo.AcnClub_Card WHERE UserID = @userID ORDER BY GainDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userId));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Card un = new Card(dr);

                    list.Add(un);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetAllBingoHistory()
        {
            List<BingoHistory> list = new List<BingoHistory>();

            string sql = "SELECT * FROM dbo.AcnClub_LogBingo WHERE Result IS NOT NULL ORDER BY ActionDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    BingoHistory bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetUserBingoHistory(int userID)
        {
            List<BingoHistory> list = new List<BingoHistory>();

            string sql = "SELECT * FROM dbo.AcnClub_LogBingo WHERE UserID = @userID ORDER BY ActionDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    BingoHistory bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetClubBingoHistory(int clubID)
        {
            List<BingoHistory> list = new List<BingoHistory>();

            string sql = "SELECT * FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result IS NOT NULL ORDER BY ActionDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    BingoHistory bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static int InsertBingoStart(int userID, string userName, int clubID)
        {
            string sql = "INSERT INTO AcnClub_LogBingo VALUES (@userID, @userName, @clubID, getdate(), null, null); SELECT scope_identity()";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userName", userName));
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                Decimal id = (Decimal)com.ExecuteScalar();

                con.Close();

                return (int)id;
            }
        }

        public static void UpdateBingoResultLog(int userID, BingoResult result)
        {
            string sql = "UPDATE AcnClub_LogBingo SET Result = @result, ResultDetail = @resultDetail WHERE [ID] = (SELECT TOP 1 [ID] FROM AcnClub_LogBingo WHERE UserID = @userID AND Result is NULL ORDER BY ActionDate DESC)";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                object tempValue = null;

                com.Parameters.Add(new SqlParameter("@userID", userID));

                if (result.Result == BingoResultType.Null)
                    tempValue = DBNull.Value;
                else
                    tempValue = (int)result.Result;

                com.Parameters.Add(new SqlParameter("@result", tempValue));

                com.Parameters.Add(new SqlParameter("@resultDetail", ClubLogic.ParseNullableParam(result.ResultDetail)));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }
        public static string UpdateBingoResult(int userID, string userName, BingoResult result, string resultType)
        {
            if (result.Result != BingoResultType.Null)
            {
                string sql = string.Empty;
                if ((resultType == "card") && (result.Result == BingoResultType.Card) && (result.ResultDetail.Length == 36))
                {
                    try
                    {
                        string guid = result.ResultDetail;
                        sql = "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, 0, 0, getdate(), null, @guid)";

                        using (SqlConnection con = SQLConn.GetConnection())
                        {
                            SqlCommand com = new SqlCommand(sql, con);

                            com.Parameters.Add(new SqlParameter("@userID", userID));
                            com.Parameters.Add(new SqlParameter("@userName", userName));
                            com.Parameters.Add(new SqlParameter("@guid", new Guid(guid)));

                            con.Open();

                            com.ExecuteNonQuery();

                            con.Close();
                        }

                        UpdateBingoResultLog(userID, result);

                        return ("a card of " + Arsenal_Player.Cache.Load(new Guid(guid)).DisplayName);
                    }
                    catch
                    {
                        //throw new Exception("Invalid Arsenal Player Guid.");
                        return "-1";
                    }
                }
                else if ((resultType == "card") && (result.Result == BingoResultType.Card) && (result.ResultDetail == "legend"))
                {
                    try
                    {
                        sql = "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, 0, 0, getdate(), null, null)";

                        using (SqlConnection con = SQLConn.GetConnection())
                        {
                            SqlCommand com = new SqlCommand(sql, con);

                            com.Parameters.Add(new SqlParameter("@userID", userID));
                            com.Parameters.Add(new SqlParameter("@userName", userName));

                            con.Open();

                            com.ExecuteNonQuery();

                            con.Close();
                        }

                        UpdateBingoResultLog(userID, result);

                        return "a legend card";
                    }
                    catch
                    {
                        return "-1";
                    }
                }
                else if (result.Result == BingoResultType.Cash)
                {
                    try
                    {
                        float bonusCash = Convert.ToSingle(result.ResultDetail);
                        string finalResult = string.Empty;

                        UserInfo info = AdminUsers.GetUserInfo(userID);
                        info.Extcredits2 += bonusCash;

                        if (AdminUsers.UpdateUserAllInfo(info))
                        {
                            finalResult = ("Cash(QSB):" + bonusCash.ToString());

                            UpdateBingoResultLog(userID, result);

                            return finalResult;
                        }
                        else
                        {
                            return "-1";
                        }
                    }
                    catch
                    {
                        return "-1";
                    }
                }
                else
                {
                    try
                    {
                        // Strip Bonus
                        int shirt = 0;
                        int shorts = 0;
                        int sock = 0;
                        float bonusCash = 0f;
                        float bonusRate = ConfigGlobal.BingoBonusRate;

                        string finalResult = string.Empty;

                        Player player = GetPlayerInfo(userID);

                        if ((result.Result == BingoResultType.Strip) || (result.Result == BingoResultType.Both))
                        {
                            if (result.ResultDetail == "shirt")
                                shirt++;
                            else if (result.ResultDetail == "shorts")
                                shorts++;
                            else if (result.ResultDetail == "sock")
                                sock++;
                            else if (result.ResultDetail == "strips")
                            {
                                shirt += 5;
                                shorts += 5;
                                sock += 5;
                            }

                            if (player == null)
                            {
                                sql = "INSERT INTO dbo.AcnClub_Player VALUES (@userID, @userName, @shirt, @shorts, @sock, null, 1, null)";
                            }
                            else
                            {
                                shirt = player.Shirt + shirt;
                                shorts = player.Shorts + shorts;
                                sock = player.Sock + sock;

                                sql = "UPDATE dbo.AcnClub_Player SET Shirt = @shirt, Shorts = @shorts, Sock = @sock WHERE UserID = @userID AND UserName = @userName";
                            }
                            using (SqlConnection con = SQLConn.GetConnection())
                            {
                                SqlCommand com = new SqlCommand(sql, con);

                                com.Parameters.Add(new SqlParameter("@userID", userID));
                                com.Parameters.Add(new SqlParameter("@userName", userName));
                                com.Parameters.Add(new SqlParameter("@shirt", shirt));
                                com.Parameters.Add(new SqlParameter("@shorts", shorts));
                                com.Parameters.Add(new SqlParameter("@sock", sock));

                                con.Open();

                                com.ExecuteNonQuery();

                                con.Close();
                            }

                            if (result.ResultDetail == "strips")
                                finalResult += ("5 " + result.ResultDetail.ToString());
                            else
                                finalResult += ("a " + result.ResultDetail.ToString());
                        }


                        //Cash Bonus
                        if (result.Result == BingoResultType.Both)
                        {
                            if (player != null)
                            {
                                if (result.ResultDetail == "shirt")
                                    bonusCash = Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Shirt)) * bonusRate));
                                else if (result.ResultDetail == "shorts")
                                    bonusCash = Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Shorts)) * bonusRate));
                                else if (result.ResultDetail == "sock")
                                    bonusCash = Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Sock)) * bonusRate));
                            }
                            else
                            {
                                bonusCash = 0f;
                            }

                            bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) + Convert.ToSingle(ConfigGlobal.BingoCost);

                            UserInfo info = AdminUsers.GetUserInfo(userID);
                            info.Extcredits2 += bonusCash;
                            if (AdminUsers.UpdateUserAllInfo(info))
                            {
                                finalResult += (" and Cash(QSB):" + bonusCash.ToString());
                            }

                            result.ResultDetail += string.Format("+({0})", bonusCash.ToString());
                        }

                        UpdateBingoResultLog(userID, result);
                        return finalResult;
                    }
                    catch
                    {
                        return "-1";
                    }
                }
            }
            else
            {
                return "-1";
            }
        }

        public static int GetClubRemainingEquipment(int clubID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND RESULT IS NOT NULL AND ActionDate BETWEEN @fromDate AND @toDate";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            if (ConfigGlobal.DailyClubEquipmentCount - count < 0)
                return 0;
            else
                return ConfigGlobal.DailyClubEquipmentCount - count;
        }

        public static int GetUserBingoGainCount(int userID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND Result IS NOT NULL";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetUserBingoGainCountToday(int userID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND Result IS NOT NULL AND ActionDate BETWEEN @fromDate AND @toDate";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetUserBingoPlayCount(int userID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetUserBingoPlayCountToday(int userID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND ActionDate BETWEEN @fromDate AND @toDate";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetClubBingoPlayCount(int clubID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetClubBingoPlayCountToday(int clubID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND ActionDate BETWEEN @fromDate AND @toDate";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static int GetClubEquipmentGainCount(int clubID, BingoResultType type, string resultDetail)
        {
            string sql = string.Empty;

            switch (type)
            {
                case BingoResultType.Strip:
                    sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND (ResultDetail = @resultDetail OR ResultDetail = 'strip')";
                    break;
                case BingoResultType.Card:
                    if (resultDetail == "legend")
                    {
                        sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND ResultDetail = @resultDetail";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND ResultDetail <> 'legend'";
                    }
                    break;
                case BingoResultType.Cash:
                case BingoResultType.Both:
                    sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result";
                    break;
            }

            if (sql != string.Empty)
            {
                using (SqlConnection con = SQLConn.GetConnection())
                {
                    SqlCommand com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@clubID", clubID));
                    com.Parameters.Add(new SqlParameter("@result", (int)type));
                    com.Parameters.Add(new SqlParameter("@resultDetail", resultDetail));

                    con.Open();

                    int count = (int)com.ExecuteScalar();

                    con.Close();

                    return count;
                }
            }
            else
                return 0;
        }

        public static List<Player> GetPlayers()
        {
            List<Player> list = new List<Player>();

            string sql = "SELECT * FROM AcnClub_Player ORDER BY ID DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                //com.Parameters.Add(new SqlParameter("@clubID", clubID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Player bh = new Player(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<Player> GetClubPlayers(int clubID)
        {
            List<Player> list = new List<Player>();

            string sql = "SELECT p.* FROM AcnClub_Player p INNER JOIN AcnClub_RelationUserClub uc ON p.UserID = uc.UserID WHERE uc.ClubUID = @clubID AND uc.IsActive = 1 ORDER BY Shirt+Shorts+Sock DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Player p = new Player(dr);

                    list.Add(p);
                }
            }

            return list;
        }

        public static int GetClubPlayerCount(int clubID)
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_Player p INNER JOIN AcnClub_RelationUserClub uc ON p.UserID = uc.UserID WHERE uc.ClubUID = @clubID AND uc.IsActive = 1";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                int count = (int)com.ExecuteScalar();

                con.Close();

                return count;
            }
        }

        public static Player GetPlayerInfo(int userId)
        {
            Player player = null;

            string sql = "SELECT * FROM dbo.AcnClub_Player WHERE UserID = @userId";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userId", userId));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                if (dt.Rows.Count != 0)
                {
                    player = new Player(dt.Rows[0]);
                }
            }

            return player;
        }

        public static Player GetPlayerInfoByPlayerID(int playerID)
        {
            Player player = null;

            string sql = "SELECT * FROM dbo.AcnClub_Player WHERE [ID] = @playerID";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@playerID", playerID));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                if (dt.Rows.Count != 0)
                {
                    player = new Player(dt.Rows[0]);
                }

            }

            return player;
        }

        public static void UpdatePlayerGoogleAdvActive(int userID, bool isActive)
        {
            Player player = GetPlayerInfo(userID);

            if (player != null)
            {
                string sql = "UPDATE dbo.AcnClub_Player SET IsActive = @isActive WHERE ID = @playerID;";

                using (SqlConnection con = SQLConn.GetConnection())
                {
                    SqlCommand com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@playerID", player.ID));
                    com.Parameters.Add(new SqlParameter("@isActive", isActive));

                    con.Open();

                    com.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        public static Card GetUserNumber(int id)
        {
            Card userNumber = null;

            string sql = "SELECT * FROM dbo.AcnClub_Card WHERE [ID] = @id";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@id", id));

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                if (dt.Rows.Count != 0)
                {
                    userNumber = new Card(dt.Rows[0]);
                }

            }

            return userNumber;
        }

        public static void UpdatePlayerCurrentNum(int userID, int userNumID)
        {
            Card un = GetUserNumber(userNumID);

            if (un.ArsenalPlayerGuid.HasValue)
            {
                string sql = "UPDATE dbo.AcnClub_Card SET IsInUse = 0 WHERE UserID = @userID; UPDATE dbo.AcnClub_Card SET IsInUse = 1 WHERE [ID] = @userNumID; UPDATE dbo.AcnClub_Player SET CurrentGuid = @currentGuid WHERE UserID = @userID";

                using (SqlConnection con = SQLConn.GetConnection())
                {
                    SqlCommand com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@userID", userID));
                    com.Parameters.Add(new SqlParameter("@userNumID", userNumID));
                    com.Parameters.Add(new SqlParameter("@currentGuid", un.ArsenalPlayerGuid));

                    con.Open();

                    com.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        public static void RemovePlayerCurrentNum(int userID, int userNumID)
        {
            Card un = GetUserNumber(userNumID);

            if (un.ArsenalPlayerGuid.HasValue)
            {
                string sql = "UPDATE dbo.AcnClub_Card SET IsInUse = 0 WHERE UserID = @userID; UPDATE dbo.AcnClub_Player SET CurrentGuid = NULL WHERE UserID = @userID AND CurrentGuid = @currentGuid";

                using (SqlConnection con = SQLConn.GetConnection())
                {
                    SqlCommand com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@userID", userID));
                    com.Parameters.Add(new SqlParameter("@userNumID", userNumID));
                    com.Parameters.Add(new SqlParameter("@currentGuid", un.ArsenalPlayerGuid));

                    con.Open();

                    com.ExecuteNonQuery();

                    con.Close();
                }
            }
        }

        public static void ActiveVideoCost(int userID, int userNumID)
        {
            string sql = @"DELETE FROM AcnClub_Card WHERE [ID] = @userNumID; 
                                 UPDATE dbo.AcnClub_Player SET Shirt = Shirt - 5, Shorts = Shorts - 5, Sock = Sock - 5 WHERE UserID = @userID";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userNumID", userNumID));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }

        public static void SetCardAcitve(int userID, int userNumID)
        {
            string sql = "UPDATE dbo.AcnClub_Card SET IsActive = 1, ActiveDate = GETDATE() WHERE [ID] = @userNumID; UPDATE dbo.AcnClub_Player SET Shirt = Shirt - 5, Shorts = Shorts - 5, Sock = Sock - 5 WHERE UserID = @userID";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userNumID", userNumID));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }

        public static int GetAllPlayerCount()
        {
            string sql = "SELECT COUNT(*) FROM AcnClub_Player";

            int count = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                con.Open();

                count = (int)com.ExecuteScalar();

                con.Close();
            }

            return count;
        }

        public static bool ValidateAuthKey(string id, string userId, string authKey)
        {
            string authPrivateKey = ConfigGlobal.AuthPrivateKey;

            string originStr = id + userId + authPrivateKey;

            byte[] bytes = Encoding.UTF8.GetBytes(originStr);

            byte[] resultBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);

            string sTemp = "";
            for (int i = 0; i < resultBytes.Length; i++)
            {
                sTemp += resultBytes[i].ToString("x").PadLeft(2, '0');
            }

            return sTemp.Substring(8, 16) == authKey;
        }

        public static bool ValidateBingoResult(int bingoHistoryID, int userID)
        {
            string sql = "SELECT TOP 1 [ID] FROM AcnClub_LogBingo WHERE UserID = @userID AND Result is NULL ORDER BY ActionDate DESC";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                try
                {
                    int id = (int)com.ExecuteScalar();

                    con.Close();

                    return id == bingoHistoryID;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static DataTable GetTopRpPlayers()
        {
            string sql = @"select top 5 A.username, A.userid, countGot * 100 / countTotal as rp, countGot, countTotal from
            (select username, userid, count(*) as countTotal from AcnClub_LogBingo group by username, userid) A
            inner join
            (select username, userid, count(*) as countGot from AcnClub_LogBingo where result is not null group by username, userid) B
            ON A.username = B.username AND A.userid = B.userid
            order by CountGot desc";

            DataTable players = new DataTable();

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                SqlDataAdapter sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                con.Close();
            }

            return players;
        }

        public static DataTable GetTopVideoPlayers()
        {
            string sql = @"select top 5 ISNULL(A.username, B.username) as username, ISNULL(A.userid, B.userid) as userid, ISNULL(inactiveVideo, 0) + ISNULL(videoCount, 0) as videoCount from
            (select username, userid, count(*) as inactiveVideo from AcnClub_Card where ArsenalPlayerGuid is null group by username, userid) A
            full outer join
            (select username, userid, count(*) as videoCount from acnclub_relationuservideo group by username, userid) B
            ON A.username = B.username AND A.userid = B.userid order by videoCount desc";

            DataTable players = new DataTable();

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                SqlDataAdapter sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                con.Close();
            }

            return players;
        }

        public static DataTable GetTopCardPlayers()
        {
            string sql = @"select top 5 username,userid, count(*) as cardCount
            from dbo.AcnClub_Card where ArsenalPlayerGuid is not null group by username, userid order by cardCount desc";

            DataTable players = new DataTable();

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                SqlDataAdapter sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                con.Close();
            }

            return players;
        }

        public static void AddCard(int userid, string username, Guid? cardGuid, bool isActive)
        {
            string sql = "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, @isActive, 0, getdate(), @activeDate, @guid)";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userid));
                com.Parameters.Add(new SqlParameter("@userName", username));
                com.Parameters.Add(new SqlParameter("@isActive", isActive));
                if (isActive)
                    com.Parameters.Add(new SqlParameter("@activeDate", DateTime.Now));
                else
                    com.Parameters.Add(new SqlParameter("@activeDate", DBNull.Value));

                if (cardGuid.HasValue)
                    com.Parameters.Add(new SqlParameter("@guid", cardGuid));
                else
                    com.Parameters.Add(new SqlParameter("@guid", DBNull.Value));

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
        }
    }

    public class BingoUtil
    {
        public static BingoResult AnalyzeFlashResult(int userID, string result, string resultType)
        {
            BingoResult br = new BingoResult();

            switch (resultType)
            {
                case "strip":
                    br.Result = BingoResultType.Strip;
                    if (result == "strip")
                        br.ResultDetail = "strips";
                    else
                        br.ResultDetail = result.ToString();

                    return br;
                case "card":
                    br.Result = BingoResultType.Card;
                    if (result.Length == 36)
                    {
                        br.ResultDetail = result.ToString();
                    }
                    else
                    {
                        br.ResultDetail = "legend";
                    }
                    return br;
                case "cash":
                    br.Result = BingoResultType.Cash;
                    float bonusCash = 0f;
                    float bonusRate = ConfigGlobal.BingoBonusRate;

                    if (result == "cash")
                    {
                        int videoActiveCount = Entity.UserVideo.GetUserVideosByUserID(userID).Count;

                        List<Card> items = PlayerStrip.GetMyNumbers(userID);
                        items.RemoveAll(delegate(Card un) { return !un.IsActive; });
                        int cardActiveCount = items.Count;

                        // Video & Card Cash Bonus
                        bonusCash += (videoActiveCount * 5 + cardActiveCount) * bonusRate;
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) + Convert.ToSingle(ConfigGlobal.BingoCost);
                    }
                    else
                    {
                        // Player Cash Bonus
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) * bonusRate;
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) + Convert.ToSingle(ConfigGlobal.BingoCost);
                    }

                    br.ResultDetail = Math.Floor(bonusCash).ToString();
                    return br;
                case "both":
                    br.Result = BingoResultType.Both;
                    br.ResultDetail = result.ToString();
                    return br;
                default:
                    br.Result = BingoResultType.Null;
                    br.ResultDetail = null;
                    return br;
            }
        }

        public static string ShowBothBingoDetail(string formatString, BingoResult br)
        {
            if ((br.Result == BingoResultType.Strip) && (br.ResultDetail != "strips"))
                return string.Format(formatString, AnalyzeStripDetail(br.ResultDetail.ToString()));
            else if (br.Result == BingoResultType.Both)
            {
                string stripResult = br.ResultDetail.Substring(0, br.ResultDetail.IndexOf("+"));
                string cashResult = br.ResultDetail.Substring(br.ResultDetail.IndexOf("(") + 1, (br.ResultDetail.IndexOf(")") - br.ResultDetail.IndexOf("(") - 1));
                return string.Format(formatString, AnalyzeStripDetail(stripResult), cashResult);
            }
            else
                return string.Empty;
        }

        public static string AnalyzeStripDetail(string resultDetail)
        {
            if (resultDetail == "shirt")
                return "球衣";
            else if (resultDetail == "shorts")
                return "球裤";
            else if (resultDetail == "sock")
                return "球袜";
            else
                return string.Empty;
        }
    }

    public class BingoResult
    {
        public BingoResult()
        {
        }

        public BingoResult(int? dbResult, string dbResultDetail)
        {
            if (dbResult == null)
            {
                Result = BingoResultType.Null;
                ResultDetail = null;
            }
            else
            {
                Result = (BingoResultType)dbResult.Value;
                ResultDetail = dbResultDetail;
            }
        }

        public BingoResultType Result;
        public string ResultDetail;
    }

    public enum BingoResultType
    {
        //Shirt = 1,
        //Shorts = 2,
        //Sock = 4,
        Strip = 7,
        Card = 8,
        //Legend = 16,
        Cash = 32,
        Both = 39,
        Null = 0
    }
}
