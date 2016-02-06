using System;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.Common.Entity;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class AdminConfig : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var list =
                Config.GetConfigs()
                    .FindAll(delegate(Config c) { return c.ConfigSystem.Equals(ConfigSystem.AcnCasino); });

            gvSysConfig.DataSource = list;
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
                    var c = new Config();

                    c.ConfigSystem = ConfigSystem.AcnCasino;
                    c.ConfigKey = gvSysConfig.DataKeys[gvSysConfig.EditIndex].Value.ToString();
                    c.ConfigValue = tbConfigValue.Text.Trim();

                    c.Update();
                    Config.Cache.RefreshCache();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
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
                Config.Cache.RefreshCache();

                League.Cache.RefreshCache();
                Team.Cache.RefreshCache();

                ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                    "alert('更新全部缓存成功');window.location.href=window.location.href", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}');", true);
            }
        }
    }
}