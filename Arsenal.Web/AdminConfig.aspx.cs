using System;
using System.Web.UI.WebControls;
using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminConfig : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            gvSysConfig.DataSource = _repo.Query<Config>(x => x.ConfigSystem.ToString() == ConfigSystem.Arsenal.ToString());
            gvSysConfig.DataBind();
        }

        protected void gvSysConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSysConfig.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvSysConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var tbConfigValue = gvSysConfig.Rows[gvSysConfig.EditIndex].Cells[1].Controls[0] as TextBox;

            if (tbConfigValue != null)
            {
                try
                {
                    var c = new Config
                    {
                        ConfigSystem = ConfigSystem.Arsenal,
                        ConfigKey = gvSysConfig.DataKeys[gvSysConfig.EditIndex]?.Value.ToString(),
                        ConfigValue = tbConfigValue.Text.Trim()
                    };

                    c.Save();

                    ConfigGlobal_Arsenal.Refresh();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            gvSysConfig.EditIndex = -1;

            BindData();
        }

        protected void gvSysConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSysConfig.EditIndex = -1;

            BindData();
        }

        protected void gvSysConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSysConfig.PageIndex = e.NewPageIndex;

            BindData();
        }

        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigGlobal_Arsenal.Refresh();

                RelationLeagueTeam.Clean();
                RelationLeagueTeam.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Match.Cache.RefreshCache();
                Player.Cache.RefreshCache();
                Team.Cache.RefreshCache();
                Video.Cache.RefreshCache();

                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                    "alert('更新全部缓存成功');window.location.href=window.location.href", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}