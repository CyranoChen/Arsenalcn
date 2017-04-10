using System;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminLeague : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

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
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var list = _repo.All<League>().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["LeagueName"] != null)
                {
                    tmpString = ViewState["LeagueName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--分类名称--")
                        returnValue = (x.LeagueName.ToLower().Contains(tmpString.ToLower()) ||
                                       x.LeagueOrgName.ToLower().Contains(tmpString.ToLower()));
                }

                if (ViewState["LeagueSeason"] != null)
                {
                    tmpString = ViewState["LeagueSeason"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && tmpString != "--所属赛季--")
                        returnValue = returnValue &&
                                      x.LeagueSeason.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                }

                if (ViewState["IsActive"] != null)
                {
                    tmpString = ViewState["IsActive"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue && x.IsActive.Equals(Convert.ToBoolean(tmpString));
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex

            if (LeagueGuid.HasValue && !LeagueGuid.Value.Equals(Guid.Empty))
            {
                var i = list.FindIndex(x => x.ID.Equals(LeagueGuid));

                if (i >= 0)
                {
                    gvLeague.PageIndex = i/gvLeague.PageSize;
                    gvLeague.SelectedIndex = i%gvLeague.PageSize;
                }
                else
                {
                    gvLeague.PageIndex = 0;
                    gvLeague.SelectedIndex = -1;
                }
            }
            else
            {
                gvLeague.SelectedIndex = -1;
            }

            #endregion

            gvLeague.DataSource = list;
            gvLeague.DataBind();

            #region set Control Custom Pager

            if (gvLeague.BottomPagerRow != null)
            {
                gvLeague.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvLeague.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvLeague.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }

            #endregion
        }

        protected void gvLeague_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLeague.PageIndex = e.NewPageIndex;
            LeagueGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvLeague.PageIndex = e.PageIndex;
                LeagueGuid = null;
            }

            BindData();
        }

        protected void gvLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvLeague.SelectedIndex != -1)
            {
                var key = gvLeague.DataKeys[gvLeague.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminLeagueView.aspx?LeagueGuid={key.Value}");
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            League.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void ddlIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsActive.SelectedValue))
                ViewState["IsActive"] = ddlIsActive.SelectedValue;
            else
                ViewState["IsActive"] = string.Empty;

            LeagueGuid = null;
            gvLeague.PageIndex = 0;

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLeagueName.Text.Trim()))
                ViewState["LeagueName"] = tbLeagueName.Text.Trim();
            else
                ViewState["LeagueName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbLeagueSeason.Text.Trim()))
                ViewState["LeagueSeason"] = tbLeagueSeason.Text.Trim();
            else
                ViewState["LeagueSeason"] = string.Empty;

            LeagueGuid = null;
            gvLeague.PageIndex = 0;

            BindData();
        }
    }
}