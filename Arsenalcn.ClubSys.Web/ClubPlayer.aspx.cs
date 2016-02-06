using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Arsenalcn.ClubSys.Web.Control;

namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubPlayer : BasePage
    {
        private List<Gamer> _list;

        public Dictionary<int, int> ClubPlayerLvCount;
        public int FormalPlayerCount;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && Title.IndexOf("{0}") >= 0)
                Title = string.Format(Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;
            ctrlLeftPanel.UserKey = userkey;

            ctrlFieldToolBar.UserID = userid;
            ctrlFieldToolBar.UserName = username;

            ctrlMenuTabBar.CurrentMenu = ClubMenuItem.ClubPlayer;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = username;

            #endregion

            ClubPlayerLvCount = new Dictionary<int, int>();

            for (var i = 0; i <= (ConfigGlobal.PlayerMaxLv + 1); i++)
            {
                ClubPlayerLvCount[i] = 0;
            }
            BindPlayers();

            ltlPlayerCount.Text =
                $"<span>本球会正式/总球员数:<em>{FormalPlayerCount}/{PlayerStrip.GetClubPlayerCount(ClubID)}</em></span>";
            ltlPlayerLv.Text = $"<span>&gt;LV5:{ClubPlayerLvCount[ConfigGlobal.PlayerMaxLv + 1]}</span>";

            for (var j = ConfigGlobal.PlayerMaxLv; j > 0; j--)
            {
                ltlPlayerLv.Text += string.Format(" <span class=\"ClubSys_PlayerLV{0}\">LV{0}:{1}</span>", j,
                    ClubPlayerLvCount[j]);
            }
        }

        private void BindPlayers()
        {
            if (_list == null)
            {
                _list = PlayerStrip.GetClubPlayers(ClubID);

                foreach (var player in _list)
                {
                    if (player.CurrentGuid != null)
                        FormalPlayerCount++;

                    var playerLv = player.Shirt;

                    if (player.Shorts < playerLv)
                        playerLv = player.Shorts;

                    if (player.Sock < playerLv)
                        playerLv = player.Sock;

                    if (playerLv <= ConfigGlobal.PlayerMaxLv)
                    {
                        player.AdditionalData2 = playerLv;
                        player.AdditionalData = playerLv*20;
                    }
                    else
                    {
                        player.AdditionalData2 = ConfigGlobal.PlayerMaxLv;
                        player.AdditionalData = ConfigGlobal.PlayerMaxLv*20;
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

        protected void gvPlayers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlayers.PageIndex = e.NewPageIndex;

            BindPlayers();
        }

        protected void gvPlayers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var player = e.Row.DataItem as Gamer;

                var ltrlNum = e.Row.FindControl("ltrlNum") as Literal;
                var ltrlShirtLV = e.Row.FindControl("ltrlShirtLV") as Literal;
                var ltrlShortsLV = e.Row.FindControl("ltrlShortsLV") as Literal;
                var ltrlSockLV = e.Row.FindControl("ltrlSockLV") as Literal;

                if (player.CurrentGuid != null)
                {
                    var StrSwfContent = "<div class=\"ClubSys_PlayerCardPH\">";
                    StrSwfContent +=
                        string.Format(
                            "<script type=\"text/javascript\">GenSwfObject('PlayerCardActive{0}','swf/PlayerCardActive.swf?XMLURL=ServerXml.aspx%3FPlayerGuid={0}','80','100');</script>",
                            player.CurrentGuid.Value);
                    StrSwfContent += "</div>";
                    ltrlNum.Text = StrSwfContent;
                }
                else
                {
                    ltrlNum.Text =
                        "<img src=\"uploadfiles/PlayerLogo.gif\" alt=\"NoCurrentPlayer\" title=\"NoCurrentPlayer\" height=\"50\" />";
                }

                var _shirtLV = Math.Min(player.Shirt, ConfigGlobal.PlayerMaxLv);
                var _shortsLV = Math.Min(player.Shorts, ConfigGlobal.PlayerMaxLv);
                var _sockLV = Math.Min(player.Sock, ConfigGlobal.PlayerMaxLv);

                ltrlShirtLV.Text =
                    string.Format(
                        "<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/shirt{1}.gif\" alt=\"LV{0}\" /></div>",
                        player.Shirt, _shirtLV);
                ltrlShortsLV.Text =
                    string.Format(
                        "<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/shorts{1}.gif\" alt=\"LV{0}\" /></div>",
                        player.Shorts, _shortsLV);
                ltrlSockLV.Text =
                    string.Format(
                        "<div class=\"Clubsys_StripLV\" title=\"LV{0}\"><img src=\"uploadfiles/StripLV/sock{1}.gif\" alt=\"LV{0}\" /></div>",
                        player.Sock, _sockLV);
            }
        }

        private class PlayerComparer : IComparer<Gamer>
        {
            #region IComparer<Player> Members

            public int Compare(Gamer x, Gamer y)
            {
                if (!string.IsNullOrEmpty(x.CurrentGuid.ToString()) && string.IsNullOrEmpty(y.CurrentGuid.ToString()))
                    return -1;
                if (string.IsNullOrEmpty(x.CurrentGuid.ToString()) == string.IsNullOrEmpty(y.CurrentGuid.ToString()))
                {
                    var xLv = (int) x.AdditionalData2;
                    var yLv = (int) y.AdditionalData2;

                    return yLv - xLv;
                }
                return 0;
            }

            #endregion
        }
    }
}