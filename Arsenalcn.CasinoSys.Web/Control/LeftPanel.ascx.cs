using System;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeftPanel : System.Web.UI.UserControl
    {
        public int UserId
        { get; set; }

        public string UserName
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserId <= 0)
            {
                pnlMyCasino.Visible = false;
            }
            else
            {
                pnlMyCasino.Visible = true;

                var g = new Gambler(UserId);

                if (g != null)
                {
                    var strRp = g.RPBonus.HasValue ? $"(RP: {g.RPBonus.Value}) " : string.Empty;
                    var strRank = g.ContestRank.HasValue ? $"(Rank: {g.ContestRank.Value}) " : string.Empty;

                    ltrlMyGamblerInfo.Text =
                        $"<li class=\"LiTitle\">博彩币:<em style=\"font-size: 12px; margin: 0 2px\">{g.Cash.ToString("N2")}</em>{(string.IsNullOrEmpty(strRp) && string.IsNullOrEmpty(strRank) ? string.Empty : $"<br /><em style=\"font-size: 12px; margin: 0 2px\" title=\"博彩获得RP | 当前赛季总排名\">{strRp}{strRank}</em>")}</li>";
                }
            }

            #region HideCasinoSysNotice
            if (string.IsNullOrEmpty(ConfigGlobal.SysNotice))
            {
                pnlNotice.Visible = false;
            }
            #endregion

        }
    }
}