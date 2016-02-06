using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeagueHeader : UserControl
    {
        private string _pageUrl = string.Empty;

        public Guid CurrLeagueGuid { get; set; } = Guid.Empty;

        public string PageUrl
        {
            set { _pageUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var list = League.Cache.LeagueList_Active;

            rptLeague.DataSource = list;
            rptLeague.DataBind();
        }

        protected void rptLeague_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var l = e.Item.DataItem as League;
                var ltrlLeagueInfo = e.Item.FindControl("ltrlLeagueInfo") as Literal;

                if (l != null && ltrlLeagueInfo != null)
                {
                    var href = $"{_pageUrl}?League={l.ID}";

                    ltrlLeagueInfo.Text =
                        string.Format(
                            "<li id=\"{0}\"><a href=\"{1}\" target=\"_self\" title=\"{3}\"><img src=\"{2}\" alt=\"{3}\" /></a></li>",
                            l.ID, href, l.LeagueLogo, l.LeagueName);
                }
            }
        }
    }
}