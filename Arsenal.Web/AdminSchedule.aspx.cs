﻿using System;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;

using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Web
{
    public partial class AdminSchedule : AdminPageBase
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
            gvSchedule.DataSource = Schedule.All();
            gvSchedule.DataBind();
        }

        protected void gvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILog log = new AppLog();
            var logInfo = new LogInfo()
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                if (gvSchedule.SelectedIndex != -1)
                {
                    string key = gvSchedule.DataKeys[gvSchedule.SelectedIndex].Value.ToString();

                    Schedule s = new Schedule();
                    s.ScheduleKey = key;
                    s.Single();

                    ISchedule instance = s.IScheduleInstance;
                    ManagedThreadPool.QueueUserWorkItem(new WaitCallback(instance.Execute));

                    s.LastCompletedTime = DateTime.Now;
                    s.Update();

                    log.Info(string.Format("ISchedule Manually: {0}", s.ScheduleType), logInfo);

                    //this.ClientScript.RegisterClientScriptBlock(typeof(string), "success", string.Format("任务：{0}执行成功');", s.ScheduleType), true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, logInfo);

                //this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
            }
            finally
            {
                //BindData();
            }
        }

        protected void gvSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSchedule.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox tbScheduleType = gvSchedule.Rows[gvSchedule.EditIndex].Cells[1].Controls[0] as TextBox;
            TextBox tbDailyTime = gvSchedule.Rows[gvSchedule.EditIndex].Cells[3].Controls[0] as TextBox;
            TextBox tbMinutes = gvSchedule.Rows[gvSchedule.EditIndex].Cells[4].Controls[0] as TextBox;
            TextBox tbIsSystem = gvSchedule.Rows[gvSchedule.EditIndex].Cells[6].Controls[0] as TextBox;
            TextBox tbIsActive = gvSchedule.Rows[gvSchedule.EditIndex].Cells[7].Controls[0] as TextBox;

            if (tbScheduleType != null && tbDailyTime != null && tbMinutes != null
                && tbIsSystem != null && tbIsActive != null)
            {
                try
                {
                    var s = new Schedule();
                    s.ScheduleKey = gvSchedule.DataKeys[gvSchedule.EditIndex].Value.ToString();
                    s.Single();

                    s.ScheduleType = tbScheduleType.Text.Trim();
                    s.DailyTime = Convert.ToInt32(tbDailyTime.Text.Trim());
                    s.Minutes = Convert.ToInt32(tbMinutes.Text.Trim());
                    s.IsSystem = Convert.ToBoolean(tbIsSystem.Text.Trim());
                    s.IsActive = Convert.ToBoolean(tbIsActive.Text.Trim());

                    s.Update();
                }
                catch (Exception ex)
                {
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}');", ex.Message.ToString()), true);
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
