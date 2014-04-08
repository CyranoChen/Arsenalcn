using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageClub : Common.BasePage
    {
        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            if (!IsPostBack)
            {
                LoadPageData();
            }

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlManageMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ManageClubMenuItem.ManageClub;
            ctrlManageMenuTabBar.UserID = this.userid;

            #endregion

        }

        protected void LoadPageData()
        {
            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null)
            {
                // Current User must be the manager of this club or Administrator
                if (club.ManagerUid.Value.Equals(this.userid) || ConfigAdmin.IsPluginAdmin(this.userid))
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

                        List<UserClub> users = ClubLogic.GetClubLeads(ClubID);
                        System.Text.StringBuilder sbEx = new System.Text.StringBuilder();
                        foreach (UserClub user in users)
                        {
                            if (user.Responsibility.Value == (int)Responsibility.Executor)
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
                        ltrlLogo.Text = string.Format("<a href=\"{0}\" target=\"_blank\" title=\"点击放大\"><img src=\"{0}\" width=\"24\" height=\"24\" alt=\"点击放大\" /></a>", "UploadFiles/" + club.LogoName);
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
                string logoName = fuLogo.FileName;

                if (logoName.ToLower().LastIndexOf(".gif") != logoName.Length - 4 && logoName.ToLower().LastIndexOf(".jpg") != logoName.Length - 4 && logoName.ToLower().LastIndexOf(".png") != logoName.Length - 4)
                {
                    //invalid logo file
                    string invalidAlert = "alert('请上传扩展名为gif，jpg或png的文件！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "invalid_logo_file", invalidAlert, true);

                    LoadPageData();

                    return;
                }

                if (fuLogo.FileBytes.LongLength > 100 * 1024)
                {
                    string fileLengthAlert = "alert('请上传小于100K的文件！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "file_too_large", fileLengthAlert, true);

                    LoadPageData();

                    return;
                }
            }

            //validate assignment
            Club club = ClubLogic.GetClubInfo(ClubID);

            //check manager
            if (tbManager.Text != club.ManagerUserName)
            {
                //check new manager existance in discuz
                if (Users.GetUserId(tbManager.Text) <= 0)
                {
                    //alert
                    string script = "alert('会长用户在系统中不存在！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "user_not_exist", script, true);

                    LoadPageData();

                    return;
                }

                int managerID = Users.GetUserId(tbManager.Text);
                //check if the new manager is a member of the club, if not, alert
                if (ClubLogic.GetActiveUserClub(managerID, ClubID) == null)
                {
                    string script = "alert('新会长必须为该球会成员！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "user_not_member", script, true);

                    LoadPageData();

                    return;
                }
            }

            //check executor
            if (tbExecutor.Text == string.Empty)
            {
                //change current executor to normal member
                List<UserClub> users = ClubLogic.GetClubLeads(ClubID);
                foreach (UserClub userClub in users)
                {
                    if (userClub.Responsibility == (int)Responsibility.Executor)
                    {
                        //save no executor
                        UserClubLogic.ChangeResponsibility(userClub.Userid.Value, userClub.UserName, ClubID, Responsibility.Member, this.username);
                    }
                }
            }
            else
            {
                //check each executor existance
                string[] executors = tbExecutor.Text.Split('|');

                //check club max executor count
                if (executors.Length > ClubLogic.GetClubExecutorQuota(ClubID))
                {
                    //alert
                    string script = "alert('干事数超过限额！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "executor_count_exceed", script, true);

                    LoadPageData();

                    return;
                }

                foreach (string executor in executors)
                {
                    string executorName = executor.Trim();

                    if (executorName == club.ManagerUserName)
                    {
                        //alert
                        string script = "alert('干事不能为该球会会长！');";
                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "user_not_manager", script, true);

                        LoadPageData();

                        return;
                    }

                    if (Users.GetUserId(executorName) <= 0)
                    {
                        //alert
                        string script = "alert('干事用户在系统中不存在！');";
                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "user_not_exist", script, true);

                        LoadPageData();

                        return;
                    }

                    int executorID = Users.GetUserId(executorName);

                    if (ClubLogic.GetActiveUserClub(executorID, ClubID) == null)
                    {
                        string script = "alert('干事必须为该球会成员！');";
                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "user_not_member", script, true);

                        LoadPageData();

                        return;
                    }
                }

                List<UserClub> leaders = ClubLogic.GetClubLeads(ClubID);

                //save executor
                foreach (string executor in executors)
                {
                    string executorName = executor.Trim();
                    int executorID = Users.GetUserId(executorName);

                    if (leaders.Exists(delegate(UserClub uc) { return uc.Userid == executorID; }))
                    {
                        // current executor has been an executor already
                        continue;
                    }
                    else
                        UserClubLogic.ChangeResponsibility(executorID, executorName, ClubID, Responsibility.Executor, this.username);
                }

                foreach (UserClub leader in leaders)
                {
                    if (leader.Responsibility.Value != (int)Responsibility.Manager)
                    {
                        if (Array.Exists(executors, delegate(string executor) { return executor == leader.UserName; }))
                        {
                            //current leader is in the new leader list
                            continue;
                        }
                        else
                            UserClubLogic.ChangeResponsibility(leader.Userid.Value, leader.UserName, ClubID, Responsibility.Member, this.username);
                    }
                }
            }

            //update info
            ClubLogic.UpdateClubInfo(ClubID, fuLogo.PostedFile, tbSlogan.Text, tbDesc.Text, Boolean.Parse(rblAppliable.SelectedValue), null);

            string scriptSaved = "alert('信息已保存');";
            this.ClientScript.RegisterClientScriptBlock(typeof(string), "saved", scriptSaved, true);

            LoadPageData();
        }
    }
}
