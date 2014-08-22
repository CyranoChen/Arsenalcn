using System;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using Discuz.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageExtcredit : Common.BasePage
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

        public int ToUserID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ToUID"], out tmp))
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
            UserInfo userInfo = AdminUsers.GetUserInfo(ToUserID);

            if (club != null && userInfo != null && this.Title.IndexOf("{0}") >= 0 && this.Title.IndexOf("{1}") >= 0)
                this.Title = string.Format(this.Title, club.FullName.ToString(), userInfo.Username.ToString().Trim());

            rvMaxCash.MaximumValue = "1";

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlManageMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ManageClubMenuItem.ManageMember;
            ctrlManageMenuTabBar.UserID = this.userid;

            #endregion

            if (!IsPostBack)
            {
                LoadPageData();
            }
        }

        protected void LoadPageData()
        {
            UserClub ucFrom = ClubLogic.GetActiveUserClub(this.userid, ClubID);
            //UserClub ucTo = ClubLogic.GetActiveUserClub(ToUserID, ClubID);
            UserInfo userFrom = AdminUsers.GetUserInfo(this.userid);
            UserInfo userTo = AdminUsers.GetUserInfo(ToUserID);

            if (ucFrom != null && this.userid != ToUserID)
            {
                if (ucFrom.Responsibility != (int)Responsibility.Executor && ucFrom.Responsibility != (int)Responsibility.Manager)
                {
                    pnlInaccessible.Visible = true;
                    phContent.Visible = false;
                }
                else
                {
                    pnlInaccessible.Visible = false;
                    phContent.Visible = true;

                    Club club = ClubLogic.GetClubInfo(ClubID);
                    lblTransferInfo.Text = string.Format("<em>{0} {1}({2})</em>转账给会员<em>{3}</em>，您现拥有枪手币<em>{4}</em>", club.FullName, ucFrom.UserName.Trim(), ClubLogic.TranslateResponsibility(ucFrom.Responsibility.Value), userTo.Username.Trim(), userFrom.Extcredits2.ToString("N2"));

                    ltrlFromUserInfo.Text = string.Format("<em>{0}</em>(金钱:{1} | RP:{2})", userFrom.Username.Trim(), userFrom.Extcredits2.ToString("N2"), userFrom.Extcredits4.ToString());
                    ltrlToUserInfo.Text = string.Format("<em>{0}</em>(金钱:{1} | RP:{2})", userTo.Username.Trim(), userTo.Extcredits2.ToString("N2"), userTo.Extcredits4.ToString());

                    lblMaxTransfer.Text = string.Format(" *最多为可转账<em>{0}</em>枪手币 ", (userFrom.Extcredits2 * 0.5f).ToString("N0"));
                    rvMaxCash.MaximumValue = Convert.ToInt32(userFrom.Extcredits2 * 0.5f).ToString();
                }
            }
            else
            {
                pnlInaccessible.Visible = true;
                lblTips.Text = "您不能对自己转账。";
                phContent.Visible = false;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                float cash = 0f;

                UserInfo user = AdminUsers.GetUserInfo(this.userid);
                float maxCash = user.Extcredits2 * 0.5f;

                if (float.TryParse(tbCash.Text.Trim(), out cash) && maxCash >= 1f)
                {
                    if (cash >= 1 && cash <= maxCash)
                        UserClubLogic.TransferMemberExtcredit(ClubID, this.userid, ToUserID, cash, 2);
                    else
                        throw new Exception("转账金额超过上下限");
                }
                else
                {
                    throw new Exception("转账金额无效");
                }

                this.ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('转账成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('转账失败');", true);
            }

            LoadPageData();
        }
    }
}
