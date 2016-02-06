using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Service
{
    public class PlayerStrip
    {
        public static bool CheckUserNumActiveCondition(int userid, int userNumID)
        {
            var un = GetUserNumber(userNumID);

            if (un == null)
                return false;

            if (un.IsActive)
                return false;

            if (un.UserID != userid)
                return false;

            var player = GetPlayerInfo(userid);

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
            var totalCount = 0;

            var ucs = ClubLogic.GetClubMembers(clubID);

            foreach (var uc in ucs)
            {
                var player = GetPlayerInfo(uc.Userid.Value);
                var numbers = GetMyNumbers(uc.Userid.Value);

                if (player != null)
                    totalCount += (player.Shirt + player.Shorts + player.Sock);

                totalCount += numbers.Count;

                foreach (var number in numbers)
                {
                    if (number.IsActive)
                        totalCount += 15;
                }
                totalCount += (Entity.UserVideo.GetUserVideosByUserID(uc.Userid.Value).Count*16);
            }

            return totalCount;
        }

        public static int GetClubMemberCardCount(int clubID)
        {
            var totalCount = 0;

            var ucs = ClubLogic.GetClubMembers(clubID);

            foreach (var uc in ucs)
            {
                var player = GetPlayerInfo(uc.Userid.Value);
                var numbers = GetMyNumbers(uc.Userid.Value);

                if (player != null)
                    totalCount += numbers.Count;
            }

            return totalCount;
        }

        public static int GetClubMemberVideoCount(int clubID)
        {
            var totalCount = 0;

            var ucs = ClubLogic.GetClubMembers(clubID);

            foreach (var uc in ucs)
            {
                var player = GetPlayerInfo(uc.Userid.Value);
                //DataTable dt = UserVideo.GetUserVideo(uc.Userid.Value);

                if (player != null)
                    totalCount += Entity.UserVideo.GetUserVideosByUserID(uc.Userid.Value).Count;
            }

            return totalCount;
        }

        public static float CalcPlayerContributionBonusRate(int userID)
        {
            var player = GetPlayerInfo(userID);

            if (player != null)
            {
                var lv = ConfigGlobal.PlayerMaxLv;

                if (lv > player.Shirt)
                    lv = player.Shirt;

                if (lv > player.Shorts)
                    lv = player.Shorts;

                if (lv > player.Sock)
                    lv = player.Sock;

                return 0.1f*lv;
            }
            return 0;
        }

        public static string CalcFlashRate(int userID)
        {
            var info = Users.GetShortUserInfo(userID);

            var result = Math.Log10(info.Extcredits2);

            var deci = decimal.Round(decimal.Parse(result.ToString()), 1);

            return deci.ToString();
        }

        public static int GetUserBingoPlayCountThisHour(int userID)
        {
            var sql =
                "SELECT COUNT(*) FROM dbo.AcnClub_LogBingo WHERE UserID = @userID AND ActionDate BETWEEN @fromDate AND @toDate";
            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Now.ToString("yyyy-MM-dd HH:59:59")));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static List<Card> GetMyNumbers(int userId)
        {
            var list = new List<Card>();

            var sql = "SELECT * FROM dbo.AcnClub_Card WHERE UserID = @userID ORDER BY GainDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userId));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var c = new Card(dr);

                    list.Add(c);
                }
            }

            return list;
        }

        public static List<Card> GetMyCards(int userId)
        {
            var list = new List<Card>();

            var sql = "SELECT * FROM dbo.AcnClub_Card WHERE UserID = @userID ORDER BY GainDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userId));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var un = new Card(dr);

                    list.Add(un);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetAllBingoHistory()
        {
            var list = new List<BingoHistory>();

            var sql = "SELECT * FROM dbo.AcnClub_LogBingo WHERE Result IS NOT NULL ORDER BY ActionDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetUserBingoHistory(int userID)
        {
            var list = new List<BingoHistory>();

            var sql = "SELECT * FROM dbo.AcnClub_LogBingo WHERE UserID = @userID ORDER BY ActionDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<BingoHistory> GetClubBingoHistory(int clubID)
        {
            var list = new List<BingoHistory>();

            var sql =
                "SELECT * FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result IS NOT NULL ORDER BY ActionDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var bh = new BingoHistory(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static int InsertBingoStart(int userID, string userName, int clubID)
        {
            var sql =
                "INSERT INTO AcnClub_LogBingo VALUES (@userID, @userName, @clubID, getdate(), null, null); SELECT scope_identity()";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userName", userName));
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                var id = (decimal) com.ExecuteScalar();

                //con.Close();

                return (int) id;
            }
        }

        public static void UpdateBingoResultLog(int userID, BingoResult result)
        {
            var sql =
                "UPDATE AcnClub_LogBingo SET Result = @result, ResultDetail = @resultDetail WHERE [ID] = (SELECT TOP 1 [ID] FROM AcnClub_LogBingo WHERE UserID = @userID AND Result is NULL ORDER BY ActionDate DESC)";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                object tempValue = null;

                com.Parameters.Add(new SqlParameter("@userID", userID));

                if (result.Result == BingoResultType.Null)
                    tempValue = DBNull.Value;
                else
                    tempValue = (int) result.Result;

                com.Parameters.Add(new SqlParameter("@result", tempValue));

                com.Parameters.Add(new SqlParameter("@resultDetail", ClubLogic.ParseNullableParam(result.ResultDetail)));

                con.Open();

                com.ExecuteNonQuery();

                //con.Close();
            }
        }

        public static string UpdateBingoResult(int userID, string userName, BingoResult result, string resultType)
        {
            if (result.Result != BingoResultType.Null)
            {
                var sql = string.Empty;
                if ((resultType == "card") && (result.Result == BingoResultType.Card) &&
                    (result.ResultDetail.Length == 36))
                {
                    try
                    {
                        var guid = result.ResultDetail;
                        sql =
                            "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, 0, 0, getdate(), null, @guid)";

                        using (var con = SQLConn.GetConnection())
                        {
                            var com = new SqlCommand(sql, con);

                            com.Parameters.Add(new SqlParameter("@userID", userID));
                            com.Parameters.Add(new SqlParameter("@userName", userName));
                            com.Parameters.Add(new SqlParameter("@guid", new Guid(guid)));

                            con.Open();

                            com.ExecuteNonQuery();

                            //con.Close();
                        }

                        UpdateBingoResultLog(userID, result);

                        return ("a card of " + Player.Cache.Load(new Guid(guid)).DisplayName);
                    }
                    catch
                    {
                        //throw new Exception("Invalid Arsenal Player Guid.");
                        return "-1";
                    }
                }
                if ((resultType == "card") && (result.Result == BingoResultType.Card) &&
                    (result.ResultDetail == "legend"))
                {
                    try
                    {
                        sql =
                            "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, 0, 0, getdate(), null, null)";

                        using (var con = SQLConn.GetConnection())
                        {
                            var com = new SqlCommand(sql, con);

                            com.Parameters.Add(new SqlParameter("@userID", userID));
                            com.Parameters.Add(new SqlParameter("@userName", userName));

                            con.Open();

                            com.ExecuteNonQuery();

                            //con.Close();
                        }

                        UpdateBingoResultLog(userID, result);

                        return "a legend card";
                    }
                    catch
                    {
                        return "-1";
                    }
                }
                if (result.Result == BingoResultType.Cash)
                {
                    try
                    {
                        var bonusCash = Convert.ToSingle(result.ResultDetail);
                        var finalResult = string.Empty;

                        var info = Users.GetUserInfo(userID);
                        info.Extcredits2 += bonusCash;

                        if (AdminUsers.UpdateUserAllInfo(info))
                        {
                            finalResult = ("Cash(QSB):" + bonusCash);

                            UpdateBingoResultLog(userID, result);

                            return finalResult;
                        }
                        return "-1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }
                try
                {
                    // Strip Bonus
                    var shirt = 0;
                    var shorts = 0;
                    var sock = 0;
                    var bonusCash = 0f;
                    var bonusRate = ConfigGlobal.BingoBonusRate;

                    var finalResult = string.Empty;

                    var player = GetPlayerInfo(userID);

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
                            sql =
                                "INSERT INTO dbo.AcnClub_Player VALUES (@userID, @userName, @shirt, @shorts, @sock, null, 1, null)";
                        }
                        else
                        {
                            shirt = player.Shirt + shirt;
                            shorts = player.Shorts + shorts;
                            sock = player.Sock + sock;

                            sql =
                                "UPDATE dbo.AcnClub_Player SET Shirt = @shirt, Shorts = @shorts, Sock = @sock WHERE UserID = @userID AND UserName = @userName";
                        }
                        using (var con = SQLConn.GetConnection())
                        {
                            var com = new SqlCommand(sql, con);

                            com.Parameters.Add(new SqlParameter("@userID", userID));
                            com.Parameters.Add(new SqlParameter("@userName", userName));
                            com.Parameters.Add(new SqlParameter("@shirt", shirt));
                            com.Parameters.Add(new SqlParameter("@shorts", shorts));
                            com.Parameters.Add(new SqlParameter("@sock", sock));

                            con.Open();

                            com.ExecuteNonQuery();

                            //con.Close();
                        }

                        if (result.ResultDetail == "strips")
                            finalResult += ("5 " + result.ResultDetail);
                        else
                            finalResult += ("a " + result.ResultDetail);
                    }


                    //Cash Bonus
                    if (result.Result == BingoResultType.Both)
                    {
                        if (player != null)
                        {
                            if (result.ResultDetail == "shirt")
                                bonusCash =
                                    Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Shirt))*bonusRate));
                            else if (result.ResultDetail == "shorts")
                                bonusCash =
                                    Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Shorts))*bonusRate));
                            else if (result.ResultDetail == "sock")
                                bonusCash =
                                    Convert.ToSingle(Math.Floor(Math.Sqrt(Convert.ToDouble(player.Sock))*bonusRate));
                        }
                        else
                        {
                            bonusCash = 0f;
                        }

                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) +
                                     Convert.ToSingle(ConfigGlobal.BingoCost);

                        var info = Users.GetUserInfo(userID);
                        info.Extcredits2 += bonusCash;
                        if (AdminUsers.UpdateUserAllInfo(info))
                        {
                            finalResult += (" and Cash(QSB):" + bonusCash);
                        }

                        result.ResultDetail += string.Format("+({0})", bonusCash);
                    }

                    UpdateBingoResultLog(userID, result);
                    return finalResult;
                }
                catch
                {
                    return "-1";
                }
            }
            return "-1";
        }

        public static int GetClubRemainingEquipment(int clubID)
        {
            var sql =
                "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND RESULT IS NOT NULL AND ActionDate BETWEEN @fromDate AND @toDate";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            if (ConfigGlobal.DailyClubEquipmentCount - count < 0)
                return 0;
            return ConfigGlobal.DailyClubEquipmentCount - count;
        }

        public static int GetUserBingoGainCount(int userID)
        {
            var sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND Result IS NOT NULL";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetUserBingoGainCountToday(int userID)
        {
            var sql =
                "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND Result IS NOT NULL AND ActionDate BETWEEN @fromDate AND @toDate";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetUserBingoPlayCount(int userID)
        {
            var sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetUserBingoPlayCountToday(int userID)
        {
            var sql =
                "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE UserID = @userID AND ActionDate BETWEEN @fromDate AND @toDate";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetClubBingoPlayCount(int clubID)
        {
            var sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetClubBingoPlayCountToday(int clubID)
        {
            var sql =
                "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND ActionDate BETWEEN @fromDate AND @toDate";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));
                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today.AddDays(1)));

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static int GetClubEquipmentGainCount(int clubID, BingoResultType type, string resultDetail)
        {
            var sql = string.Empty;

            switch (type)
            {
                case BingoResultType.Strip:
                    sql =
                        "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND (ResultDetail = @resultDetail OR ResultDetail = 'strip')";
                    break;
                case BingoResultType.Card:
                    if (resultDetail == "legend")
                    {
                        sql =
                            "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND ResultDetail = @resultDetail";
                    }
                    else
                    {
                        sql =
                            "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result AND ResultDetail <> 'legend'";
                    }
                    break;
                case BingoResultType.Cash:
                case BingoResultType.Both:
                    sql = "SELECT COUNT(*) FROM AcnClub_LogBingo WHERE ClubID = @clubID AND Result = @result";
                    break;
            }

            if (sql != string.Empty)
            {
                using (var con = SQLConn.GetConnection())
                {
                    var com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@clubID", clubID));
                    com.Parameters.Add(new SqlParameter("@result", (int) type));
                    com.Parameters.Add(new SqlParameter("@resultDetail", resultDetail));

                    con.Open();

                    var count = (int) com.ExecuteScalar();

                    //con.Close();

                    return count;
                }
            }
            return 0;
        }

        public static List<Gamer> GetPlayers()
        {
            var list = new List<Gamer>();

            var sql = "SELECT * FROM AcnClub_Player ORDER BY ID DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                //com.Parameters.Add(new SqlParameter("@clubID", clubID));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var bh = new Gamer(dr);

                    list.Add(bh);
                }
            }

            return list;
        }

        public static List<Gamer> GetClubPlayers(int clubID)
        {
            var list = new List<Gamer>();

            var sql =
                "SELECT p.* FROM AcnClub_Player p INNER JOIN AcnClub_RelationUserClub uc ON p.UserID = uc.UserID WHERE uc.ClubUID = @clubID AND uc.IsActive = 1 ORDER BY Shirt+Shorts+Sock DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var p = new Gamer(dr);

                    list.Add(p);
                }
            }

            return list;
        }

        public static int GetClubPlayerCount(int clubID)
        {
            var sql =
                "SELECT COUNT(*) FROM AcnClub_Player p INNER JOIN AcnClub_RelationUserClub uc ON p.UserID = uc.UserID WHERE uc.ClubUID = @clubID AND uc.IsActive = 1";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@clubID", clubID));

                con.Open();

                var count = (int) com.ExecuteScalar();

                //con.Close();

                return count;
            }
        }

        public static Gamer GetPlayerInfo(int userId)
        {
            Gamer player = null;

            var sql = "SELECT * FROM dbo.AcnClub_Player WHERE UserID = @userId";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userId", userId));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                if (dt.Rows.Count != 0)
                {
                    player = new Gamer(dt.Rows[0]);
                }
            }

            return player;
        }

        public static Gamer GetPlayerInfoByPlayerID(int playerID)
        {
            Gamer player = null;

            var sql = "SELECT * FROM dbo.AcnClub_Player WHERE [ID] = @playerID";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@playerID", playerID));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                if (dt.Rows.Count != 0)
                {
                    player = new Gamer(dt.Rows[0]);
                }
            }

            return player;
        }

        public static void UpdatePlayerInfo(int id, int shirt, int shorts, int sock, bool isActive)
        {
            var sql = @"UPDATE dbo.AcnClub_Player SET Shirt = @shirt, Shorts = @shorts, Sock = @sock,
                                  IsActive = @isActive WHERE ID = @id";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@id", id));
                com.Parameters.Add(new SqlParameter("@shirt", shirt));
                com.Parameters.Add(new SqlParameter("@shorts", shorts));
                com.Parameters.Add(new SqlParameter("@sock", sock));
                com.Parameters.Add(new SqlParameter("@isActive", isActive));

                con.Open();

                com.ExecuteNonQuery();

                //con.Close();
            }
        }

        public static void UpdatePlayerGoogleAdvActive(int userID, bool isActive)
        {
            var player = GetPlayerInfo(userID);

            if (player != null)
            {
                var sql = "UPDATE dbo.AcnClub_Player SET IsActive = @isActive WHERE ID = @playerID;";

                using (var con = SQLConn.GetConnection())
                {
                    var com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@playerID", player.ID));
                    com.Parameters.Add(new SqlParameter("@isActive", isActive));

                    con.Open();

                    com.ExecuteNonQuery();

                    //con.Close();
                }
            }
        }

        public static Card GetUserNumber(int id)
        {
            Card userNumber = null;

            var sql = "SELECT * FROM dbo.AcnClub_Card WHERE [ID] = @id";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@id", id));

                var sda = new SqlDataAdapter(com);
                con.Open();

                var dt = new DataTable();

                sda.Fill(dt);

                //con.Close();

                if (dt.Rows.Count != 0)
                {
                    userNumber = new Card(dt.Rows[0]);
                }
            }

            return userNumber;
        }

        public static void UpdatePlayerCurrentNum(int userID, int userNumID)
        {
            var un = GetUserNumber(userNumID);

            if (un.ArsenalPlayerGuid.HasValue)
            {
                var sql =
                    "UPDATE dbo.AcnClub_Card SET IsInUse = 0 WHERE UserID = @userID; UPDATE dbo.AcnClub_Card SET IsInUse = 1 WHERE [ID] = @userNumID; UPDATE dbo.AcnClub_Player SET CurrentGuid = @currentGuid WHERE UserID = @userID";

                using (var con = SQLConn.GetConnection())
                {
                    var com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@userID", userID));
                    com.Parameters.Add(new SqlParameter("@userNumID", userNumID));
                    com.Parameters.Add(new SqlParameter("@currentGuid", un.ArsenalPlayerGuid));

                    con.Open();

                    com.ExecuteNonQuery();

                    //con.Close();
                }
            }
        }

        public static void RemovePlayerCurrentNum(int userID, int userNumID)
        {
            var un = GetUserNumber(userNumID);

            if (un.ArsenalPlayerGuid.HasValue)
            {
                var sql =
                    "UPDATE dbo.AcnClub_Card SET IsInUse = 0 WHERE UserID = @userID; UPDATE dbo.AcnClub_Player SET CurrentGuid = NULL WHERE UserID = @userID AND CurrentGuid = @currentGuid";

                using (var con = SQLConn.GetConnection())
                {
                    var com = new SqlCommand(sql, con);

                    com.Parameters.Add(new SqlParameter("@userID", userID));
                    com.Parameters.Add(new SqlParameter("@userNumID", userNumID));
                    com.Parameters.Add(new SqlParameter("@currentGuid", un.ArsenalPlayerGuid));

                    con.Open();

                    com.ExecuteNonQuery();

                    //con.Close();
                }
            }
        }

        public static void ActiveVideoCost(int userID, int userNumID)
        {
            var sql = @"DELETE FROM AcnClub_Card WHERE [ID] = @userNumID; 
                                 UPDATE dbo.AcnClub_Player SET Shirt = Shirt - 5, Shorts = Shorts - 5, Sock = Sock - 5 WHERE UserID = @userID";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userNumID", userNumID));

                con.Open();

                com.ExecuteNonQuery();

                //con.Close();
            }
        }

        public static void SetCardAcitve(int userID, int userNumID)
        {
            var sql =
                "UPDATE dbo.AcnClub_Card SET IsActive = 1, ActiveDate = GETDATE() WHERE [ID] = @userNumID; UPDATE dbo.AcnClub_Player SET Shirt = Shirt - 5, Shorts = Shorts - 5, Sock = Sock - 5 WHERE UserID = @userID";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));
                com.Parameters.Add(new SqlParameter("@userNumID", userNumID));

                con.Open();

                com.ExecuteNonQuery();

                //con.Close();
            }
        }

        public static int GetAllPlayerCount()
        {
            var sql = "SELECT COUNT(*) FROM AcnClub_Player";

            var count = 0;

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                con.Open();

                count = (int) com.ExecuteScalar();

                //con.Close();
            }

            return count;
        }

        public static bool ValidateAuthKey(string id, string userId, string authKey)
        {
            var authPrivateKey = ConfigGlobal.AuthPrivateKey;

            var originStr = id + userId + authPrivateKey;

            var bytes = Encoding.UTF8.GetBytes(originStr);

            var resultBytes = MD5.Create().ComputeHash(bytes);

            var sTemp = "";
            for (var i = 0; i < resultBytes.Length; i++)
            {
                sTemp += resultBytes[i].ToString("x").PadLeft(2, '0');
            }

            return sTemp.Substring(8, 16) == authKey;
        }

        public static bool ValidateBingoResult(int bingoHistoryID, int userID)
        {
            var sql =
                "SELECT TOP 1 [ID] FROM AcnClub_LogBingo WHERE UserID = @userID AND Result is NULL ORDER BY ActionDate DESC";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                com.Parameters.Add(new SqlParameter("@userID", userID));

                con.Open();

                try
                {
                    var id = (int) com.ExecuteScalar();

                    //con.Close();

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
            var sql = @"select top 5 A.username, A.userid, countGot * 100 / countTotal as rp, countGot, countTotal from
            (select username, userid, count(*) as countTotal from AcnClub_LogBingo group by username, userid) A
            inner join
            (select username, userid, count(*) as countGot from AcnClub_LogBingo where result is not null group by username, userid) B
            ON A.username = B.username AND A.userid = B.userid
            order by CountGot desc";

            var players = new DataTable();

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                var sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                //con.Close();
            }

            return players;
        }

        public static DataTable GetTopVideoPlayers()
        {
            var sql =
                @"select top 5 ISNULL(A.username, B.username) as username, ISNULL(A.userid, B.userid) as userid, ISNULL(inactiveVideo, 0) + ISNULL(videoCount, 0) as videoCount from
            (select username, userid, count(*) as inactiveVideo from AcnClub_Card where ArsenalPlayerGuid is null group by username, userid) A
            full outer join
            (select username, userid, count(*) as videoCount from acnclub_relationuservideo group by username, userid) B
            ON A.username = B.username AND A.userid = B.userid order by videoCount desc";

            var players = new DataTable();

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                var sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                //con.Close();
            }

            return players;
        }

        public static DataTable GetTopCardPlayers()
        {
            var sql = @"select top 5 username,userid, count(*) as cardCount
            from dbo.AcnClub_Card where ArsenalPlayerGuid is not null group by username, userid order by cardCount desc";

            var players = new DataTable();

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

                var sda = new SqlDataAdapter(com);

                con.Open();

                sda.Fill(players);

                //con.Close();
            }

            return players;
        }

        public static void AddCard(int userid, string username, Guid? cardGuid, bool isActive)
        {
            var sql =
                "INSERT INTO dbo.AcnClub_Card (UserID, UserName, IsActive, IsInUse, GainDate, ActiveDate, ArsenalPlayerGuid) VALUES (@userID, @userName, @isActive, 0, getdate(), @activeDate, @guid)";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);

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

                //con.Close();
            }
        }
    }

    public class BingoUtil
    {
        public static BingoResult AnalyzeFlashResult(int userID, string result, string resultType)
        {
            var br = new BingoResult();

            switch (resultType)
            {
                case "strip":
                    br.Result = BingoResultType.Strip;
                    if (result == "strip")
                        br.ResultDetail = "strips";
                    else
                        br.ResultDetail = result;

                    return br;
                case "card":
                    br.Result = BingoResultType.Card;
                    if (result.Length == 36)
                    {
                        br.ResultDetail = result;
                    }
                    else
                    {
                        br.ResultDetail = "legend";
                    }
                    return br;
                case "cash":
                    br.Result = BingoResultType.Cash;
                    var bonusCash = 0f;
                    var bonusRate = ConfigGlobal.BingoBonusRate;

                    if (result == "cash")
                    {
                        var videoActiveCount = Entity.UserVideo.GetUserVideosByUserID(userID).Count;

                        var items = PlayerStrip.GetMyNumbers(userID);
                        items.RemoveAll(delegate(Card un) { return !un.IsActive; });
                        var cardActiveCount = items.Count;

                        // Video & Card Cash Bonus
                        bonusCash += (videoActiveCount*5 + cardActiveCount)*bonusRate;
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) +
                                     Convert.ToSingle(ConfigGlobal.BingoCost);
                    }
                    else
                    {
                        // Player Cash Bonus
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost)*bonusRate;
                        bonusCash += Convert.ToSingle(ConfigGlobal.BingoGetCost) +
                                     Convert.ToSingle(ConfigGlobal.BingoCost);
                    }

                    br.ResultDetail = Math.Floor(bonusCash).ToString();
                    return br;
                case "both":
                    br.Result = BingoResultType.Both;
                    br.ResultDetail = result;
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
                return string.Format(formatString, AnalyzeStripDetail(br.ResultDetail));
            if (br.Result == BingoResultType.Both)
            {
                var stripResult = br.ResultDetail.Substring(0, br.ResultDetail.IndexOf("+"));
                var cashResult = br.ResultDetail.Substring(br.ResultDetail.IndexOf("(") + 1,
                    (br.ResultDetail.IndexOf(")") - br.ResultDetail.IndexOf("(") - 1));
                return string.Format(formatString, AnalyzeStripDetail(stripResult), cashResult);
            }
            return string.Empty;
        }

        public static string AnalyzeStripDetail(string resultDetail)
        {
            if (resultDetail == "shirt")
                return "球衣";
            if (resultDetail == "shorts")
                return "球裤";
            if (resultDetail == "sock")
                return "球袜";
            return string.Empty;
        }
    }

    public class BingoResult
    {
        public BingoResultType Result;
        public string ResultDetail;

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
                Result = (BingoResultType) dbResult.Value;
                ResultDetail = dbResultDetail;
            }
        }
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