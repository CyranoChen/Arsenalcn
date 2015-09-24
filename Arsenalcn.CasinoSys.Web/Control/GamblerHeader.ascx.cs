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
                BtnViewBet.OnClientClick = $"window.location.href='MyBetLog.aspx?userid={UserId}'; return false;";
                BtnViewBonus.OnClientClick = $"window.location.href='MyBonusLog.aspx?userid={UserId}'; return false;";
            }
            else
            {
                BtnBet.Visible = true;
                BtnViewBet.Visible = false;
                BtnViewBonus.Visible = false;
            }

            if (UserId > 0)
            {
                var currentGamlber = new Entity.Gambler(UserId);

                ltrlTotalBet.Text = currentGamlber.TotalBet.ToString("N2");
                ltrlWin.Text = currentGamlber.Win.ToString();
                ltrlLose.Text = currentGamlber.Lose.ToString();
                ltrlEarning.Text = Entity.Bet.GetUserTotalWinCash(UserId).ToString("N2");
                ltrlCash.Text = currentGamlber.Cash.ToString("N2");

                ltrlQSB.Text = Users.GetUserExtCredits(UserId, 2).ToString("N2");
                ltrlRP.Text = Users.GetUserExtCredits(UserId, 4).ToString("f0");

                //UserInfo userInfo = AdminUsers.GetUserInfo(UserID);
                var myAvatar = Avatars.GetAvatarUrl(UserId, AvatarSize.Small);
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
                imgAvatar.AlternateText = UserName;

                hlUserName.Text = UserName;
            }
        }

        public int UserId
        { get; set; }

        public string UserName
        {
            get;
            set;
        }
    }
}