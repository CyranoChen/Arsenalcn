using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyCreateClub : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            #endregion

            var minPosts = ConfigGlobal.MinPostsToCreateClub;
            ltrlMinPosts.Text = minPosts.ToString();
            ltrlMinPosts1.Text = minPosts.ToString();

            if (!IsPostBack)
            {
                //check if user can create a club
                if (ClubLogic.GetActiveUserClubs(userid).Count >= ConfigGlobal.SingleUserMaxClubCount)
                {
                    //hide main content
                    pnlInaccessible.Visible = true;
                    phContent.Visible = false;
                }
                else
                {
                    var userInfo = Users.GetShortUserInfo(userid);

                    if (userInfo.Posts < minPosts)
                    {
                        //hide main content
                        pnlInaccessible.Visible = true;
                        phContent.Visible = false;
                    }
                    else
                    {
                        pnlInaccessible.Visible = false;
                        phContent.Visible = true;

                        //check if user has an application under approve
                        var club = ClubLogic.GetCreateClubApplicationByUserID(userid);

                        if (club != null)
                        {
                            tbFullName.Text = club.FullName;
                            tbShortName.Text = club.ShortName;
                            tbManagerName.Text = club.ManagerUserName;
                            tbSlogan.Text = club.Slogan;
                            tbDesc.Text = club.Description;
                        }
                        else
                        {
                            tbManagerName.Text = username;
                            btnCancel.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var club = ClubLogic.GetCreateClubApplicationByUserID(userid);

            var sameNameClub = ClubLogic.GetClubInfo(tbFullName.Text);

            if (club != null)
            {
                if (club.FullName == tbFullName.Text)
                    ClubLogic.UpdateApplyClub(club.ID.Value, tbFullName.Text, tbShortName.Text, tbSlogan.Text,
                        tbDesc.Text, userid, username);
                else
                {
                    if (sameNameClub != null)
                    {
                        var script = "alert('该球会名已被使用！');";
                        ClientScript.RegisterClientScriptBlock(typeof (string), "name_used", script, true);
                    }
                    else
                        ClubLogic.UpdateApplyClub(club.ID.Value, tbFullName.Text, tbShortName.Text, tbSlogan.Text,
                            tbDesc.Text, userid, username);
                }
            }
            else
            {
                if (sameNameClub != null)
                {
                    var script = "alert('该球会名已被使用！');";
                    ClientScript.RegisterClientScriptBlock(typeof (string), "name_used", script, true);
                }
                else
                    ClubLogic.ApplyClub(tbFullName.Text, tbShortName.Text, tbSlogan.Text, tbDesc.Text, userid, username);
            }

            var scriptSaved = "alert('申请已提交！'); window.location.href = window.location.href;";
            ClientScript.RegisterClientScriptBlock(typeof (string), "saved", scriptSaved, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var club = ClubLogic.GetCreateClubApplicationByUserID(userid);

            if (club != null)
                ClubLogic.ApproveClub(club.ID.Value, false);

            var script = "alert('申请已取消！');";
            ClientScript.RegisterClientScriptBlock(typeof (string), "canceled", script, true);
        }
    }
}