using System;
using System.Web.UI.WebControls;
using System.Linq;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
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
            gvSysConfig.DataSource = Config.All(ConfigSystem.iArsenal).ToList();
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

                    var c = new Config();

                    c.ConfigSystem = ConfigSystem.iArsenal;
                    c.ConfigKey = gvSysConfig.DataKeys[gvSysConfig.EditIndex].Value.ToString();

                    c.ConfigValue = tbConfigValue.Text.Trim();

                    c.Update();
                    Config.Cache.RefreshCache();
                }

                gvSysConfig.EditIndex = -1;

                BindData();
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
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
                Config.Cache.RefreshCache();

                Arsenal_Match.Cache.RefreshCache();
                Arsenal_Player.Cache.RefreshCache();
                Arsenal_Team.Cache.RefreshCache();

                MatchTicket.Cache.RefreshCache();
                Member.Cache.RefreshCache();
                Product.Cache.RefreshCache();

                Order.RefreshOrderBaseType();

                ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新全部缓存成功');window.location.href=window.location.href", true);
            }
            catch (Exception ex)
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }
    }
}
