using System;
using System.Web.UI;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class MenuTabBar : UserControl
    {
        public CasinoMenuType CurrentMenu { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
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