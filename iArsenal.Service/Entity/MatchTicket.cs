using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Extension;

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
            var m = Arsenal_Match.Cache.Load(ID);

            ID = m.ID;
            TeamGuid = m.TeamGuid;
            TeamName = m.TeamName;
            IsHome = m.IsHome;
            ResultHome = m.ResultHome;
            ResultAway = m.ResultAway;
            PlayTime = m.PlayTime;

            PlayTimeLocal = ConvertToDst(PlayTime);

            LeagueGuid = m.LeagueGuid;
            LeagueName = m.LeagueName;
            Round = m.Round;

            #region Generate Match ResultInfo

            if (ResultHome.HasValue && ResultAway.HasValue)
            {
                if (IsHome)
                    ResultInfo = ResultHome.Value + "：" + ResultAway.Value;
                else
                    ResultInfo = ResultAway.Value + "：" + ResultHome.Value;
            }
            else
                ResultInfo = string.Empty;

            #endregion

            // Ticket Info Initializer

            if (dr != null)
            {
                ProductCode = dr["ProductCode"].ToString();

                ProductInfo = Product.Cache.Load(ProductCode) != null
                    ? Product.Cache.Load(ProductCode).Name
                    : string.Empty;

                Deadline = (DateTime)dr["Deadline"];
                WaitingDeadline = (DateTime)dr["WaitingDeadline"];

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
                WaitingDeadline = m.PlayTime.AddMonths(-1).AddDays(-7);
                AllowMemberClass = null;
                TicketCount = null;
                IsActive = false;
                Remark = string.Empty;
            }
        }

        public void Single()
        {
            var sql = $"SELECT * FROM {Repository.GetTableAttr<MatchTicket>().Name} WHERE MatchGuid = @key";

            var dapper = DapperHelper.GetInstance();

            var dt = dapper.ExecuteDataTable(sql, new { key = ID });

            Init(dt?.Rows[0]);
        }

        public bool Any()
        {
            var sql = $"SELECT COUNT(*) FROM {Repository.GetTableAttr<MatchTicket>().Name} WHERE MatchGuid = @key";

            //SqlParameter[] para = { new SqlParameter("@key", ID) };

            var dapper = DapperHelper.GetInstance();

            return dapper.ExecuteScalar<int>(sql, new { key = ID }) > 0;
        }

        // Get MatchTickets by Arsenal Matchs
        public static List<MatchTicket> All()
        {
            var mlist = Arsenal_Match.Cache.MatchList;

            if (mlist != null && mlist.Count > 0)
            {
                // Get DataSet of iArsenal_MatchTicket 
                var attr = Repository.GetTableAttr<MatchTicket>();
                var sql = $"SELECT * FROM {attr.Name} ORDER BY {attr.Sort}";

                var dapper = DapperHelper.GetInstance();

                var dt = dapper.ExecuteDataTable(sql);

                if (dt != null)
                {
                    dt.PrimaryKey = new[] { dt.Columns["MatchGuid"] };
                }

                var list = new List<MatchTicket>();

                foreach (var m in mlist)
                {
                    var mt = new MatchTicket { ID = m.ID };

                    if (dt != null)
                    {
                        var dr = dt.Rows.Find(m.ID);
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
            return null;
        }

        public void Create()
        {
            var sql =
                @"INSERT INTO {0} (MatchGuid, ProductCode, Deadline, WaitingDeadline, AllowMemberClass, TicketCount, IsActive, Remark) 
                               VALUES (@key, @productCode, @deadline, @waitingDeadline, @allowMemberClass, @ticketCount, @isActive, @remark)";

            sql = string.Format(sql, Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para =
            {
                new SqlParameter("@key", ID),
                new SqlParameter("@productCode", ProductCode),
                new SqlParameter("@deadline", Deadline),
                new SqlParameter("@waitingDeadline", WaitingDeadline),
                new SqlParameter("@allowMemberClass",
                    !AllowMemberClass.HasValue ? DBNull.Value : (object) AllowMemberClass.Value),
                new SqlParameter("@TicketCount", !TicketCount.HasValue ? DBNull.Value : (object) TicketCount.Value),
                new SqlParameter("@isActive", IsActive),
                new SqlParameter("@remark", Remark)
            };

            var dapper = DapperHelper.GetInstance();

            dapper.Execute(sql, para.ToDapperParameters());
        }

        public void Update()
        {
            var sql =
                @"UPDATE {0} SET ProductCode = @productCode, Deadline = @deadline, WaitingDeadline = @waitingDeadline, 
                    AllowMemberClass = @allowMemberClass, TicketCount = @ticketCount, IsActive = @isActive, Remark = @remark WHERE MatchGuid = @key";

            sql = string.Format(sql, Repository.GetTableAttr<MatchTicket>().Name);

            SqlParameter[] para =
            {
                new SqlParameter("@key", ID),
                new SqlParameter("@productCode", ProductCode),
                new SqlParameter("@deadline", Deadline),
                new SqlParameter("@waitingDeadline", WaitingDeadline),
                new SqlParameter("@allowMemberClass",
                    !AllowMemberClass.HasValue ? DBNull.Value : (object) AllowMemberClass.Value),
                new SqlParameter("@TicketCount", !TicketCount.HasValue ? DBNull.Value : (object) TicketCount.Value),
                new SqlParameter("@isActive", IsActive),
                new SqlParameter("@remark", Remark)
            };

            var dapper = DapperHelper.GetInstance();

            dapper.Execute(sql, para.ToDapperParameters());
        }

        public void Delete()
        {
            var sql = $"DELETE FROM {Repository.GetTableAttr<MatchTicket>().Name} WHERE MatchGuid = @key";

            var dapper = DapperHelper.GetInstance();

            dapper.Execute(sql, new { key = ID });
        }

        public static void MatchTicketCountStatistics()
        {
            var matches = Cache.MatchTicketList.FindAll(mt => mt.IsActive);

            if (matches.Any())
            {
                IRepository repo = new Repository();

                var oQuery = repo.Query<Order>(o => o.OrderType == OrderBaseType.Ticket)
                    .FindAll(o => o.IsActive && !o.Status.Equals(OrderStatusType.Error));
                var oiQuery = repo.Query<OrderItem>(oi => oi.Remark != string.Empty)
                    .FindAll(x => x.IsActive);

                foreach (var mt in matches)
                {
                    var list = oiQuery.FindAll(oi => oi.Remark.Equals(mt.ID.ToString()));
                    var count = oQuery.FindAll(o => list.Any(oi => oi.OrderID.Equals(o.ID))).Count;

                    if (count > 0 && !mt.TicketCount.Equals(count))
                    {
                        mt.TicketCount = count;
                    }
                    else if (count == 0 && mt.TicketCount.HasValue)
                    {
                        mt.TicketCount = null;
                    }
                    else
                    {
                        continue;
                    }

                    mt.Update();
                }

                Cache.RefreshCache();
            }
        }

        private static DateTime ConvertToDst(DateTime date)
        {
            var begDst = new DateTime(date.Year, 3, 31);
            var endDst = new DateTime(date.Year, 11, 1);

            if (begDst.DayOfWeek != DayOfWeek.Sunday)
                begDst = begDst.AddDays(-((int)begDst.DayOfWeek));

            endDst = endDst.DayOfWeek == DayOfWeek.Sunday ? endDst.AddDays(-7) : endDst.AddDays(-(int)endDst.DayOfWeek);

            if (date.AddHours(-7) > begDst && date.AddHours(-7) < endDst)
            {
                return date.AddHours(-7);
            }

            return date.AddHours(-8);
        }

        public bool CheckMemberCanPurchase(MemberPeriod mp)
        {
            // Check Member Class for Purchase the MatchTicket
            bool retValue;

            if (AllowMemberClass.HasValue)
            {
                retValue = mp != null && mp.IsActive && (int)mp.MemberClass >= AllowMemberClass.Value;
            }
            else
            {
                retValue = true;
            }

            return retValue;
        }

        public static class Cache
        {
            public static List<MatchTicket> MatchTicketList;

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
        }

        #region Members and Properties

        // Match Info Properties
        [DbColumn("MatchGuid", IsKey = true)]
        public Guid ID { get; set; }

        public Guid TeamGuid { get; set; }

        public string TeamName { get; set; }

        public bool IsHome { get; set; }

        public int? ResultHome { get; set; }

        public int? ResultAway { get; set; }

        public string ResultInfo { get; set; }


        public DateTime PlayTime { get; set; }

        public DateTime PlayTimeLocal { get; set; }

        public Guid? LeagueGuid { get; set; }

        public string LeagueName { get; set; }

        public int? Round { get; set; }

        // Ticket Info Properties

        [DbColumn("ProductCode")]
        public string ProductCode { get; set; }

        public string ProductInfo { get; set; }

        [DbColumn("Deadline")]
        public DateTime Deadline { get; set; }

        [DbColumn("WaitingDeadline")]
        public DateTime WaitingDeadline { get; set; }

        [DbColumn("AllowMemberClass")]
        public int? AllowMemberClass { get; set; }

        [DbColumn("TicketCount")]
        public int? TicketCount { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        #endregion
    }
}