using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using iArsenal.Entity;
using ArsenalTeam = iArsenal.Entity.Arsenal.Team;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_MatchList : PageBase.AcnPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);
            tbTeamName.Attributes["placeholder"] = "--对阵球队--";

            if (!IsPostBack)
            {
                #region Bind ddlProductCode

                List<Product> pList = Product.Cache.Load(ProductType.MatchTicket).FindAll(p => p.IsActive);

                ddlProductCode.DataSource = pList;
                ddlProductCode.DataTextField = "DisplayName";
                ddlProductCode.DataValueField = "Code";
                ddlProductCode.DataBind();

                ddlProductCode.Items.Insert(0, new ListItem("--请选择比赛等级--", string.Empty));

                #endregion

                BindData();
            }
        }

        private Guid? _leagueGuid = null;
        private Guid? LeagueGuid
        {
            get
            {
                if (_leagueGuid.HasValue && _leagueGuid == Guid.Empty)
                    return _leagueGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["LeagueGuid"]))
                {
                    try { return new Guid(Request.QueryString["LeagueGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _leagueGuid = value; }
        }

        private void BindData()
        {
            try
            {
                List<MatchTicket> list = MatchTicket.GetMatchTickets().FindAll(delegate(MatchTicket mt)
                {
                    Boolean returnValue = true;
                    string tmpString = string.Empty;

                    if (ViewState["ProductCode"] != null)
                    {
                        tmpString = ViewState["ProductCode"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && mt.ProductCode.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                    }

                    if (ViewState["TeamName"] != null)
                    {
                        tmpString = ViewState["TeamName"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && mt.TeamName.ToLower().Contains(tmpString.ToLower());
                    }

                    returnValue = returnValue && mt.IsActive && mt.IsHome;
                    returnValue = returnValue && !string.IsNullOrEmpty(mt.ProductCode);

                    if (ConfigGlobal.DefaultMatchDate.HasValue)
                    {
                        returnValue = returnValue && mt.PlayTime > ConfigGlobal.DefaultMatchDate.Value;
                    }

                    return returnValue;
                });

                gvMatch.DataSource = list;
                gvMatch.DataBind();

                #region set Control Custom Pager
                if (gvMatch.BottomPagerRow != null)
                {
                    gvMatch.BottomPagerRow.Visible = true;
                    ctrlCustomPagerInfo.Visible = true;

                    ctrlCustomPagerInfo.PageIndex = gvMatch.PageIndex;
                    ctrlCustomPagerInfo.PageCount = gvMatch.PageCount;
                    ctrlCustomPagerInfo.RowCount = list.Count;
                    ctrlCustomPagerInfo.InitComponent();
                }
                else
                {
                    ctrlCustomPagerInfo.Visible = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvMatch.PageIndex = e.PageIndex;
            }

            BindData();
        }

        protected void gvMatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MatchTicket mt = e.Row.DataItem as MatchTicket;
                ArsenalTeam at = Team.Cache.Load(mt.TeamGuid);

                string _strRank = mt.ProductInfo.Trim();

                Literal ltrlTeamInfo = e.Row.FindControl("ltrlTeamInfo") as Literal;
                Label lblMatchTicketRank = e.Row.FindControl("lblMatchTicketRank") as Label;
                Label lblMatchDeadlineOrResult = e.Row.FindControl("lblMatchDeadlineOrResult") as Label;
                HyperLink hlTicketApply = e.Row.FindControl("hlTicketApply") as HyperLink;

                if (ltrlTeamInfo != null)
                {
                    ltrlTeamInfo.Text = string.Format("<span class=\"MatchTicket_TeamInfo\"><img src=\"{0}\" alt=\"{1}\" /></span>", ConfigGlobal.AcnCasinoURL + at.TeamLogo, at.TeamEnglishName);
                }
                else
                {
                    ltrlTeamInfo.Text = string.Empty;
                }

                if (lblMatchTicketRank != null && !string.IsNullOrEmpty(_strRank))
                {
                    lblMatchTicketRank.Text = _strRank.Substring(_strRank.Length - 7, 7);
                }
                else
                {
                    lblMatchTicketRank.Text = string.Empty;
                }

                if (lblMatchDeadlineOrResult != null && !string.IsNullOrEmpty(mt.ResultInfo))
                {
                    lblMatchDeadlineOrResult.Text = string.Format("<em>{0}</em>", mt.ResultInfo);
                }
                else
                {
                    lblMatchDeadlineOrResult.Text = string.Format("<em>{0}</em>", mt.Deadline.ToString("yyyy-MM-dd"));
                }

                if (hlTicketApply != null && mt.Deadline >= DateTime.Now.AddDays(-1))
                {
                    hlTicketApply.NavigateUrl = string.Format("iArsenalOrder_MatchTicket.aspx?MatchGuid={0}", mt.MatchGuid.ToString());
                    hlTicketApply.Target = "_self";
                    hlTicketApply.Visible = true;
                }
                else
                {
                    hlTicketApply.Visible = false;
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTeamName.Text.Trim()))
                ViewState["TeamName"] = tbTeamName.Text.Trim();
            else
                ViewState["TeamName"] = string.Empty;

            gvMatch.PageIndex = 0;

            BindData();
        }

        protected void ddlProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCode.SelectedValue))
                ViewState["ProductCode"] = ddlProductCode.SelectedValue;
            else
                ViewState["ProductCode"] = string.Empty;

            gvMatch.PageIndex = 0;

            BindData();
        }
    }
}