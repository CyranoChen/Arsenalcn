using System;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminLeagueView : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private Guid LeagueGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["LeagueGuid"]))
                {
                    try { return new Guid(Request.QueryString["LeagueGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (LeagueGuid != Guid.Empty)
            {
                IEntity entity = new Entity();
                League l = entity.Single<League>(LeagueGuid);

                tbLeagueGuid.Text = LeagueGuid.ToString();
                tbLeagueName.Text = l.LeagueName;
                tbLeagueOrgName.Text = l.LeagueOrgName;
                tbLeagueSeason.Text = l.LeagueSeason;
                tbLeagueTime.Text = l.LeagueTime.ToString("yyyy-MM-dd");
                tbLeagueLogo.Text = l.LeagueLogo;
                tbLeagueOrder.Text = l.LeagueOrder.ToString();
                cbIsActive.Checked = l.IsActive;
            }
            else
            {
                tbLeagueGuid.Text = Guid.NewGuid().ToString();
                tbLeagueTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                League l = new League();

                if (!string.IsNullOrEmpty(tbLeagueName.Text.Trim()))
                    l.LeagueName = tbLeagueName.Text.Trim();
                else
                    throw new Exception("请输入分类名称");

                l.LeagueOrgName = tbLeagueOrgName.Text.Trim();
                l.LeagueSeason = tbLeagueSeason.Text.Trim();
                l.LeagueTime = Convert.ToDateTime(tbLeagueTime.Text.Trim());
                l.LeagueLogo = tbLeagueLogo.Text.Trim();
                l.LeagueOrder = Convert.ToInt16(tbLeagueOrder.Text.Trim());
                l.IsActive = cbIsActive.Checked;

                if (LeagueGuid != Guid.Empty)
                {
                    l.LeagueGuid = LeagueGuid;
                    l.Update<League>(l);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    l.LeagueGuid = new Guid(tbLeagueGuid.Text.Trim());
                    l.Create<League>(l);
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "save", "alert('添加成功');window.location.href = 'AdminLeague.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (LeagueGuid != Guid.Empty)
            {
                Response.Redirect("AdminLeague.aspx?LeagueGuid=" + LeagueGuid.ToString());
            }
            else
            {
                Response.Redirect("AdminLeague.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (LeagueGuid != Guid.Empty)
                {
                    IEntity entity = new Entity();
                    entity.Delete<League>(LeagueGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminLeague.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除失败')", true);
            }
        }
    }
}
