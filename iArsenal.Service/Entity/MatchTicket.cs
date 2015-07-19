using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using iArsenal.Service.Arsenal;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_MatchTicket", Key = "MatchGuid", Sort = "Deadline DESC")]
    public class MatchTicket
    {
        public MatchTicket() { }

        private MatchTicket(DataRow dr)
        {
            Init(dr);
        }

        private void Init(DataRow dr = null)
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

        public void Single()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE MatchGuid = @key",
                Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@key", ID), 
                                  };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Init(ds.Tables[0].Rows[0]);
            }
            else
            {
                Init();
            }
        }

        public bool Any()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE MatchGuid = @key",
                Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = { new SqlParameter("@key", ID), };

            DataSet ds = DataAccess.ExecuteDataset(sql, para);

            return ds.Tables[0].Rows.Count > 0;
        }

        // Get MatchTickets by Arsenal Matchs
        public static List<MatchTicket> All()
        {
            var mlist = Arsenal_Match.Cache.MatchList;

            if (mlist != null && mlist.Count > 0)
            {
                // Get DataSet of iArsenal_MatchTicket 
                var attr = Repository.GetTableAttr<MatchTicket>();
                string sql = string.Format("SELECT * FROM {0} ORDER BY {1}", attr.Name, attr.Sort);
                DataSet ds = DataAccess.ExecuteDataset(sql);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["MatchGuid"] };
                }

                var list = new List<MatchTicket>();

                foreach (Match m in mlist)
                {
                    MatchTicket mt = new MatchTicket();
                    mt.ID = m.ID;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows.Find(m.ID);
                        mt.Init(dr);
                    }
                    else
                    {
                        mt.Init();
                    }

                    list.Add(mt);
                }

                return list;
            }
            else
            {
                return null;
            }
        }

        public void Create(SqlTransaction trans = null)
        {
            string sql = @"INSERT INTO {0} (MatchGuid, ProductCode, Deadline, AllowMemberClass, TicketCount, IsActive, Remark) 
                               VALUES (@key, @productCode, @deadline, @allowMemberClass, @ticketCount, @isActive, @remark)";

            sql = string.Format(sql, Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@key", ID),
                                      new SqlParameter("@productCode", ProductCode),
                                      new SqlParameter("@deadline", Deadline),
                                      new SqlParameter("@allowMemberClass", !AllowMemberClass.HasValue ? (object)DBNull.Value : (object)AllowMemberClass.Value),
                                      new SqlParameter("@TicketCount", !TicketCount.HasValue ? (object)DBNull.Value : (object)TicketCount.Value),
                                      new SqlParameter("@isActive", IsActive),
                                      new SqlParameter("@remark", Remark)
                                  };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Update(SqlTransaction trans = null)
        {
            string sql = @"UPDATE {0} SET ProductCode = @productCode, Deadline = @deadline, AllowMemberClass = @allowMemberClass,
                                  TicketCount = @ticketCount, IsActive = @isActive, Remark = @remark WHERE MatchGuid = @key";

            sql = string.Format(sql, Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = {
                                      new SqlParameter("@key", ID),
                                      new SqlParameter("@productCode", ProductCode),
                                      new SqlParameter("@deadline", Deadline),
                                      new SqlParameter("@allowMemberClass", !AllowMemberClass.HasValue ? (object)DBNull.Value : (object)AllowMemberClass.Value),
                                      new SqlParameter("@TicketCount", !TicketCount.HasValue ? (object)DBNull.Value : (object)TicketCount.Value),
                                      new SqlParameter("@isActive", IsActive),
                                      new SqlParameter("@remark", Remark)
                                  };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public void Delete(SqlTransaction trans = null)
        {
            string sql = string.Format("DELETE FROM {0} WHERE MatchGuid = @key",
                Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para = { new SqlParameter("@key", ID) };

            DataAccess.ExecuteNonQuery(sql, para, trans);
        }

        public static void MatchTicketCountStatistics()
        {
            var query = MatchTicket.Cache.MatchTicketList.FindAll(mt => mt.IsActive);

            if (query != null && query.Count > 0)
            {
                IRepository repo = new Repository();

                var oQuery = repo.Query<Order>(o => o.IsActive && o.OrderType.Equals(OrderBaseType.Ticket) && !o.Status.Equals(OrderStatusType.Error));
                var oiQuery = repo.Query<OrderItem>(oi => oi.IsActive & !string.IsNullOrEmpty(oi.Remark));

                foreach (MatchTicket mt in query)
                {
                    var _list = oiQuery.Where(oi => oi.Remark.Equals(mt.ID.ToString())).ToList();
                    var _count = oQuery.Count(o => _list.Any(oi => oi.OrderID.Equals(o.ID)));

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

                    mt.Update();
                }

                MatchTicket.Cache.RefreshCache();
            }
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
                MatchTicketList = All();
            }

            public static MatchTicket Load(Guid guid)
            {
                return MatchTicketList.Find(x => x.ID.Equals(guid));
            }

            public static List<MatchTicket> MatchTicketList;
        }

        #region Members and Properties

        // Match Info Properties
        [DbColumn("MatchGuid", Key = true)]
        public Guid ID
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

        [DbColumn("ProductCode")]
        public string ProductCode
        { get; set; }

        public string ProductInfo
        { get; set; }

        [DbColumn("Deadline")]
        public DateTime Deadline
        { get; set; }

        [DbColumn("AllowMemberClass")]
        public int? AllowMemberClass
        { get; set; }

        [DbColumn("TicketCount")]
        public int? TicketCount
        { get; set; }

        [DbColumn("IsActive")]
        public Boolean IsActive
        { get; set; }

        [DbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}
