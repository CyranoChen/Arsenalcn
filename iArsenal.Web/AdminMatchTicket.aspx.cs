using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminMatchTicket : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                #region Bind ddlLeague

                List<MatchTicket> mtList = MatchTicket.Cache.MatchTicketList;

                //var query = from mt in mtList
                //            group mt by new { mt.LeagueGuid, mt.LeagueName } into l
                //            select new
                //            {
                //                LeagueGuid = l.Key.LeagueGuid,
                //                LeagueName = l.Key.LeagueName
                //            };

                var list = mtList.GroupBy(mt => new { mt.LeagueGuid, mt.LeagueName })
                    .Select(l => new { l.Key.LeagueGuid, l.Key.LeagueName })
                    .OrderByDescending(l => l.LeagueName).ToList();

                ddlLeague.DataSource = list;
                ddlLeague.DataTextField = "LeagueName";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();

                ddlLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));

                #endregion

                #region Bind ddlProductCode

                List<Product> pList = Product.Cache.Load(ProductType.MatchTicket);

                ddlProductCode.DataSource = pList;
                ddlProductCode.DataTextField = "DisplayName";
                ddlProductCode.DataValueField = "Code";
                ddlProductCode.DataBind();

                ddlProductCode.Items.Insert(0, new ListItem("--请选择比赛等级--", string.Empty));

                #endregion

                if (!string.IsNullOrEmpty(ddlIsHome.SelectedValue))
                    ViewState["IsHome"] = ddlIsHome.SelectedValue;
                else
                    ViewState["IsHome"] = string.Empty;

                BindData();
            }
        }

        private Guid? _matchGuid = null;
        private Guid? MatchGuid
        {
            get
            {
                if (_matchGuid.HasValue && _matchGuid == Guid.Empty)
                    return _matchGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["MatchGuid"]))
                {
                    try { return new Guid(Request.QueryString["MatchGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _matchGuid = value; }
        }

        private void BindData()
        {
            var list = MatchTicket.All().FindAll(x =>
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["LeagueGuid"] != null)
                {
                    tmpString = ViewState["LeagueGuid"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.LeagueGuid.Equals(new Guid(tmpString));
                }

                if (ViewState["IsHome"] != null)
                {
                    tmpString = ViewState["IsHome"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.IsHome.Equals(Convert.ToBoolean(tmpString));
                }

                if (ViewState["ProductCode"] != null)
                {
                    tmpString = ViewState["ProductCode"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.ProductCode.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                }

                if (ViewState["AllowMemberClass"] != null)
                {
                    tmpString = ViewState["AllowMemberClass"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && x.AllowMemberClass.HasValue)
                        returnValue = returnValue && x.AllowMemberClass.Equals(Convert.ToInt16(tmpString));
                }

                if (ViewState["TeamName"] != null)
                {
                    tmpString = ViewState["TeamName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--对阵球队--")
                        returnValue = returnValue && x.TeamName.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (MatchGuid.HasValue && MatchGuid != Guid.Empty)
            {
                int i = list.FindIndex(mt => mt.ID.Equals(MatchGuid));
                if (i >= 0)
                {
                    gvMatchTicket.PageIndex = i / gvMatchTicket.PageSize;
                    gvMatchTicket.SelectedIndex = i % gvMatchTicket.PageSize;
                }
                else
                {
                    gvMatchTicket.PageIndex = 0;
                    gvMatchTicket.SelectedIndex = -1;
                }
            }
            else
            {
                gvMatchTicket.SelectedIndex = -1;
            }
            #endregion

            gvMatchTicket.DataSource = list;
            gvMatchTicket.DataBind();

            #region set Control Custom Pager
            if (gvMatchTicket.BottomPagerRow != null)
            {
                gvMatchTicket.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvMatchTicket.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvMatchTicket.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvMatchTicket_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatchTicket.PageIndex = e.NewPageIndex;
            MatchGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMatchTicket.PageIndex = e.PageIndex;
                MatchGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvMatchTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMatchTicket.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminMatchTicketView.aspx?MatchGuid={0}", gvMatchTicket.DataKeys[gvMatchTicket.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            MatchTicket.Cache.RefreshCache();
            //Arsenal_Player.Cache.RefreshCache();
            //Arsenal_Team.Cache.RefreshCache();

            MatchTicket.MatchTicketCountStatistics();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTeamName.Text.Trim()))
                ViewState["TeamName"] = tbTeamName.Text.Trim();
            else
                ViewState["TeamName"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatchTicket.PageIndex = 0;

            BindData();
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
            {
                ViewState["LeagueGuid"] = ddlLeague.SelectedValue;
            }
            else
                ViewState["LeagueGuid"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatchTicket.PageIndex = 0;

            BindData();
        }

        protected void ddlIsHome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsHome.SelectedValue))
                ViewState["IsHome"] = ddlIsHome.SelectedValue;
            else
                ViewState["IsHome"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatchTicket.PageIndex = 0;

            BindData();
        }

        protected void ddlProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCode.SelectedValue))
                ViewState["ProductCode"] = ddlProductCode.SelectedValue;
            else
                ViewState["ProductCode"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatchTicket.PageIndex = 0;

            BindData();
        }

        protected void ddlAllowMemberClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlAllowMemberClass.SelectedValue))
                ViewState["AllowMemberClass"] = ddlAllowMemberClass.SelectedValue;
            else
                ViewState["AllowMemberClass"] = string.Empty;

            MatchGuid = Guid.Empty;
            gvMatchTicket.PageIndex = 0;

            BindData();
        }

        protected void gvMatchTicket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MatchTicket mt = e.Row.DataItem as MatchTicket;

                Label lblHomeAway = e.Row.FindControl("lblHomeAway") as Label;

                if (lblHomeAway != null)
                {
                    lblHomeAway.Text = mt.IsHome ? "主场" : "客场";
                }
                else
                {
                    lblHomeAway.Visible = false;
                }
            }
        }
    }
}
