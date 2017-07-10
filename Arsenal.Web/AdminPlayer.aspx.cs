using System;
using System.Linq;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenal.Web.Control;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminPlayer : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid? _playerGuid;

        private Guid? PlayerGuid
        {
            get
            {
                if (_playerGuid.HasValue && _playerGuid == Guid.Empty)
                    return _playerGuid;
                if (!string.IsNullOrEmpty(Request.QueryString["PlayerGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["PlayerGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return null;
            }
            set { _playerGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;
            ctrlCustomPagerInfo.PageChanged += ctrlCustomPagerInfo_PageChanged;

            if (!IsPostBack)
            {
                #region Bind ddlSquadNumber

                ddlSquadNumber.DataSource = Player.Cache.ColListSquadNumber;
                ddlSquadNumber.DataBind();

                ddlSquadNumber.Items.Insert(0, new ListItem("--球员号码--", string.Empty));

                #endregion

                #region Bind ddlPosition

                ddlPosition.DataSource = Player.Cache.ColListPosition;
                ddlPosition.DataBind();

                ddlPosition.Items.Insert(0, new ListItem("--球员位置--", string.Empty));

                #endregion

                BindData();
            }
        }

        private void BindData()
        {
            var list = _repo.All<Player>().ToList().FindAll(x =>
            {
                var returnValue = true;
                string tmpString;

                if (ViewState["SquadNumber"] != null)
                {
                    tmpString = ViewState["SquadNumber"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = x.SquadNumber.Equals(Convert.ToInt16(tmpString));
                }

                if (ViewState["PlayerPosition"] != null)
                {
                    tmpString = ViewState["PlayerPosition"].ToString();
                    if (!string.IsNullOrEmpty(tmpString))
                        returnValue = returnValue &&
                                      x.PlayerPosition.ToString().Equals(tmpString, StringComparison.OrdinalIgnoreCase);
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
                var i = list.FindIndex(x => x.ID.Equals(PlayerGuid));
                if (i >= 0)
                {
                    gvPlayer.PageIndex = i/gvPlayer.PageSize;
                    gvPlayer.SelectedIndex = i%gvPlayer.PageSize;
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
            PlayerGuid = null;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvPlayer.PageIndex = e.PageIndex;
                PlayerGuid = null;
            }

            BindData();
        }

        protected void gvPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPlayer.SelectedIndex != -1)
            {
                var key = gvPlayer.DataKeys[gvPlayer.SelectedIndex];
                if (key != null)
                    Response.Redirect($"AdminPlayerView.aspx?PlayerGuid={key.Value}");
            }
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            Player.Cache.RefreshCache();

            ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                "alert('更新缓存成功');window.location.href=window.location.href", true);
        }

        protected void ddlSquadNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSquadNumber.SelectedValue))
                ViewState["SquadNumber"] = ddlSquadNumber.SelectedValue;
            else
                ViewState["SquadNumber"] = string.Empty;

            PlayerGuid = null;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPosition.SelectedValue))
                ViewState["PlayerPosition"] = ddlPosition.SelectedValue;
            else
                ViewState["PlayerPosition"] = string.Empty;

            PlayerGuid = null;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void ddlIsLegend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlIsLegend.SelectedValue))
                ViewState["IsLegend"] = ddlIsLegend.SelectedValue;
            else
                ViewState["IsLegend"] = string.Empty;

            PlayerGuid = null;
            gvPlayer.PageIndex = 0;

            BindData();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbDisplayName.Text.Trim()))
                ViewState["DisplayName"] = tbDisplayName.Text.Trim();
            else
                ViewState["DisplayName"] = string.Empty;

            PlayerGuid = null;
            gvPlayer.PageIndex = 0;

            BindData();
        }
    }
}