using System;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using Discuz.Forum;
using Discuz.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyCreateClub : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            #endregion

            int minPosts = ConfigGlobal.MinPostsToCreateClub;
            ltrlMinPosts.Text = minPosts.ToString();
            ltrlMinPosts1.Text = minPosts.ToString();

            if (!IsPostBack)
            {
                //check if user can create a club
                if (ClubLogic.GetActiveUserClubs(this.userid).Count >= ConfigGlobal.SingleUserMaxClubCount)
                {
                    //hide main content
                    pnlInaccessible.Visible = true;
                    phContent.Visible = false;
                }
                else
                {
                    ShortUserInfo userInfo = Users.GetShortUserInfo(userid);

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
                        Club club = ClubLogic.GetCreateClubApplicationByUserID(this.userid);

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
                            tbManagerName.Text = this.username;
                            btnCancel.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Club club = ClubLogic.GetCreateClubApplicationByUserID(this.userid);

            Club sameNameClub = ClubLogic.GetClubInfo(tbFullName.Text);

            if (club != null)
            {
                if (club.FullName == tbFullName.Text)
                    ClubLogic.UpdateApplyClub(club.ID.Value, tbFullName.Text, tbShortName.Text, tbSlogan.Text, tbDesc.Text, this.userid, this.username);
                else
                {
                    if (sameNameClub != null)
                    {
                        string script = "alert('该球会名已被使用！');";
                        this.ClientScript.RegisterClientScriptBlock(typeof(string), "name_used", script, true);
                    }
                    else
                        ClubLogic.UpdateApplyClub(club.ID.Value, tbFullName.Text, tbShortName.Text, tbSlogan.Text, tbDesc.Text, this.userid, this.username);
                }
            }
            else
            {
                if (sameNameClub != null)
                {
                    string script = "alert('该球会名已被使用！');";
                    this.ClientScript.RegisterClientScriptBlock(typeof(string), "name_used", script, true);
                }
                else
                    ClubLogic.ApplyClub(tbFullName.Text, tbShortName.Text, tbSlogan.Text, tbDesc.Text, this.userid, this.username);
            }

            string scriptSaved = "alert('申请已提交！'); window.location.href = window.location.href;";
            this.ClientScript.RegisterClientScriptBlock(typeof(string), "saved", scriptSaved, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Club club = ClubLogic.GetCreateClubApplicationByUserID(this.userid);

            if( club != null )
                ClubLogic.ApproveClub(club.ID.Value, false);

            string script = "alert('申请已取消！');";
            this.ClientScript.RegisterClientScriptBlock(typeof(string), "canceled", script, true);
        }
    }
}
