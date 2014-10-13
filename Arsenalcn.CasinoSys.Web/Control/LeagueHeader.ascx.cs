using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;
using ArsenalLeauge = Arsenalcn.CasinoSys.Entity.Arsenal.League;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeagueHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ArsenalLeauge> list = Entity.Arsenal_League.Cache.LeagueList_Active;

            rptLeague.DataSource = list;
            rptLeague.DataBind();
        }

        private Guid currLeagueGuid = Guid.Empty;
        public Guid CurrLeagueGuid
        {
            get
            {
                return currLeagueGuid;
            }
            set
            {
                currLeagueGuid = value;
            }
        }

        private string pageURL = string.Empty;
        public string PageURL
        {
            set
            {
                pageURL = value;
            }
        }

        protected void rptLeague_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ArsenalLeauge l = e.Item.DataItem as ArsenalLeauge;
                Literal ltrlLeagueInfo = e.Item.FindControl("ltrlLeagueInfo") as Literal;

                string href = string.Format("{0}?League={1}", pageURL, l.LeagueGuid.ToString());

                ltrlLeagueInfo.Text = string.Format("<li id=\"{0}\"><a href=\"{1}\" target=\"_self\" title=\"{3}\"><img src=\"{2}\" alt=\"{3}\" /></a></li>", l.LeagueGuid.ToString(), href, l.LeagueLogo.ToString(), l.LeagueName.ToString());
            }
        }
    }
}