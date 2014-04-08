using System;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class MenuTabBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public CasinoMenuType CurrentMenu
        { get; set; }
    }

    public enum CasinoMenuType
    {
        CasinoPortal,
        CasinoGame,
        CasinoGroup,
        CasinoBetLog,
        CasinoGambler,
        CasinoRank
    }
}