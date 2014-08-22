using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using ArsenalPlayer = Arsenal.Entity.Player;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubPlayer : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Club club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubPlayer;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = this.userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = this.username;

            #endregion

            ClubPlayerLvCount = new Dictionary<int, int>();

            for (int i = 0; i <= (ConfigGlobal.PlayerMaxLv + 1); i++)
            {
                ClubPlayerLvCount[i] = 0;
            }
            BindPlayers();

            this.ltlPlayerCount.Text = string.Format("<span>本球会正式/总球员数:<em>{0}/{1}</em></span>", FormalPlayerCount.ToString(), PlayerStrip.GetClubPlayerCount(ClubID).ToString());
            this.ltlPlayerLv.Text = string.Format("<span>&gt;LV5:{0}</span>", ClubPlayerLvCount[ConfigGlobal.PlayerMaxLv + 1].ToString());

            for (int j = ConfigGlobal.PlayerMaxLv; j > 0; j--)
            {
                this.ltlPlayerLv.Text += string.Format(" <span class=\"ClubSys_PlayerLV{0}\">LV{0}:{1}</span>", j.ToString(), ClubPlayerLvCount[j].ToString());
            }

        }

        public Dictionary<int, int> ClubPlayerLvCount;
        public int FormalPlayerCount = 0;

        private List<Player> _list;
        private void BindPlayers()
        {
            if (_list == null)
            {

                _list = PlayerStrip.GetClubPlayers(ClubID);

                foreach (Player player in _list)
                {
                    if (player.CurrentGuid != null)
                        FormalPlayerCount++;

                    int playerLv = player.Shirt;

                    if (player.Shorts < playerLv)
                        playerLv = player.Shorts;

                    if (player.Sock < playerLv)
                        playerLv = player.Sock;

                    if (playerLv <= ConfigGlobal.PlayerMaxLv)
                    {
                        player.AdditionalData2 = playerLv;
                        player.AdditionalData = playerLv * 20;
                    }
                    else
                    {
                        player.AdditionalData2 = ConfigGlobal.PlayerMaxLv;
                        player.AdditionalData = ConfigGlobal.PlayerMaxLv * 20;
                        playerLv = ConfigGlobal.PlayerMaxLv + 1;
                    }

                    if (ClubPlayerLvCount.ContainsKey(playerLv))
                        ClubPlayerLvCount[playerLv]++;
                    else
                        ClubPlayerLvCount[playerLv] = 0;
                }
            }

            _list.Sort(new PlayerComparer());

            gvPlayers.DataSource = _list;
            gvPlayers.DataBind();
        }

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

        protected void gvPlayers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayers.PageIndex = e.NewPageIndex;

            BindPlayers();
        }

        protected void gvPlayers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Player player = e.Row.DataItem as Player;

                Literal ltrlNum = e.Row.FindControl("ltrlNum") as Literal;
                Literal ltrlShirtLV = e.Row.FindControl("ltrlShirtLV") as Literal;
                Literal ltrlShortsLV = e.Row.FindControl("ltrlShortsLV") as Literal;
                Literal ltrlSockLV = e.Row.FindControl("ltrlSockLV") as Literal;

                if (player.CurrentGuid != null)
                {
                    String StrSwfContent = "<div class=\"ClubSys_PlayerCardPH\">";
                    StrSwfContent += string.Format("<script type=\"text/javascript\">GenSwfObject('PlayerCardActive{0}','swf/PlayerCardActive.swf?XMLURL=ServerXml.aspx%3FPlayerGuid={0}','80','100');</script>", player.CurrentGuid.Value.ToString());
                    StrSwfContent += "</div>";
                    ltrlNum.Text = StrSwfContent;
                }
                else
                {
                    ltrlNum.Text = "<img src=\"uploadfiles/PlayerLogo.gif\" alt=\"NoCurrentPlayer\" title=\"NoCurrentPlayer\" height=\"50\" />";
                }

                int _shirtLV = Math.Min(player.Shirt, ConfigGlobal.PlayerMaxLv);
                int _shortsLV = Math.Min(player.Shorts, ConfigGlobal.PlayerMaxLv);
                int _sockLV = Math.Min(player.Sock, ConfigGlobal.PlayerMaxLv);

                ltrlShirtLV.Text = string.Format("<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/shirt{1}.gif\" alt=\"LV{0}\" /></div>", player.Shirt.ToString(), _shirtLV.ToString());
                ltrlShortsLV.Text = string.Format("<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/shorts{1}.gif\" alt=\"LV{0}\" /></div>", player.Shorts.ToString(), _shortsLV.ToString());
                ltrlSockLV.Text = string.Format("<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/sock{1}.gif\" alt=\"LV{0}\" /></div>", player.Sock.ToString(), _sockLV.ToString());
            }
        }

        private class PlayerComparer : IComparer<Player>
        {

            #region IComparer<Player> Members

            public int Compare(Player x, Player y)
            {
                if (!string.IsNullOrEmpty(x.CurrentGuid.ToString()) && string.IsNullOrEmpty(y.CurrentGuid.ToString()))
                    return -1;
                else if (string.IsNullOrEmpty(x.CurrentGuid.ToString()) == string.IsNullOrEmpty(y.CurrentGuid.ToString()))
                {
                    int xLv = (int)x.AdditionalData2;
                    int yLv = (int)y.AdditionalData2;

                    return yLv - xLv;
                }
                else
                    return 0;
            }

            #endregion
        }
    }
}
