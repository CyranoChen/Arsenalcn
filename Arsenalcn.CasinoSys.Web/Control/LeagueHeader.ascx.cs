using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class LeagueHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtLeague = Entity.League.GetLeague(true);

            rptLeague.DataSource = dtLeague;
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
                DataRowView drv = e.Item.DataItem as DataRowView;
                Literal ltrlLeagueInfo = e.Item.FindControl("ltrlLeagueInfo") as Literal;

                League l = new League((Guid)drv["LeagueGuid"]);

                string href = string.Format("{0}?League={1}", pageURL, l.LeagueGuid.ToString());

                ltrlLeagueInfo.Text = string.Format("<li id=\"{0}\"><a href=\"{1}\" target=\"_self\" title=\"{3}\"><img src=\"{2}\" alt=\"{3}\" /></a></li>", l.LeagueGuid.ToString(), href, l.LeagueLogo.ToString(), l.LeagueName.ToString());
            }
        }
    }
}