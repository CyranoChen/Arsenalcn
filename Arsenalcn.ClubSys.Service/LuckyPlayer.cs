using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Service
{
    public class LuckyPlayer
    {
        public static void GenerateLuckyPlayer()
        {
            if (ConfigGlobal.LuckyPlayerActive)
            {
                string sql = @"SELECT TOP 1 ID FROM dbo.AcnClub_Player WHERE (UserID IN
                          (SELECT UserID FROM dbo.AcnClub_RelationUserClub WHERE IsActive = 1)) ORDER BY NEWID()";

                int playerID = 0;

                using (SqlConnection con = SQLConn.GetConnection())
                {
                    SqlCommand com = new SqlCommand(sql, con);

                    con.Open();

                    playerID = (int)com.ExecuteScalar();

                    con.Close();
                }

                if (playerID > 0)
                {
                    sql = "IF NOT EXISTS (SELECT * FROM dbo.Arsenalcn_Config WHERE ConfigKey = 'LuckyPlayerID' AND ConfigSystem = 'AcnClub') INSERT INTO dbo.Arsenalcn_Config VALUES ('AcnClub', 'LuckyPlayerID', '-1'); UPDATE dbo.Arsenalcn_Config SET ConfigValue = @value WHERE ConfigKey = 'LuckyPlayerID' AND ConfigSystem = 'AcnClub'; ";
                    sql = sql + "IF NOT EXISTS (SELECT * FROM dbo.Arsenalcn_Config WHERE ConfigKey = 'LuckyPlayerBonusGot'  AND ConfigSystem = 'AcnClub') INSERT INTO dbo.Arsenalcn_Config VALUES ('AcnClub', 'LuckyPlayerBonusGot', 'false'); UPDATE dbo.Arsenalcn_Config SET ConfigValue = 'false' WHERE ConfigKey = 'LuckyPlayerBonusGot' AND ConfigSystem = 'AcnClub'; ";
                    sql = sql + "INSERT INTO dbo.AcnClub_LogLuckyPlayer (PlayerID, TotalBonus, [Date], BonusGot) values (@value, @bonus, getdate(), 0);";

                    using (SqlConnection con = SQLConn.GetConnection())
                    {
                        SqlCommand com = new SqlCommand(sql, con);
                        com.Parameters.Add(new SqlParameter("@value", playerID.ToString()));
                        com.Parameters.Add(new SqlParameter("@bonus", CalcTotalBonus()));

                        con.Open();

                        com.ExecuteNonQuery();

                        con.Close();
                    }
                }
            }
        }

        public static int CalcTotalBonus()
        {
            string sql = "SELECT COUNT(*) FROM dbo.AcnClub_LogBingo WHERE ActionDate BETWEEN @fromDate AND @toDate";

            int totalBonus = 0;

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                con.Open();

                com.Parameters.Add(new SqlParameter("@fromDate", DateTime.Today.AddDays(-1)));
                com.Parameters.Add(new SqlParameter("@toDate", DateTime.Today));

                int count = (int)com.ExecuteScalar();

                totalBonus = count * ConfigGlobal.BingoGetCost;

                con.Close();
            }

            return totalBonus;
        }

        public static void SetBonusGot(int playerID, int bonusToClub, int clubID, int luckyPlayerID)
        {
            string sql = "UPDATE dbo.Arsenalcn_Config SET ConfigValue = 'true' WHERE ConfigKey = 'LuckyPlayerBonusGot'; ";
            sql = sql + "UPDATE dbo.AcnClub_LogLuckyPlayer SET BonusGot = 1 WHERE [ID] = (SELECT TOP 1 [ID] FROM AcnClub_LogLuckyPlayer ORDER BY [Date] DESC); ";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
           
            Player player = PlayerStrip.GetPlayerInfoByPlayerID(playerID);
            Player lPlayer = PlayerStrip.GetPlayerInfoByPlayerID(luckyPlayerID);
    
            if (player != null && clubID > 0)
            {
                ClubHistory ch = new ClubHistory();
                ch.ClubID = clubID;
                ch.ActionUserName = lPlayer.UserName;
                ch.OperatorUserName = player.UserName;
                ch.ActionType = ClubHistoryActionType.LuckyPlayer.ToString();
                ch.ActionDescription = ClubLogic.BuildClubHistoryActionDesc(ClubHistoryActionType.LuckyPlayer, bonusToClub.ToString(), lPlayer.UserName.ToString());

                ClubLogic.SaveClubHistory(ch);
            }
        }

        public static DataTable GetLuckPlayerHistory()
        {
            string sql = "SELECT * FROM dbo.AcnClub_LogLuckyPlayer lp INNER JOIN dbo.AcnClub_Player p ON lp.PlayerID = p.[ID] order by lp.ID desc";

            using (SqlConnection con = SQLConn.GetConnection())
            {
                SqlCommand com = new SqlCommand(sql, con);

                SqlDataAdapter sda = new SqlDataAdapter(com);
                con.Open();

                DataTable dt = new DataTable();

                sda.Fill(dt);

                con.Close();

                return dt;
            }
        }
    }
}
