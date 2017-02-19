using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
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
            gvSysConfig.DataSource = _repo.All<Config>().FindAll(x => x.ConfigSystemInfo == ConfigSystem.iArsenal);
            gvSysConfig.DataBind();
        }

        protected void gvSysConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSysConfig.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvSysConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var tbConfigValue = gvSysConfig.Rows[gvSysConfig.EditIndex].Cells[1].Controls[0] as TextBox;

                if (tbConfigValue != null)
                {
                    var c = new Config(ConfigSystem.iArsenal,
                        gvSysConfig.DataKeys[gvSysConfig.EditIndex]?.Value.ToString(),
                        tbConfigValue.Text.Trim());

                    c.Save();

                    ConfigGlobal.Refresh();
                }

                gvSysConfig.EditIndex = -1;

                BindData();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
            }
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
                ConfigGlobal.Refresh();

                Arsenal_Match.Cache.RefreshCache();
                Arsenal_Player.Cache.RefreshCache();
                Arsenal_Team.Cache.RefreshCache();

                MatchTicket.Cache.RefreshCache();
                Member.Cache.RefreshCache();
                Product.Cache.RefreshCache();
                Showcase.Cache.RefreshCache();

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