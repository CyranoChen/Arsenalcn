using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

using Discuz.Forum;
using Discuz.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class FieldToolBar : System.Web.UI.UserControl
    {
        private int userid = -1;
        public int UserID
        {
            set
            {
                userid = value;
            }
        }

        private string username = string.Empty;
        public string UserName
        {
            set
            {
                username = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.userid == -1)
            {
                pnlFuncLink.Visible = false;

                ltrlToolBarTip.Text = "<strong>欢迎进入，请在<a href=\"/login.aspx\" target=\"_self\">登录</a>后使用全部功能</strong>";
            }
            else
            {
                pnlFuncLink.Visible = true;

                List<Club> myClubs = ClubLogic.GetActiveUserClubs(this.userid);

                int leftCount = ConfigGlobal.SingleUserMaxClubCount - myClubs.Count;

                if (leftCount < 0)
                {
                    leftCount = 0;
                }

                ltrlToolBarTip.Text = string.Format("<strong>提醒：您还可以加入<em>{0}</em>个球会</strong>", leftCount.ToString());

                if (ConfigAdmin.IsPluginAdmin(this.userid))
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

                int luckyPlayerID = ConfigGlobal.LuckyPlayerID;

                Player player = PlayerStrip.GetPlayerInfoByPlayerID(luckyPlayerID);
                List<Club> clubs = ClubLogic.GetActiveUserClubs(player.UserID);
                bool IsLuckyPlayerLeader = clubs.Exists(delegate(Club club) { return ClubLogic.GetClubLeads(club.ID.Value).Exists(delegate(UserClub uc) { return uc.Userid == this.userid; }); });

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
            int luckyPlayerID = ConfigGlobal.LuckyPlayerID;

            Player player = PlayerStrip.GetPlayerInfoByPlayerID(luckyPlayerID);
            Player gPlayer = PlayerStrip.GetPlayerInfo(this.userid);

            List<Club> clubs = ClubLogic.GetActiveUserClubs(player.UserID);
            bool IsLuckyPlayerLeader = clubs.Exists(delegate(Club club) { return ClubLogic.GetClubLeads(club.ID.Value).Exists(delegate(UserClub uc) { return uc.Userid == this.userid; }); });

            string script = string.Empty;
            bool CanGetLuckyPlayerBonus = false;

            if ((gPlayer.UserID == player.UserID) || IsLuckyPlayerLeader)
                CanGetLuckyPlayerBonus = true;

            if (player != null && gPlayer != null && !ConfigGlobal.LuckyPlayerBonusGot && CanGetLuckyPlayerBonus)
            {
                int totalBonus = LuckyPlayer.CalcTotalBonus();

                int bonusToUser = (int)(totalBonus * ConfigGlobal.LuckyPlayerBonusPercentage);
                int bonusToClub = totalBonus - bonusToUser;

                UserInfo userInfo = AdminUsers.GetUserInfo(userid);
                userInfo.Extcredits2 += bonusToUser;

                AdminUsers.UpdateUserAllInfo(userInfo);

                //club update
                //List<Club> clubs = ClubLogic.GetActiveUserClubs(userid);
                int clubID = -1;

                if (clubs.Count == 0)
                    bonusToClub = 0;
                else
                {
                    Club club = clubs[0];
                    clubID = club.ID.Value;

                    club.Fortune += bonusToClub;

                    ClubLogic.SaveClub(club);
                }

                LuckyPlayer.SetBonusGot(gPlayer.ID, bonusToClub, clubID, player.ID);

                script = string.Format("alert('您已获得幸运球员奖金{0}枪手币，球会获得{1}枪手币');", bonusToUser, bonusToClub);

                btnGetBonus.Visible = true;
                btnGetBonus.Enabled = false;
                btnGetBonus.Text = "已领取";
            }
            else
            {
                script = "alert('您无法领取今日的幸运球员奖金');";
            }

            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "alert", script, true);
        }
    }
}