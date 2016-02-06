using System;
using System.Web.UI.WebControls;
using iArsenal.Service;
using iArsenal.Web.Control;
using ArsenalTeam = iArsenal.Service.Arsenal.Team;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_MatchList : AcnPageBase
    {
        private Guid? _leagueGuid;

        private Guid? LeagueGuid
        {
            get
            {
                if (_leagueGuid.HasValue && _leagueGuid == Guid.Empty)
                    return _leagueGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["LeagueGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["LeagueGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _leagueGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;
            tbTeamName.Attributes["placeholder"] = "--对阵球队--";

            if (!IsPostBack)
            {
                #region Bind ddlProductCode

                var pList = Product.Cache.Load(ProductType.MatchTicket).FindAll(p => p.IsActive);

                ddlProductCode.DataSource = pList;
                ddlProductCode.DataTextField = "DisplayName";
                ddlProductCode.DataValueField = "Code";
                ddlProductCode.DataBind();

                ddlProductCode.Items.Insert(0, new ListItem("--请选择比赛等级--", string.Empty));

                #endregion

                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                var list = MatchTicket.Cache.MatchTicketList.FindAll(delegate(MatchTicket mt)
                {
                    var returnValue = true;
                    var tmpString = string.Empty;

                    if (ViewState["ProductCode"] != null)
                    {
                        tmpString = ViewState["ProductCode"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue &&
                                          mt.ProductCode.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
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
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void gvMatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMatch.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
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
                var mt = e.Row.DataItem as MatchTicket;
                var at = Arsenal_Team.Cache.Load(mt.TeamGuid);

                var _strRank = mt.ProductInfo.Trim();

                var ltrlTeamInfo = e.Row.FindControl("ltrlTeamInfo") as Literal;
                var lblMatchTicketRank = e.Row.FindControl("lblMatchTicketRank") as Label;
                var lblMatchDeadlineOrResult = e.Row.FindControl("lblMatchDeadlineOrResult") as Label;
                var hlAllowMemberClass = e.Row.FindControl("hlAllowMemberClass") as HyperLink;
                var hlTicketApply = e.Row.FindControl("hlTicketApply") as HyperLink;

                if (ltrlTeamInfo != null)
                {
                    ltrlTeamInfo.Text =
                        $"<span class=\"MatchTicket_TeamInfo\"><img src=\"{ConfigGlobal.AcnCasinoURL + at.TeamLogo}\" alt=\"{at.TeamEnglishName}\" /></span>";
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
                    lblMatchDeadlineOrResult.Text = $"<em>{mt.ResultInfo}</em>";
                }
                else
                {
                    lblMatchDeadlineOrResult.Text = $"<em>{mt.Deadline.ToString("yyyy-MM-dd")}</em>";
                }

                if (hlAllowMemberClass != null && mt.AllowMemberClass.HasValue)
                {
                    if (mt.AllowMemberClass.Value == 1)
                    {
                        hlAllowMemberClass.Text = "Core";
                        hlAllowMemberClass.ToolTip = "普通(Core)会员以上可预订";
                        hlAllowMemberClass.NavigateUrl = "iArsenalMemberPeriod.aspx";
                        hlAllowMemberClass.Visible = true;
                    }
                    else if (mt.AllowMemberClass.Value == 2)
                    {
                        hlAllowMemberClass.Text = "<em>Premier</em>";
                        hlAllowMemberClass.ToolTip = "只限高级(Premier)会员可预订";
                        hlAllowMemberClass.NavigateUrl = "iArsenalMemberPeriod.aspx";
                        hlAllowMemberClass.Visible = true;
                    }
                    else
                    {
                        hlAllowMemberClass.Visible = false;
                    }
                }
                else
                {
                    hlAllowMemberClass.Visible = false;
                }

                if (hlTicketApply != null && mt.Deadline > DateTime.Now)
                {
                    hlTicketApply.NavigateUrl = $"iArsenalOrder_MatchTicket.aspx?MatchGuid={mt.ID}";
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