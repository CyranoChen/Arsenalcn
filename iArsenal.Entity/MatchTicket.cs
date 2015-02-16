using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using iArsenal.Entity.Arsenal;
using iArsenal.Entity.ServiceProvider;

namespace iArsenal.Entity
{
    public class MatchTicket
    {
        public MatchTicket() { }

        private MatchTicket(DataRow dr)
        {
            InitMatchTicket(dr);
        }

        private void InitMatchTicket(DataRow dr)
        {

            // Match Info Initializer

            Match m = Arsenal_Match.Cache.Load(MatchGuid);

            MatchGuid = m.MatchGuid;
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

        public void Select()
        {
            DataRow dr = DataAccess.MatchTicket.GetMatchTicketByID(MatchGuid);

            InitMatchTicket(dr);
        }

        public void Update()
        {
            DataAccess.MatchTicket.UpdateMatchTicket(MatchGuid, ProductCode, Deadline, AllowMemberClass, TicketCount, IsActive, Remark);
        }

        public void Insert()
        {
            DataAccess.MatchTicket.InsertMatchTicket(MatchGuid, ProductCode, Deadline, AllowMemberClass, TicketCount, IsActive, Remark);
        }

        public void Delete()
        {
            DataAccess.MatchTicket.DeleteMatchTicket(MatchGuid);
        }

        public static List<MatchTicket> GetMatchTickets()
        {
            List<Match> mlist = Arsenal_Match.Cache.MatchList;

            if (mlist != null && mlist.Count > 0)
            {
                List<MatchTicket> list = new List<MatchTicket>();

                foreach (Match m in mlist)
                {
                    MatchTicket mt = new MatchTicket();
                    mt.MatchGuid = m.MatchGuid;
                    mt.Select();

                    list.Add(mt);
                }

                return list;
            }
            else
            {
                return null;
            }
        }

        public static void MatchTicketCountStatistics()
        {
            try
            {
                List<MatchTicket> list = MatchTicket.Cache.MatchTicketList.FindAll(mt => mt.IsActive);

                if (list != null && list.Count > 0)
                {
                    List<OrderBase> oList = OrderBase.GetOrders().FindAll(o =>
                        o.IsActive && o.OrderType.Equals(OrderBaseType.Ticket) && !o.Status.Equals(OrderStatusType.Error));
                    List<OrderItemBase> oiList = OrderItemBase.GetOrderItems().FindAll(oi =>
                        oi.IsActive & !string.IsNullOrEmpty(oi.Remark));

                    foreach (MatchTicket mt in list)
                    {
                        List<OrderBase> tList = oList.FindAll(o =>
                            oiList.FindAll(oi => oi.Remark.Equals(mt.MatchGuid.ToString()))
                            .Any(oi => oi.OrderID.Equals(o.OrderID)));

                        if (tList.Count > 0 && !mt.TicketCount.Equals(tList.Count))
                        {
                            mt.TicketCount = tList.Count;
                        }
                        else if (tList.Count == 0 && mt.TicketCount.HasValue)
                        {
                            mt.TicketCount = null;
                        }
                        else
                        {
                            continue;
                        }

                        mt.Update();
                    }

                    MatchTicket.Cache.RefreshCache();
                }
            }
            catch
            { throw new Exception(); }
        }

        //public static List<MatchTicket> GetMatchTickets_IncludeOrder()
        //{
        //    List<Match> mlist = Arsenal_Match.Cache.MatchList;

        //    if (mlist != null && mlist.Count > 0)
        //    {
        //        List<MatchTicket> list = new List<MatchTicket>();

        //        List<OrderBase> oList = OrderBase.GetOrders().FindAll(o => o.IsActive);
        //        List<OrderItemBase> oiList = OrderItemBase.GetOrderItems().FindAll(oi =>
        //            oi.IsActive & !string.IsNullOrEmpty(oi.Remark));

        //        foreach (Match m in mlist)
        //        {
        //            MatchTicket mt = new MatchTicket();
        //            mt.MatchGuid = m.MatchGuid;
        //            mt.Select();

        //            // Find out Order Ticket List
        //            mt.OrderTicketList = oList.FindAll(o =>
        //                oiList.FindAll(oi => oi.Remark.Equals(mt.MatchGuid.ToString()))
        //                .Any(oi => oi.OrderID.Equals(o.OrderID)));

        //            list.Add(mt);
        //        }

        //        return list;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public bool Exist()
        {
            DataRow dr = DataAccess.MatchTicket.GetMatchTicketByID(MatchGuid);

            if (dr != null)
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
                MatchTicketList = GetMatchTickets();
            }

            public static MatchTicket Load(Guid guid)
            {
                return MatchTicketList.Find(mt => mt.MatchGuid.Equals(guid));
            }

            public static List<MatchTicket> MatchTicketList;
        }

        #region Members and Properties

        // Match Info Properties

        public Guid MatchGuid
        { get; set; }

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

        public string ProductCode
        { get; set; }

        public string ProductInfo
        { get; set; }

        public DateTime Deadline
        { get; set; }

        public int? AllowMemberClass
        { get; set; }

        public int? TicketCount
        { get; set; }

        public Boolean IsActive
        { get; set; }

        public string Remark
        { get; set; }

        //public List<OrderBase> OrderTicketList
        //{ get; set; }

        #endregion
    }
}
