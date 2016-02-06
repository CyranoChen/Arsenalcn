using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.Common.Entity;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class FieldToolBar : UserControl
    {
        private int userid = -1;

        private string username = string.Empty;

        public int UserID
        {
            set { userid = value; }
        }

        public string UserName
        {
            set { username = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userid == -1)
            {
                pnlFuncLink.Visible = false;

                ltrlToolBarTip.Text = "<strong>欢迎进入，请在<a href=\"/login.aspx\" target=\"_self\">登录</a>后使用全部功能</strong>";
            }
            else
            {
                pnlFuncLink.Visible = true;

                var myClubs = ClubLogic.GetActiveUserClubs(userid);

                var leftCount = ConfigGlobal.SingleUserMaxClubCount - myClubs.Count;

                if (leftCount < 0)
                {
                    leftCount = 0;
                }

                ltrlToolBarTip.Text = $"<strong>提醒：您还可以加入<em>{leftCount}</em>个球会</strong>";

                if (ConfigAdmin.IsPluginAdmin(userid))
                {
                    phAdministrator.Visible = true;
                }
                else
                {
                    phAdministrator.Visible = false;
                }
            }

            //lucky player

            if (ConfigGlobal.LuckyPlayerActive)
            {
                phLuckPlayer.Visible = true;

                ltrlBonus.Text = LuckyPlayer.CalcTotalBonus().ToString();

                var luckyPlayerID = ConfigGlobal.LuckyPlayerID;

                var player = PlayerStrip.GetPlayerInfoByPlayerID(luckyPlayerID);
                var clubs = ClubLogic.GetActiveUserClubs(player.UserID);
                var IsLuckyPlayerLeader =
                    clubs.Exists(
                        delegate(Club club)
                        {
                            return
                                ClubLogic.GetClubLeads(club.ID.Value)
                                    .Exists(delegate(UserClub uc) { return uc.Userid == this.userid; });
                        });

                if (DateTime.Now.Hour < ConfigGlobal.LuckyPlayerDeadline)
                {
                    IsLuckyPlayerLeader = false;
                }

                if (player != null)
                {
                    ltrlLuckyPlayerName.Text = player.UserName;

                    if ((player.UserID != userid && !IsLuckyPlayerLeader) || (userid == -1))
                    {
                        if (ConfigGlobal.LuckyPlayerBonusGot)
                        {
                            btnGetBonus.Visible = true;
                            btnGetBonus.Enabled = false;
                            btnGetBonus.Text = "已领取";
                        }
                        else
                            btnGetBonus.Visible = false;
                    }
                    else if (!ConfigGlobal.LuckyPlayerBonusGot)
                    {
                        btnGetBonus.Visible = true;
                        if ((player.UserID != userid) && IsLuckyPlayerLeader)
                        {
                            btnGetBonus.Text = "请代领";
                        }
                    }
                    else
                    {
                        btnGetBonus.Visible = true;
                        btnGetBonus.Enabled = false;
                        btnGetBonus.Text = "已领取";
                    }
                }
                else
                {
                    btnGetBonus.Visible = false;
                }
            }
            else
                phLuckPlayer.Visible = false;
        }

        protected void btnGetBonus_Click(object sender, EventArgs e)
        {
            var luckyPlayerID = ConfigGlobal.LuckyPlayerID;

            var player = PlayerStrip.GetPlayerInfoByPlayerID(luckyPlayerID);
            var gPlayer = PlayerStrip.GetPlayerInfo(userid);

            var clubs = ClubLogic.GetActiveUserClubs(player.UserID);
            var isLuckyPlayerLeader =
                clubs.Exists(
                    delegate(Club club)
                    {
                        return
                            ClubLogic.GetClubLeads(club.ID.Value)
                                .Exists(delegate(UserClub uc) { return uc.Userid == this.userid; });
                    });

            var script = string.Empty;
            var CanGetLuckyPlayerBonus = false;

            if ((gPlayer.UserID == player.UserID) || isLuckyPlayerLeader)
                CanGetLuckyPlayerBonus = true;

            if (player != null && gPlayer != null && !ConfigGlobal.LuckyPlayerBonusGot && CanGetLuckyPlayerBonus)
            {
                var totalBonus = LuckyPlayer.CalcTotalBonus();

                var bonusToUser = (int) (totalBonus*ConfigGlobal.LuckyPlayerBonusPercentage);
                var bonusToClub = totalBonus - bonusToUser;

                var userInfo = Users.GetUserInfo(userid);
                userInfo.Extcredits2 += bonusToUser;

                AdminUsers.UpdateUserAllInfo(userInfo);

                //club update
                //List<Club> clubs = ClubLogic.GetActiveUserClubs(userid);
                var clubID = -1;

                if (clubs.Count == 0)
                    bonusToClub = 0;
                else
                {
                    var club = clubs[0];
                    clubID = club.ID.Value;

                    club.Fortune += bonusToClub;

                    ClubLogic.SaveClub(club);
                }

                LuckyPlayer.SetBonusGot(gPlayer.ID, bonusToClub, clubID, player.ID);

                Config.Cache.RefreshCache();

                script = $"alert('您已获得幸运球员奖金{bonusToUser}枪手币，球会获得{bonusToClub}枪手币');";

                btnGetBonus.Visible = true;
                btnGetBonus.Enabled = false;
                btnGetBonus.Text = "已领取";
            }
            else
            {
                script = "alert('您无法领取今日的幸运球员奖金');";
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof (string), "alert", script, true);
        }
    }
}