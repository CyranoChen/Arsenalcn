using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubGetStrip : Common.BasePage
    {
        private int _clubID = -1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AnonymousRedirect = true;
        }

        public int ClubID
        {
            get
            {
                return _clubID;
            }
        }

        public string UserName
        {
            get
            {
                return this.username;
            }
        }

        public int UserID
        {
            get
            {
                return this.userid;
            }
        }

        public int ProfileUserID
        {
            get
            {
                return this.userid;
            }
        }

        public string DisplaySwf
        {
            get;
            set;
        }

        public string IsGoogleAdv
        {
            get;
            set;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Club> clubs = ClubLogic.GetActiveUserClubs(userid);

            if (clubs.Count == 0)
            {
                //user without a club joined can not access this page
                string script = "alert('您尚未加入一个球会！'); window.location.href = 'ClubPortal.aspx';";

                this.ClientScript.RegisterClientScriptBlock(typeof(string), "redirect", script, true);
            }
            else
            {
                _clubID = clubs[0].ID.Value;
            }

            if (_clubID > 0)
            {
                if (PlayerStrip.GetClubRemainingEquipment(_clubID) <= 0)
                {
                    //ctrlGoogleAdv.DisplayAdv = "none";
                    cbGoogleAdvActive.Visible = false;
                    pnlShowGetStrip.Visible = false;
                    lblGetStripUserInfo.Visible = false;
                    lblGetStripNotAvailable.Visible = true;
                    lblGetStripNotAvailable.Text = string.Format("<em>今天本球会的装备领取已到上限。({0})</em>", ConfigGlobal.DailyClubEquipmentCount.ToString());
                }
                else if (PlayerStrip.GetUserBingoGainCountToday(userid) >= ConfigGlobal.DailyUserEquipmentCount)
                {
                    //ctrlGoogleAdv.DisplayAdv = "none";
                    cbGoogleAdvActive.Visible = false;
                    pnlShowGetStrip.Visible = false;
                    lblGetStripUserInfo.Visible = false;
                    lblGetStripNotAvailable.Visible = true;
                    lblGetStripNotAvailable.Text = string.Format("<em>您今天的装备领取已到上限。({0})</em>", ConfigGlobal.DailyUserEquipmentCount.ToString());
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

                Club club = ClubLogic.GetClubInfo(_clubID);

                if (club != null && this.Title.IndexOf("{0}") >= 0)
                    this.Title = string.Format(this.Title, club.FullName);

                #region SetControlProperty

                ctrlLeftPanel.UserID = this.userid;
                ctrlLeftPanel.UserName = this.username;
                ctrlLeftPanel.UserKey = this.userkey;

                ctrlFieldToolBar.UserID = this.userid;
                ctrlFieldToolBar.UserName = this.username;

                //ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubStrip;
                //ctrlMenuTabBar.ClubID = _clubID;

                ctrlPlayerHeader.UserID = this.userid;
                ctrlPlayerHeader.ProfileUserID = this.ProfileUserID;

                #endregion
            }
        }

        private void BindGetStrip()
        {
            Gamer player = PlayerStrip.GetPlayerInfo(UserID);

            if (ConfigGlobal.GoogleAdvActive && player != null)
            {
                cbGoogleAdvActive.Visible = true;
                cbGoogleAdvActive.Checked = player.IsActive;
            }
            else
                cbGoogleAdvActive.Visible = false;

            int totalCount = PlayerStrip.GetUserBingoPlayCount(UserID);
            int getStripRP = 0;

            if (totalCount > 0)
                getStripRP = Convert.ToInt16(PlayerStrip.GetUserBingoGainCount(userid) * 100 / totalCount);

            lblGetStripUserInfo.Text = string.Format("<em>{0}</em>今日获得/尝试:<em>{1}({2})</em> | 获得率:<em>{3}%</em> | 库存:<em>{4}/{5}</em>", UserName.Trim(), PlayerStrip.GetUserBingoGainCountToday(UserID).ToString(), PlayerStrip.GetUserBingoPlayCountToday(UserID).ToString(), getStripRP.ToString(), PlayerStrip.GetClubRemainingEquipment(ClubID).ToString(), ConfigGlobal.DailyClubEquipmentCount.ToString());

            if (ConfigGlobal.GoogleAdvActive && player != null && player.IsActive)
            {
                //ctrlGoogleAdv.DisplayAdv = string.Empty;
                DisplaySwf = "none";
                IsGoogleAdv = "true";
                lblGetStripUserInfo.Text += string.Format(" | <em title=\"抽取与获取装备均免费\">打工模式</em>");
            }
            else
            {
                //ctrlGoogleAdv.DisplayAdv = "none";
                DisplaySwf = string.Empty;
                IsGoogleAdv = "false";
                lblGetStripUserInfo.Text += string.Format(" | 每次抽取:<em title=\"枪手币\">{0}</em> | 每件获得:<em title=\"枪手币\">{1}</em>", ConfigGlobal.BingoCost.ToString(), ConfigGlobal.BingoGetCost.ToString());
            }
        }
    }
}
