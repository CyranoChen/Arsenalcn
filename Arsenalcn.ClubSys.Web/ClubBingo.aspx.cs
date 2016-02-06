using System;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubGetStrip : BasePage
    {
        public int ClubID { get; private set; } = -1;

        public string UserName
        {
            get { return username; }
        }

        public int UserID
        {
            get { return userid; }
        }

        public int ProfileUserID
        {
            get { return userid; }
        }

        public string DisplaySwf { get; set; }

        public string IsGoogleAdv { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var clubs = ClubLogic.GetActiveUserClubs(userid);

            if (clubs.Count == 0)
            {
                //user without a club joined can not access this page
                var script = "alert('您尚未加入一个球会！'); window.location.href = 'ClubPortal.aspx';";

                ClientScript.RegisterClientScriptBlock(typeof (string), "redirect", script, true);
            }
            else
            {
                ClubID = clubs[0].ID.Value;
            }

            if (ClubID > 0)
            {
                if (PlayerStrip.GetClubRemainingEquipment(ClubID) <= 0)
                {
                    //ctrlGoogleAdv.DisplayAdv = "none";
                    cbGoogleAdvActive.Visible = false;
                    pnlShowGetStrip.Visible = false;
                    lblGetStripUserInfo.Visible = false;
                    lblGetStripNotAvailable.Visible = true;
                    lblGetStripNotAvailable.Text =
                        $"<em>今天本球会的装备领取已到上限。({ConfigGlobal.DailyClubEquipmentCount})</em>";
                }
                else if (PlayerStrip.GetUserBingoGainCountToday(userid) >= ConfigGlobal.DailyUserEquipmentCount)
                {
                    //ctrlGoogleAdv.DisplayAdv = "none";
                    cbGoogleAdvActive.Visible = false;
                    pnlShowGetStrip.Visible = false;
                    lblGetStripUserInfo.Visible = false;
                    lblGetStripNotAvailable.Visible = true;
                    lblGetStripNotAvailable.Text =
                        $"<em>您今天的装备领取已到上限。({ConfigGlobal.DailyUserEquipmentCount})</em>";
                }
                else
                {
                    pnlShowGetStrip.Visible = true;
                    lblGetStripUserInfo.Visible = true;
                    lblGetStripNotAvailable.Visible = false;

                    if (IsPostBack)
                        PlayerStrip.UpdatePlayerGoogleAdvActive(UserID, cbGoogleAdvActive.Checked);

                    BindGetStrip();
                }

                var club = ClubLogic.GetClubInfo(ClubID);

                if (club != null && Title.IndexOf("{0}") >= 0)
                    Title = string.Format(Title, club.FullName);

                #region SetControlProperty

                ctrlLeftPanel.UserID = userid;
                ctrlLeftPanel.UserName = username;
                ctrlLeftPanel.UserKey = userkey;

                ctrlFieldToolBar.UserID = userid;
                ctrlFieldToolBar.UserName = username;

                //ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubStrip;
                //ctrlMenuTabBar.ClubID = _clubID;

                ctrlPlayerHeader.UserID = userid;
                ctrlPlayerHeader.ProfileUserID = ProfileUserID;

                #endregion
            }
        }

        private void BindGetStrip()
        {
            var player = PlayerStrip.GetPlayerInfo(UserID);

            if (ConfigGlobal.GoogleAdvActive && player != null)
            {
                cbGoogleAdvActive.Visible = true;
                cbGoogleAdvActive.Checked = player.IsActive;
            }
            else
                cbGoogleAdvActive.Visible = false;

            var totalCount = PlayerStrip.GetUserBingoPlayCount(UserID);
            var getStripRP = 0;

            if (totalCount > 0)
                getStripRP = Convert.ToInt16(PlayerStrip.GetUserBingoGainCount(userid)*100/totalCount);

            lblGetStripUserInfo.Text =
                $"<em>{UserName.Trim()}</em>今日获得/尝试:<em>{PlayerStrip.GetUserBingoGainCountToday(UserID)}({PlayerStrip.GetUserBingoPlayCountToday(UserID)})</em> | 获得率:<em>{getStripRP}%</em> | 库存:<em>{PlayerStrip.GetClubRemainingEquipment(ClubID)}/{ConfigGlobal.DailyClubEquipmentCount}</em>";

            if (ConfigGlobal.GoogleAdvActive && player != null && player.IsActive)
            {
                //ctrlGoogleAdv.DisplayAdv = string.Empty;
                DisplaySwf = "none";
                IsGoogleAdv = "true";
                lblGetStripUserInfo.Text += " | <em title=\"抽取与获取装备均免费\">打工模式</em>";
            }
            else
            {
                //ctrlGoogleAdv.DisplayAdv = "none";
                DisplaySwf = string.Empty;
                IsGoogleAdv = "false";
                lblGetStripUserInfo.Text +=
                    $" | 每次抽取:<em title=\"枪手币\">{ConfigGlobal.BingoCost}</em> | 每件获得:<em title=\"枪手币\">{ConfigGlobal.BingoGetCost}</em>";
            }
        }
    }
}