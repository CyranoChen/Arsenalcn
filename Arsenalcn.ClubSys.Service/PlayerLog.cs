using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.Common;

namespace Arsenalcn.ClubSys.Service
{
    public class PlayerLog
    {
        public static void LogHistory(int userId, string userName, PlayerHistoryType type, string desc)
        {
            var sql = "INSERT INTO dbo.AcnClub_LogPlayer VALUES (@userid, @username, @typeCode, @typeDesc, getdate());";

            using (var con = SQLConn.GetConnection())
            {
                var com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("@userid", userId));
                com.Parameters.Add(new SqlParameter("@username", userName));
                com.Parameters.Add(new SqlParameter("@typeCode", (int) type));
                com.Parameters.Add(new SqlParameter("@typeDesc", desc));

                con.Open();

                com.ExecuteNonQuery();

                //con.Close();
            }
        }

        public static List<GamerHistory> GetUserPlayerHistory(int userID)
        {
            var list = new List<GamerHistory>();

            var sql = "SELECT * FROM dbo.AcnClub_LogPlayer WHERE UserID = @userID ORDER BY ActionDate DESC";

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
                    var ph = new GamerHistory(dr);

                    list.Add(ph);
                }
            }

            return list;
        }
    }

    public enum PlayerHistoryType
    {
        ActivateCard = 1,
        ActivateVideo = 2,
        ConsolidateCards = 3,
        Award = 4
    }

    public interface PlayerHistoryDescGenerator
    {
        string Generate();
    }

    public class ActivateCardDesc : PlayerHistoryDescGenerator
    {
        private readonly Card _un;

        public ActivateCardDesc(Card un)
        {
            _un = un;
        }

        #region PlayerHistoryDescGenerator Members

        public string Generate()
        {
            var _pName = Player.Cache.Load(_un.ArsenalPlayerGuid.Value).DisplayName;

            return string.Format("激活卡片{0}({1})", _pName, _un.ID);
        }

        #endregion
    }

    public class ActivateVideoDesc : PlayerHistoryDescGenerator
    {
        private readonly Card _un;

        public ActivateVideoDesc(Card un)
        {
            _un = un;
        }

        #region PlayerHistoryDescGenerator Members

        public string Generate()
        {
            return string.Format("激活视频({0})", _un.ID);
        }

        #endregion
    }

    public class ConsolidateCardsDesc : PlayerHistoryDescGenerator
    {
        private readonly Card _un1;
        private readonly Card _un2;

        public ConsolidateCardsDesc(Card un1, Card un2)
        {
            _un1 = un1;
            _un2 = un2;
        }

        #region PlayerHistoryDescGenerator Members

        public string Generate()
        {
            var _pName1 = Player.Cache.Load(_un1.ArsenalPlayerGuid.Value).DisplayName;
            var _pName2 = Player.Cache.Load(_un2.ArsenalPlayerGuid.Value).DisplayName;

            return string.Format("融合卡片{0}({1})和卡片{2}({3})", _pName1, _un1.ID, _pName2, _un2.ID);
        }

        #endregion
    }

    public class AwardDesc : PlayerHistoryDescGenerator
    {
        private readonly bool _card;
        private readonly float _cash;
        private readonly float _rp;
        private readonly bool _video;

        public AwardDesc(float cash, float rp, bool card, bool video)
        {
            _cash = cash;
            _rp = rp;
            _card = card;
            _video = video;
        }

        #region PlayerHistoryDescGenerator Members

        public string Generate()
        {
            var awardDesc = "获得奖励:";
            if (_cash != 0)
            {
                awardDesc += string.Format(" {0}枪手币", _cash);
            }

            if (_rp != 0)
            {
                awardDesc += string.Format(" {0}RP", _rp);
            }

            if (_card)
            {
                awardDesc += " 球星卡一张";
            }

            if (_video)
            {
                awardDesc += " 视频卡一张";
            }

            return awardDesc;
        }

        #endregion
    }
}