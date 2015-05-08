using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.Common;
using Arsenalcn.Common.Entity;

namespace Arsenal.Web
{
    public partial class AdminConfig : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            List<Config> list = Config.GetConfigs().FindAll(delegate(Config c) { return c.ConfigSystem.Equals(ConfigSystem.Arsenal); });

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
            TextBox tbConfigValue = gvSysConfig.Rows[gvSysConfig.EditIndex].Cells[1].Controls[0] as TextBox;

            if (tbConfigValue != null)
            {
                try
                {
                    Config c = new Config();

                    c.ConfigSystem = ConfigSystem.Arsenal;
                    c.ConfigKey = gvSysConfig.DataKeys[gvSysConfig.EditIndex].Value.ToString();
                    c.ConfigValue = tbConfigValue.Text.Trim();

                    c.Update();
                    Config.Cache.RefreshCache();
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
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

                Service.League.Cache.RefreshCache();
                Service.Match.Cache.RefreshCache();
                Service.Player.Cache.RefreshCache();
                Service.Team.Cache.RefreshCache();
                Service.Video.Cache.RefreshCache();

                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新全部缓存成功');window.location.href=window.location.href", true);
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }
    }
}
