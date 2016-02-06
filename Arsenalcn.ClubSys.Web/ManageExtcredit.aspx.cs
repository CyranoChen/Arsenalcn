using System;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ManageExtcredit : BasePage
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

        public int ToUserID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ToUID"], out tmp))
                    return tmp;
                Response.Redirect("ClubPortal.aspx");

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);
            var userInfo = Users.GetUserInfo(ToUserID);

            if (club != null && userInfo != null && Title.IndexOf("{0}") >= 0 && Title.IndexOf("{1}") >= 0)
                Title = string.Format(Title, club.FullName, userInfo.Username.Trim());

            rvMaxCash.MaximumValue = "1";

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlManageMenuTabBar.CurrentMenu = ManageClubMenuItem.ManageMember;
            ctrlManageMenuTabBar.UserID = userid;

            #endregion

            if (!IsPostBack)
            {
                LoadPageData();
            }
        }

        protected void LoadPageData()
        {
            //UserClub ucFrom = ClubLogic.GetActiveUserClubs(this.userid);
            //UserClub ucTo = ClubLogic.GetActiveUserClub(ToUserID, ClubID);

            var userFrom = Users.GetUserInfo(userid);
            var userTo = Users.GetUserInfo(ToUserID);

            if (userid != ToUserID)
            {
                var list = ClubLogic.GetUserManagedClubs(userid);

                if (list != null && list.Count > 0)
                {
                    pnlInaccessible.Visible = false;
                    phContent.Visible = true;

                    var club = ClubLogic.GetClubInfo(ClubID);
                    lblTransferInfo.Text = string.Format("<em>{1}</em>转账给会员<em>{0} {2}</em>，您现拥有枪手币<em>{3}</em>",
                        club.FullName, userFrom.Username.Trim(), userTo.Username.Trim(),
                        userFrom.Extcredits2.ToString("N2"));

                    ltrlFromUserInfo.Text =
                        $"<em>{userFrom.Username.Trim()}</em>(金钱:{userFrom.Extcredits2.ToString("N2")} | RP:{userFrom.Extcredits4})";
                    ltrlToUserInfo.Text =
                        $"<em>{userTo.Username.Trim()}</em>(金钱:{userTo.Extcredits2.ToString("N2")} | RP:{userTo.Extcredits4})";

                    lblMaxTransfer.Text = $" *最多为可转账<em>{(userFrom.Extcredits2*0.5f).ToString("N0")}</em>枪手币 ";
                    rvMaxCash.MaximumValue = Convert.ToInt32(userFrom.Extcredits2*0.5f).ToString();
                }
                else
                {
                    pnlInaccessible.Visible = true;
                    lblTips.Text = "您没有转帐权限。";
                    phContent.Visible = false;
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
                var cash = 0f;

                var user = Users.GetUserInfo(userid);
                var maxCash = user.Extcredits2*0.5f;

                if (float.TryParse(tbCash.Text.Trim(), out cash) && maxCash >= 1f)
                {
                    if (cash >= 1 && cash <= maxCash)
                        UserClubLogic.TransferMemberExtcredit(ClubID, userid, ToUserID, cash, 2);
                    else
                        throw new Exception("转账金额超过上下限");
                }
                else
                {
                    throw new Exception("转账金额无效");
                }

                ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                    "alert('转账成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('转账失败');", true);
            }

            LoadPageData();
        }
    }
}