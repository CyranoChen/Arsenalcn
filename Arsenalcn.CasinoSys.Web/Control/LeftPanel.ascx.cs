using System;

using Arsenalcn.CasinoSys.Entity;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeftPanel : System.Web.UI.UserControl
    {
        public int UserID
        { get; set; }

        public string UserName
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserID <= 0)
            {
                pnlMyCasino.Visible = false;
            }
            else
            {
                pnlMyCasino.Visible = true;

                Gambler g = new Gambler(UserID);

                if (g != null)
                {
                    string _strRP = g.RPBonus.HasValue ? string.Format("(RP: {0}) ", g.RPBonus.Value) : string.Empty;
                    string _strRank = g.ContestRank.HasValue ? string.Format("(Rank: {0}) ", g.ContestRank.Value) : string.Empty;

                    ltrlMyGamblerInfo.Text = string.Format("<li class=\"LiTitle\">博彩币:<em style=\"font-size: 12px; margin: 0px 2px\">{0}</em>{1}</li>", g.Cash.ToString("N2"),
                        string.IsNullOrEmpty(_strRP) && string.IsNullOrEmpty(_strRank)
                        ? string.Empty : string.Format("<br /><em style=\"font-size: 12px; margin: 0px 2px\" title=\"博彩获得RP | 当前赛季总排名\">{0}{1}</em>", _strRP, _strRank));
                }
            }

            #region HideCasinoSysNotice
            if (string.IsNullOrEmpty(Entity.ConfigGlobal.SysNotice))
            {
                pnlNotice.Visible = false;
            }
            #endregion

        }
    }
}