using System;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class GamblerHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["UserID"]))
            {
                BtnBet.Visible = false;
                BtnViewBet.Visible = true;
                BtnViewBonus.Visible = true;
                BtnViewBet.OnClientClick = string.Format("window.location.href='MyBetLog.aspx?userid={0}'; return false;", UserID);
                BtnViewBonus.OnClientClick = string.Format("window.location.href='MyBonusLog.aspx?userid={0}'; return false;", UserID);
            }
            else
            {
                BtnBet.Visible = true;
                BtnViewBet.Visible = false;
                BtnViewBonus.Visible = false;
            }

            if (UserID > 0)
            {
                Entity.Gambler currentGamlber = new Entity.Gambler(UserID);

                ltrlTotalBet.Text = currentGamlber.TotalBet.ToString("N2");
                ltrlWin.Text = currentGamlber.Win.ToString();
                ltrlLose.Text = currentGamlber.Lose.ToString();
                ltrlEarning.Text = Entity.Bet.GetUserTotalWinCash(UserID).ToString("N2");
                ltrlCash.Text = currentGamlber.Cash.ToString("N2");

                ltrlQSB.Text = AdminUsers.GetUserExtCredits(UserID, 2).ToString("N2");
                ltrlRP.Text = AdminUsers.GetUserExtCredits(UserID, 4).ToString("f0");

                //UserInfo userInfo = AdminUsers.GetUserInfo(UserID);
                string myAvatar = Avatars.GetAvatarUrl(UserID, AvatarSize.Small);
                //if (userInfo.Avatar == string.Empty)
                //{
                //    imgAvatar.ImageUrl = "/images/common/noavatar_small.gif";
                //}
                //else if (userInfo.Avatar.ToLower().IndexOf(@"/") == 0)
                //{
                //    imgAvatar.ImageUrl = userInfo.Avatar;
                //}
                //else if (userInfo.Avatar.ToLower().IndexOf("http") >= 0)
                //{
                //    imgAvatar.ImageUrl = userInfo.Avatar;
                //}
                //else
                //{
                //    imgAvatar.ImageUrl = string.Format("/{0}", userInfo.Avatar);
                //}
                imgAvatar.ImageUrl = myAvatar;
                imgAvatar.AlternateText = UserName.ToString();

                hlUserName.Text = UserName.ToString();
            }
        }

        public int UserID
        { get; set; }

        public string UserName
        {
            get;
            set;
        }
    }
}