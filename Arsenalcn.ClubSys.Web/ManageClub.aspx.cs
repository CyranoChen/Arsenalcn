using System;
using System.Text;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageClub : BasePage
    {
        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, club.FullName);

            if (!IsPostBack)
            {
                LoadPageData();
            }

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlManageMenuTabBar.CurrentMenu = ManageClubMenuItem.ManageClub;
            ctrlManageMenuTabBar.UserID = userid;

            #endregion
        }

        protected void LoadPageData()
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null)
            {
                // Current User must be the manager of this club or Administrator
                if (club.ManagerUid.Value.Equals(userid) || ConfigAdmin.IsPluginAdmin(userid))
                {
                    pnlInaccessible.Visible = false;
                    phContent.Visible = true;

                    //Init Form
                    ltrlClubName.Text = club.FullName;
                    ltrlClubManagerName.Text = club.ManagerUserName;
                    ltrlFullName.Text = club.FullName;
                    ltrlShortName.Text = club.ShortName;

                    if (club.IsAppliable.Value)
                        rblAppliable.SelectedValue = "true";
                    else
                        rblAppliable.SelectedValue = "false";

                    tbManager.Text = club.ManagerUserName;

                    if (club.RankLevel == 0)
                    {
                        phExecutor.Visible = false;
                    }
                    else
                    {
                        phExecutor.Visible = true;

                        var users = ClubLogic.GetClubLeads(ClubID);
                        var sbEx = new StringBuilder();
                        foreach (var user in users)
                        {
                            if (user.Responsibility.Value == (int) Responsibility.Executor)
                            {
                                sbEx.AppendFormat("{0}|", user.UserName);
                            }
                        }

                        if (sbEx.Length != 0)
                        {
                            sbEx.Remove(sbEx.Length - 1, 1);
                        }

                        tbExecutor.Text = sbEx.ToString();
                    }

                    tbSlogan.Text = club.Slogan;
                    tbDesc.Text = club.Description;

                    if (club.LogoName != string.Empty)
                    {
                        ltrlLogo.Visible = true;
                        ltrlLogo.Text =
                            string.Format(
                                "<a href=\"{0}\" target=\"_blank\" title=\"点击放大\"><img src=\"{0}\" width=\"24\" height=\"24\" alt=\"点击放大\" /></a>",
                                "UploadFiles/" + club.LogoName);
                    }
                    else
                    {
                        ltrlLogo.Visible = false;
                    }
                }
                else
                {
                    pnlInaccessible.Visible = true;
                    phContent.Visible = false;
                }
            }
            else
            {
                pnlInaccessible.Visible = true;
                phContent.Visible = false;
            }
        }

        protected void linkButtonReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void linkButtonSave_Click(object sender, EventArgs e)
        {
            if (fuLogo.PostedFile.ContentLength != 0)
            {
                var logoName = fuLogo.FileName;

                if (logoName.ToLower().LastIndexOf(".gif") != logoName.Length - 4 &&
                    logoName.ToLower().LastIndexOf(".jpg") != logoName.Length - 4 &&
                    logoName.ToLower().LastIndexOf(".png") != logoName.Length - 4)
                {
                    //invalid logo file
                    var invalidAlert = "alert('请上传扩展名为gif，jpg或png的文件！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "invalid_logo_file", invalidAlert, true);

                    LoadPageData();

                    return;
                }

                if (fuLogo.FileBytes.LongLength > 100*1024)
                {
                    var fileLengthAlert = "alert('请上传小于100K的文件！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "file_too_large", fileLengthAlert, true);

                    LoadPageData();

                    return;
                }
            }

            //validate assignment
            var club = ClubLogic.GetClubInfo(ClubID);

            //check manager
            if (tbManager.Text != club.ManagerUserName)
            {
                //check new manager existance in discuz
                if (Users.GetUserId(tbManager.Text) <= 0)
                {
                    //alert
                    var script = "alert('会长用户在系统中不存在！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "user_not_exist", script, true);

                    LoadPageData();

                    return;
                }

                var managerID = Users.GetUserId(tbManager.Text);
                //check if the new manager is a member of the club, if not, alert
                if (ClubLogic.GetActiveUserClub(managerID, ClubID) == null)
                {
                    var script = "alert('新会长必须为该球会成员！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "user_not_member", script, true);

                    LoadPageData();

                    return;
                }
            }

            //check executor
            if (tbExecutor.Text == string.Empty)
            {
                //change current executor to normal member
                var users = ClubLogic.GetClubLeads(ClubID);
                foreach (var userClub in users)
                {
                    if (userClub.Responsibility == (int) Responsibility.Executor)
                    {
                        //save no executor
                        UserClubLogic.ChangeResponsibility(userClub.Userid.Value, userClub.UserName, ClubID,
                            Responsibility.Member, username);
                    }
                }
            }
            else
            {
                //check each executor existance
                var executors = tbExecutor.Text.Split('|');

                //check club max executor count
                if (executors.Length > ClubLogic.GetClubExecutorQuota(ClubID))
                {
                    //alert
                    var script = "alert('干事数超过限额！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "executor_count_exceed", script, true);

                    LoadPageData();

                    return;
                }

                foreach (var executor in executors)
                {
                    var executorName = executor.Trim();

                    if (executorName == club.ManagerUserName)
                    {
                        //alert
                        var script = "alert('干事不能为该球会会长！');";
                        ClientScript.RegisterClientScriptBlock(typeof (string), "user_not_manager", script, true);

                        LoadPageData();

                        return;
                    }

                    if (Users.GetUserId(executorName) <= 0)
                    {
                        //alert
                        var script = "alert('干事用户在系统中不存在！');";
                        ClientScript.RegisterClientScriptBlock(typeof (string), "user_not_exist", script, true);

                        LoadPageData();

                        return;
                    }

                    var executorID = Users.GetUserId(executorName);

                    if (ClubLogic.GetActiveUserClub(executorID, ClubID) == null)
                    {
                        var script = "alert('干事必须为该球会成员！');";
                        ClientScript.RegisterClientScriptBlock(typeof (string), "user_not_member", script, true);

                        LoadPageData();

                        return;
                    }
                }

                var leaders = ClubLogic.GetClubLeads(ClubID);

                //save executor
                foreach (var executor in executors)
                {
                    var executorName = executor.Trim();
                    var executorID = Users.GetUserId(executorName);

                    if (leaders.Exists(delegate(UserClub uc) { return uc.Userid == executorID; }))
                    {
                        // current executor has been an executor already
                    }
                    else
                        UserClubLogic.ChangeResponsibility(executorID, executorName, ClubID, Responsibility.Executor,
                            username);
                }

                foreach (var leader in leaders)
                {
                    if (leader.Responsibility.Value != (int) Responsibility.Manager)
                    {
                        if (Array.Exists(executors, delegate(string executor) { return executor == leader.UserName; }))
                        {
                            //current leader is in the new leader list
                        }
                        else
                            UserClubLogic.ChangeResponsibility(leader.Userid.Value, leader.UserName, ClubID,
                                Responsibility.Member, username);
                    }
                }
            }

            //update info
            ClubLogic.UpdateClubInfo(ClubID, fuLogo.PostedFile, tbSlogan.Text, tbDesc.Text,
                bool.Parse(rblAppliable.SelectedValue), null);

            var scriptSaved = "alert('信息已保存');";
            ClientScript.RegisterClientScriptBlock(typeof (string), "saved", scriptSaved, true);

            LoadPageData();
        }
    }
}