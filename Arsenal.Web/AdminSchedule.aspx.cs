using System;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Web
{
    public partial class AdminSchedule : AdminPageBase
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
            gvSchedule.DataSource = _repo.All<Schedule>();
            gvSchedule.DataBind();
        }

        protected void gvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILog log = new AppLog();
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                if (gvSchedule.SelectedIndex != -1)
                {
                    var dataKey = gvSchedule.DataKeys[gvSchedule.SelectedIndex];
                    if (dataKey != null)
                    {
                        var key = dataKey.Value.ToString();

                        var s = _repo.Single<Schedule>(x => x.ScheduleKey == key);

                        var instance = s.IScheduleInstance;
                        ManagedThreadPool.QueueUserWorkItem(instance.Execute);

                        s.LastCompletedTime = DateTime.Now;
                        _repo.Update(s, x => x.ScheduleKey == s.ScheduleKey);

                        log.Info($"ISchedule Manually: {s.ScheduleType}", logInfo);
                    }

                    //this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", string.Format("任务：{0}执行成功');", s.ScheduleType), true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, logInfo);

                //this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
        }

        protected void gvSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSchedule.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var tbScheduleType = gvSchedule.Rows[gvSchedule.EditIndex].Cells[1].Controls[0] as TextBox;
            var tbDailyTime = gvSchedule.Rows[gvSchedule.EditIndex].Cells[3].Controls[0] as TextBox;
            var tbMinutes = gvSchedule.Rows[gvSchedule.EditIndex].Cells[4].Controls[0] as TextBox;
            var tbIsSystem = gvSchedule.Rows[gvSchedule.EditIndex].Cells[6].Controls[0] as TextBox;
            var tbIsActive = gvSchedule.Rows[gvSchedule.EditIndex].Cells[7].Controls[0] as TextBox;

            if (tbScheduleType != null && tbDailyTime != null && tbMinutes != null
                && tbIsSystem != null && tbIsActive != null)
            {
                try
                {
                    var dataKey = gvSchedule.DataKeys[gvSchedule.EditIndex];
                    if (dataKey != null)
                    {
                        var s = _repo.Single<Schedule>(x => x.ScheduleKey == dataKey.Value.ToString());

                        s.ScheduleType = tbScheduleType.Text.Trim();
                        s.DailyTime = Convert.ToInt32(tbDailyTime.Text.Trim());
                        s.Minutes = Convert.ToInt32(tbMinutes.Text.Trim());
                        s.IsSystem = Convert.ToBoolean(tbIsSystem.Text.Trim());
                        s.IsActive = Convert.ToBoolean(tbIsActive.Text.Trim());

                        _repo.Update(s, x => x.ScheduleKey == s.ScheduleKey);
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}');", true);
                }
            }

            gvSchedule.EditIndex = -1;

            BindData();
        }

        protected void gvSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSchedule.EditIndex = -1;

            BindData();
        }

        protected void gvSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSchedule.PageIndex = e.NewPageIndex;

            BindData();
        }
    }
}