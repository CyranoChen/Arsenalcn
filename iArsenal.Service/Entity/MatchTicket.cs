using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using iArsenal.Service.Arsenal;
using iArsenal.Service.ServiceProvider;
using Arsenalcn.Core;
using System.Data.SqlClient;

namespace iArsenal.Service
{
    [AttrDbTable("iArsenal_MatchTicket", Key = "MatchGuid", Sort = "Deadline DESC")]
    public class MatchTicket : Entity<Guid>
    {
        public MatchTicket() : base() { }

        public MatchTicket(DataRow dr)
        {
            // Match Info Initializer
            Match m = Arsenal_Match.Cache.Load(ID);

            ID = m.ID;
            TeamGuid = m.TeamGuid;
            TeamName = m.TeamName;
            IsHome = m.IsHome;
            ResultHome = m.ResultHome;
            ResultAway = m.ResultAway;
            PlayTime = m.PlayTime;

            PlayTimeLocal = ConvertToDST(PlayTime);

            LeagueGuid = m.LeagueGuid;
            LeagueName = m.LeagueName;
            Round = m.Round;

            #region Generate Match ResultInfo

            if (ResultHome.HasValue && ResultAway.HasValue)
            {
                if (IsHome)
                    ResultInfo = ResultHome.Value.ToString() + "：" + ResultAway.Value.ToString();
                else
                    ResultInfo = ResultAway.Value.ToString() + "：" + ResultHome.Value.ToString();
            }
            else
                ResultInfo = string.Empty;

            #endregion

            // Ticket Info Initializer

            if (dr != null)
            {
                ProductCode = dr["ProductCode"].ToString();

                ProductInfo = Product.Cache.Load(ProductCode) != null
                    ? Product.Cache.Load(ProductCode).Name : string.Empty;

                Deadline = (DateTime)dr["Deadline"];

                if (!Convert.IsDBNull(dr["AllowMemberClass"]))
                    AllowMemberClass = Convert.ToInt16(dr["AllowMemberClass"]);
                else
                    AllowMemberClass = null;

                if (!Convert.IsDBNull(dr["TicketCount"]))
                    TicketCount = Convert.ToInt16(dr["TicketCount"]);
                else
                    TicketCount = null;

                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Remark = dr["Remark"].ToString();
            }
            else
            {
                ProductCode = string.Empty;
                ProductInfo = string.Empty;
                Deadline = m.PlayTime.AddMonths(-2).AddDays(-7);
                AllowMemberClass = null;
                TicketCount = null;
                IsActive = false;
                Remark = string.Empty;
            }
        }

        public static void MatchTicketCountStatistics()
        {
            try
            {
                var query = MatchTicket.Cache.MatchTicketList.FindAll(mt => mt.IsActive);

                if (query != null && query.Count > 0)
                {
                    IRepository repo = new Repository();

                    var oQuery = repo.Query<Order>(o => o.IsActive && o.OrderType.Equals(OrderBaseType.Ticket) && !o.Status.Equals(OrderStatusType.Error));
                    var oiQuery = repo.Query<OrderItem>(oi => oi.IsActive & !string.IsNullOrEmpty(oi.Remark));

                    foreach (MatchTicket mt in query)
                    {
                        var _count = oQuery.Count(o =>
                            oiQuery.Where(oi => oi.Remark.Equals(mt.ID.ToString()))
                            .Any(oi => oi.OrderID.Equals(o.ID)));


                        if (_count > 0 && !mt.TicketCount.Equals(_count))
                        {
                            mt.TicketCount = _count;
                        }
                        else if (_count == 0 && mt.TicketCount.HasValue)
                        {
                            mt.TicketCount = null;
                        }
                        else
                        {
                            continue;
                        }

                        repo.Update<MatchTicket>(mt);
                    }

                    MatchTicket.Cache.RefreshCache();
                }
            }
            catch
            { throw new Exception(); }
        }

        public bool Any()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE MatchGuid = @key",
                Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = { new SqlParameter("@key", ID), };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows[0] != null)
            { return true; }
            else
            { return false; }
        }

        private static DateTime ConvertToDST(DateTime date)
        {
            DateTime begDST = new DateTime(date.Year, 3, 31);
            DateTime endDST = new DateTime(date.Year, 11, 1);

            if (begDST.DayOfWeek != DayOfWeek.Sunday)
                begDST = begDST.AddDays(-((int)begDST.DayOfWeek));

            if (endDST.DayOfWeek == DayOfWeek.Sunday)
                endDST = endDST.AddDays(-7);
            else
                endDST = endDST.AddDays(-((int)endDST.DayOfWeek));

            if (date.AddHours(-7) > begDST && date.AddHours(-7) < endDST)
            {
                return date.AddHours(-7);
            }

            return date.AddHours(-8);
        }

        public bool CheckMemberCanPurchase(MemberPeriod mp)
        {
            // Check Member Class for Purchase the MatchTicket

            if (AllowMemberClass.HasValue)
            {
                if (mp != null && mp.IsActive && (int)mp.MemberClass >= AllowMemberClass.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static class Cache
        {
            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IRepository repo = new Repository();

                MatchTicketList = repo.All<MatchTicket>().ToList();
            }

            public static MatchTicket Load(Guid guid)
            {
                return MatchTicketList.Find(x => x.ID.Equals(guid));
            }

            public static List<MatchTicket> MatchTicketList;
        }

        #region Members and Properties

        // Match Info Properties

        public Guid TeamGuid
        { get; set; }

        public string TeamName
        { get; set; }

        public Boolean IsHome
        { get; set; }

        public int? ResultHome
        { get; set; }

        public int? ResultAway
        { get; set; }

        public string ResultInfo
        { get; set; }


        public DateTime PlayTime
        { get; set; }

        public DateTime PlayTimeLocal
        { get; set; }

        public Guid? LeagueGuid
        { get; set; }

        public string LeagueName
        { get; set; }

        public int? Round
        { get; set; }

        // Ticket Info Properties

        [AttrDbColumn("ProductCode")]
        public string ProductCode
        { get; set; }

        public string ProductInfo
        { get; set; }

        [AttrDbColumn("Deadline")]
        public DateTime Deadline
        { get; set; }

        [AttrDbColumn("AllowMemberClass")]
        public int? AllowMemberClass
        { get; set; }

        [AttrDbColumn("TicketCount")]
        public int? TicketCount
        { get; set; }

        [AttrDbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [AttrDbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}
