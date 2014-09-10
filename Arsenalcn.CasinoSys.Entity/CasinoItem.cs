using System;
using System.Data;
using System.Data.SqlClient;

using Arsenalcn.CasinoSys.DataAccess;
using System.Collections.Generic;

namespace Arsenalcn.CasinoSys.Entity
{
    public abstract class CasinoItem
    {
        public CasinoItem() { }

        public static CasinoItem CreateInstance(CasinoType itemType)
        {
            CasinoItem item;

            switch (itemType)
            {
                case CasinoType.SingleChoice:
                    item = new SingleChoice();
                    item.ItemType = CasinoType.SingleChoice;
                    break;
                case CasinoType.MatchResult:
                    item = new MatchResult();
                    item.ItemType = CasinoType.MatchResult;
                    break;
                default:
                    throw new Exception("Unknown Casino Item Type.");
            }

            return item;
        }

        protected abstract void BuildDetail();

        public static CasinoItem GetCasinoItem(Guid itemID)
        {
            DataRow dr = DataAccess.CasinoItem.GetCasinoItemByID(itemID);

            if (dr != null)
            {
                CasinoType itemType = (CasinoType)Enum.Parse(typeof(CasinoType), Convert.ToString(dr["ItemType"]));

                CasinoItem item = CasinoItem.CreateInstance(itemType);

                item.ItemGuid = itemID;

                item.ItemType = itemType;

                if (Convert.IsDBNull(dr["MatchGuid"]))
                    item.MatchGuid = null;
                else
                    item.MatchGuid = (Guid)dr["MatchGuid"];

                if (Convert.IsDBNull(dr["ItemTitle"]))
                    item.ItemTitle = null;
                else
                    item.ItemTitle = Convert.ToString(dr["ItemTitle"]);

                if (Convert.IsDBNull(dr["ItemBody"]))
                    item.ItemBody = null;
                else
                    item.ItemBody = Convert.ToString(dr["ItemBody"]);

                item.CreateTime = Convert.ToDateTime(dr["CreateTime"]);
                item.PublishTime = Convert.ToDateTime(dr["PublishTime"]);
                item.CloseTime = Convert.ToDateTime(dr["CloseTime"]);

                item.BankerID = (Guid)dr["BankerID"];
                item.BankerName = Convert.ToString(dr["BankerName"]);

                if (Convert.IsDBNull(dr["Earning"]))
                    item.Earning = null;
                else
                    item.Earning = Convert.ToSingle(dr["Earning"]);

                item.OwnerID = Convert.ToInt32(dr["OwnerID"]);
                item.OwnerUserName = Convert.ToString(dr["OwnerUserName"]);

                item.BuildDetail();

                return item;
            }
            else
                return null;
        }

        public virtual Guid Save(SqlTransaction trans)
        {
            if (ItemGuid.HasValue)
            {
                //update
                if (Earning.HasValue)
                {
                    DataAccess.CasinoItem.UpdateCasinoItem(ItemGuid.Value, Earning.Value, trans);
                }

                return ItemGuid.Value;
            }
            else
            {
                //insert
                Guid newGuid = DataAccess.CasinoItem.InsertCasinoItem(ItemType.ToString(), MatchGuid, ItemTitle, ItemBody, PublishTime, CloseTime, BankerID, BankerName, OwnerID, OwnerUserName, trans);

                return newGuid;
            }
        }

        public static void UpdateCasinoItemCloseTime(Guid matchGuid, DateTime closeTime)
        {
            DataAccess.CasinoItem.UpdateCasinoItemCloseTime(matchGuid, closeTime);
        }

        public static Guid? GetCasinoItemGuidByMatch(Guid matchGuid, CasinoType type)
        {
            return DataAccess.CasinoItem.GetCasinoItemGuidByMatch(matchGuid, type.ToString(), null);
        }

        public static void ActiveCasinoItemStatistics()
        {
            DataTable dt = DataAccess.CasinoItem.GetActiveCasinoItem();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CasinoItem item = CasinoItem.GetCasinoItem((Guid)dr["CasinoItemGuid"]);
                    item.Earning = DataAccess.Bet.GetTotalEarningByCasinoItemGuid((Guid)dr["CasinoItemGuid"]);

                    item.Save(null);
                }
            }
        }

        public static DataTable GetMatchCasinoItemView(bool isOpen)
        {
            DataTable dt = null;

            if (isOpen)
            {     
                //OpenMatchView
                dt = DataAccess.CasinoItem.GetOpenMatchView(ConfigGlobal.CasinoValidDays);
            }
            else
            {
                //AllMatchView
                dt = DataAccess.CasinoItem.GetAllMatchView();
            }

            return dt;
        }

        public static DataTable GetEndViewByMatch()
        {
            return DataAccess.CasinoItem.GetEndMatchView();
        }

        public static DataTable GetEndViewByMatch(Guid leagueGuid)
        {
            return DataAccess.CasinoItem.GetEndMatchView(leagueGuid);
        }

        public static DataTable GetEndViewByMatch(Guid leagueGuid, Guid groupGuid, bool isTable)
        {
            return DataAccess.CasinoItem.GetEndMatchView(leagueGuid, groupGuid, isTable);
        }

        public static DataTable GetEndViewByTeam(Guid teamGuid)
        {
            DataTable dt = DataAccess.CasinoItem.GetEndMatchViewByTeamGuid(teamGuid);

            //if (dt != null)
            //{
            //    dt.Columns.Add("LeagueDisplayName", typeof(string));
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (!Convert.IsDBNull(dr["Round"]))
            //            dr["LeagueDisplayName"] = string.Format("{0}{1}(第{2}轮)", dr["LeagueName"].ToString(), dr["LeagueSeason"].ToString(), dr["Round"].ToString());
            //        else
            //            dr["LeagueDisplayName"] = string.Format("{0}{1}", dr["LeagueName"].ToString(), dr["LeagueSeason"].ToString());
            //    }
            //}

            return dt;
        }

        public static DataTable GetHistoryViewByMatch(Guid matchGuid)
        {
            Match match = new Match(matchGuid);

            DataTable dt = DataAccess.CasinoItem.GetEndMatchViewByTeams(match.Home, match.Away);

            //if (dt != null)
            //{
            //    dt.Columns.Add("LeagueDisplayName", typeof(string));
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (!Convert.IsDBNull(dr["Round"]))
            //            dr["LeagueDisplayName"] = string.Format("{0}{1}(第{2}轮)", dr["LeagueName"].ToString(), dr["LeagueSeason"].ToString(), dr["Round"].ToString());
            //        else
            //            dr["LeagueDisplayName"] = string.Format("{0}{1}", dr["LeagueName"].ToString(), dr["LeagueSeason"].ToString());
            //    }
            //}

            return dt;
        }

        public static int[] GetHistoryResultByMatch(Guid matchGuid)
        {
            Match match = new Match(matchGuid);
            int[] intArr = new int[4];
            int matchCount = 0;
            int wonCount = 0;
            int drawCount = 0;
            int loseCount = 0;

            DataTable dt = DataAccess.CasinoItem.GetEndMatchViewByTeams(match.Home, match.Away);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Match historyMatch = new Match((Guid)dr["MatchGuid"]);

                    if (match.Home == historyMatch.Home && match.Away == historyMatch.Away)
                    {
                        if (historyMatch.ResultHome > historyMatch.ResultAway)
                            wonCount++;
                        else if (historyMatch.ResultHome < historyMatch.ResultAway)
                            loseCount++;
                        else
                            drawCount++;
                    }
                    else if (match.Home == historyMatch.Away && match.Away == historyMatch.Home)
                    {
                        if (historyMatch.ResultHome < historyMatch.ResultAway)
                            wonCount++;
                        else if (historyMatch.ResultHome > historyMatch.ResultAway)
                            loseCount++;
                        else
                            drawCount++;
                    }
                }
                matchCount = wonCount + drawCount + loseCount;
            }

            intArr[0] = matchCount;
            intArr[1] = wonCount;
            intArr[2] = drawCount;
            intArr[3] = loseCount;

            return intArr;
        }

        public static int GetMatchCasinoItemCount()
        {
            return DataAccess.CasinoItem.GetMatchCasinoItemCount();
        }

        public static int GetOtherCasinoItemCount()
        {
            return DataAccess.CasinoItem.GetOtherCasinoItemCount();
        }

        public static DataTable GetTopMatchEarning(out int months)
        {
            return DataAccess.CasinoItem.GetTopMatchEarning(out months);
        }

        public static DataTable GetTopMatchLoss(out int months)
        {
            return DataAccess.CasinoItem.GetTopMatchLoss(out months);
        }

        public Guid? ItemGuid
        { get; set; }

        public CasinoType ItemType
        { get; set; }

        public Guid? MatchGuid
        { get; set; }

        public string ItemTitle
        { get; set; }

        public string ItemBody
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public DateTime PublishTime
        { get; set; }

        public DateTime CloseTime
        { get; set; }

        public Guid BankerID
        { get; set; }

        public string BankerName
        { get; set; }

        public float? Earning
        { get; set; }

        public int OwnerID
        { get; set; }

        public string OwnerUserName
        { get; set; }

        public enum CasinoType
        {
            SingleChoice,
            MatchResult
        }
    }
}
