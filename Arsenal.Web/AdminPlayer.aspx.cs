using System;
using System.Linq;
using System.Web.UI.WebControls;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminPlayer : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                #region Bind ddlSquadNumber
                ddlSquadNumber.DataSource = Player.Cache.ColList_SquadNumber;
                ddlSquadNumber.DataBind();

                ddlSquadNumber.Items.Insert(0, new ListItem("--球员号码--", string.Empty));
                #endregion

                #region Bind ddlPosition
                ddlPosition.DataSource = Player.Cache.ColList_Position;
                ddlPosition.DataBind();

                ddlPosition.Items.Insert(0, new ListItem("--球员位置--", string.Empty));
                #endregion

                BindData();
            }
        }

        private Guid? _playerGuid = null;
        private Guid? PlayerGuid
        {
            get
            {
                if (_playerGuid.HasValue && _playerGuid == Guid.Empty)
                    return _playerGuid;
                else if (!string.IsNullOrEmpty(Request.QueryString["PlayerGuid"]))
                {
                    try { return new Guid(Request.QueryString["PlayerGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return null;
            }
            set { _playerGuid = value; }
        }

        private void BindData()
        {
            var list = repo.All<Player>().ToList().FindAll(x =>
                {
                    Boolean returnValue = true;
                    string tmpString = string.Empty;

                    if (ViewState["SquadNumber"] != null)
                    {
                        tmpString = ViewState["SquadNumber"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.SquadNumber.Equals(Convert.ToInt16(tmpString));
                    }

                    if (ViewState["Position"] != null)
                    {
                        tmpString = ViewState["Position"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.Position.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                    }

                    if (ViewState["IsLegend"] != null)
                    {
                        tmpString = ViewState["IsLegend"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.IsLegend.Equals(Convert.ToBoolean(tmpString));
                    }

                    if (ViewState["DisplayName"] != null)
                    {
                        tmpString = ViewState["DisplayName"].ToString();
                        if (!string.IsNullOrEmpty(tmpString) && tmpString != "--球员姓名--")
                            returnValue = returnValue && x.DisplayName.ToLower().Contains(tmpString.ToLower());
                    }

                    return returnValue;
                });

            #region set GridView Selected PageIndex
            if (PlayerGuid.HasValue && !PlayerGuid.Value.Equals(Guid.Empty))
            {
                int i = list.FindIndex(x => x.ID.Equals(PlayerGuid));
                if (i >= 0)
                {
                    gvPlayer.PageIndex = i / gvPlayer.PageSize;
                    gvPlayer.SelectedIndex = i % gvPlayer.PageSize;
                }
                else
                {
                    gvPlayer.PageIndex = 0;
                    gvPlayer.SelectedIndex = -1;
                }
            }
            else
            {
                gvPlayer.SelectedIndex = -1;
            }
            #endregion

            gvPlayer.DataSource = list;
            gvPlayer.DataBind();

            #region set Control Custom Pager
            if (gvPlayer.BottomPagerRow != null)
            {
                gvPlayer.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvPlayer.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvPlayer.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvPlayer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayer.PageIndex = e.NewPageIndex;
            PlayerGuid = Guid.Empty;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvPlayer.PageIndex = e.PageIndex;
                PlayerGuid = Guid.Empty;
            }

            BindData();
        }

        protected void gvPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPlayer.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminPlayerView.aspx?PlayerGuid={0}", gvPlayer.DataKeys[gvPlayer.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Player.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void ddlSquadNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSquadNumber.SelectedValue))
                ViewState["SquadNumber"] = ddlSquadNumber.SelectedValue;
            else
                ViewState["SquadNumber"] = string.Empty;

            PlayerGuid = Guid.Empty;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPosition.SelectedValue))
                ViewState["Position"] = ddlPosition.SelectedValue;
            else
                ViewState["Position"] = string.Empty;

            PlayerGuid = Guid.Empty;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void ddlIsLegend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsLegend.SelectedValue))
                ViewState["IsLegend"] = ddlIsLegend.SelectedValue;
            else
                ViewState["IsLegend"] = string.Empty;

            PlayerGuid = Guid.Empty;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbDisplayName.Text.Trim()))
                ViewState["DisplayName"] = tbDisplayName.Text.Trim();
            else
                ViewState["DisplayName"] = string.Empty;

            PlayerGuid = Guid.Empty;
            gvPlayer.PageIndex = 0;

            BindData();
        }
    }
}