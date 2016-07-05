using System;
using System.Data.SqlClient;
using System.Web;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class ServerMembershipCheck : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var strOrderId = context.Request.QueryString["OrderID"];
                    int id;

                    IRepository repo = new Repository();

                    if (!string.IsNullOrEmpty(strOrderId) && int.TryParse(strOrderId, out id))
                    {
                        // Get Current MatchTicket Order
                        var oTicket = (OrdrTicket)Order.Select(id);

                        // Get Order MatchTicket Info
                        var oiMatchTicket = oTicket?.OIMatchTicket;

                        if (oiMatchTicket != null && oiMatchTicket.IsActive)
                        {
                            var mt = MatchTicket.Cache.Load(oiMatchTicket.MatchGuid);
                            var mp = MemberPeriod.GetCurrentMemberPeriodByMemberID(oTicket.MemberID);

                            // isMemberCouldPurchase, should be false
                            if (!mt.CheckMemberCanPurchase(mp))
                            {
                                var currProductType = mt.AllowMemberClass == 2 ?
                                    ProductType.MembershipPremier : ProductType.MembershipCore;

                                var p = Product.Cache.Load(currProductType).Find(x => x.IsActive);

                                var isUpgrade = (mp != null && mp.MemberClass.Equals(MemberClassType.Core) &&
                                                mt.AllowMemberClass.HasValue && mt.AllowMemberClass == 2);

                                #region isRenew
                                var isRenew = false;

                                var mpLast = MemberPeriod.GetCurrentMemberPeriodByMemberID(oTicket.MemberID, -1);

                                if (mpLast != null && mpLast.MemberClass.Equals(MemberClassType.Core))
                                {
                                    isRenew = currProductType.Equals(ProductType.MembershipCore);
                                }
                                if (mpLast != null && mpLast.MemberClass.Equals(MemberClassType.Premier))
                                {
                                    isRenew = currProductType.Equals(ProductType.MembershipPremier);
                                }
                                #endregion

                                #region new membership order

                                var o = new Order
                                {
                                    Mobile = oTicket.Mobile,
                                    UpdateTime = DateTime.Now,
                                    Description = string.Empty,
                                    OrderType = OrderBaseType.Membership,
                                    MemberID = oTicket.MemberID,
                                    MemberName = oTicket.MemberName,
                                    Address = oTicket.Address,
                                    Payment = string.Empty,
                                    Price = 0,
                                    Sale = null,
                                    Deposit = null,
                                    Status = OrderStatusType.Submitted,
                                    Rate = 0,
                                    CreateTime = DateTime.Now,
                                    IsActive = true,
                                    Remark = string.Empty
                                };

                                //Get the Order ID after Insert new one
                                object key;
                                repo.Insert(o, out key, trans);
                                var newId = Convert.ToInt32(key);
                                #endregion

                                #region new membership orderItem
                                //New Order Items
                                var oi = new OrdrItmMembership();

                                // Set AlterMethod & MemberCardNo
                                if (isUpgrade)
                                {
                                    oi.AlterMethod = "Upgrade";
                                    oi.MemberCardNo = mp.MemberCardNo;
                                }
                                else if (isRenew)
                                {
                                    oi.AlterMethod = "Renew";
                                    oi.MemberCardNo = mpLast.MemberCardNo;
                                }
                                else
                                {
                                    oi.AlterMethod = string.Empty;

                                    var rand = new Random(Guid.NewGuid().GetHashCode());
                                    oi.MemberCardNo = rand.Next(100, 999).ToString();
                                }

                                oi.EndDate = CurrSeasonDeadline;

                                oi.OrderID = newId;
                                oi.Quantity = 1;

                                #region set membership sale
                                var pPremier = Product.Cache.Load("iMS2");
                                var pCore = Product.Cache.Load("iMS1");

                                if (isUpgrade)
                                {
                                    oi.Sale = pPremier.PriceCNY - pCore.PriceCNY;
                                }
                                else if (isRenew)
                                {
                                    oi.Sale = currProductType.Equals(ProductType.MembershipPremier)
                                        ? Convert.ToDouble(Math.Floor(pPremier.PriceCNY * 0.88)) : pCore.PriceCNY;
                                }
                                else
                                {
                                    oi.Sale = null;
                                }
                                #endregion

                                oi.Place(Member.Cache.Load(o.MemberID), p, trans);

                                #endregion

                                trans.Commit();

                                context.Response.Redirect($"~/iArsenalOrderView_Membership.aspx?OrderID={newId}", false);
                                context.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                }
                catch
                {
                    context.Response.Redirect("iArsenalOrder.aspx", false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
        }

        public bool IsReusable => false;

        private DateTime CurrSeasonDeadline
        {
            get
            {
                // Set Default Deadline yyyy-05-31 23:59:59
                var seasonDeadline = new DateTime(DateTime.Now.Year, 6, 1).AddSeconds(-1);

                if (DateTime.Now >= seasonDeadline)
                {
                    return seasonDeadline.AddYears(1);
                }
                return seasonDeadline;
            }
        }
    }
}