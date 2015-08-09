using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.CasinoSys.Entity;

namespace Arsenalcn.CasinoSys.Web.Control
{
    public partial class TopContestList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ConfigGlobal.DefaultLeagueID.Equals(Guid.Empty))
            {
                League l = League.Cache.Load(ConfigGlobal.DefaultLeagueID);
                ltrlContestTitle.Text = string.Format("<a title=\"{0}{1}\">ACN博彩竞赛排行榜</a>", l.LeagueName, l.LeagueSeason);

                float _tbs = ConfigGlobal.TotalBetStandard;

                List<Entity.CasinoGambler> listUpper =
                    Entity.CasinoGambler.GetCasinoGamblers(l.ID).FindAll(delegate(Entity.CasinoGambler cg)
                    { return cg.TotalBet >= _tbs; });

                List<Entity.CasinoGambler> listLower =
                    Entity.CasinoGambler.GetCasinoGamblers(l.ID).FindAll(delegate(Entity.CasinoGambler cg)
                    { return cg.TotalBet < _tbs; });

                if (listUpper.Count == 0 && listLower.Count == 0)
                {
                    pnlContestTop.Visible = false;
                }
                else
                {
                    listUpper = Entity.CasinoGambler.SortCasinoGambler(listUpper);
                    rptContestUpper.DataSource = listUpper.GetRange(0, listUpper.Count > 5 ? 5 : listUpper.Count);
                    rptContestUpper.DataBind();

                    listLower = Entity.CasinoGambler.SortCasinoGambler(listLower);
                    rptContestLower.DataSource = listLower.GetRange(0, listLower.Count > 5 ? 5 : listLower.Count);
                    rptContestLower.DataBind();

                    pnlContestTop.Visible = true;
                }
            }
            else
            {
                pnlContestTop.Visible = false;
            }
        }

        protected void rptContestUpper_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Entity.CasinoGambler cg = e.Item.DataItem as Entity.CasinoGambler;

                Literal ltrlUpperGamblerRank = e.Item.FindControl("ltrlUpperGamblerRank") as Literal;

                if (ltrlUpperGamblerRank != null)
                    ltrlUpperGamblerRank.Text = string.Format("<li class=\"IconTop{0}\"><em title=\"竞赛积分\">{0}</em><a href=\"MyBonusLog.aspx?UserID={1}\">{2}</a></li>",
                        cg.Rank, cg.UserID, cg.UserName.Trim());
            }
        }

        protected void rptContestLower_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Entity.CasinoGambler cg = e.Item.DataItem as Entity.CasinoGambler;

                Literal ltrlLowerGamblerRank = e.Item.FindControl("ltrlLowerGamblerRank") as Literal;

                if (ltrlLowerGamblerRank != null)
                    ltrlLowerGamblerRank.Text = string.Format("<li class=\"IconTop{0}\"><em title=\"竞赛积分\">{0}</em><a href=\"MyBonusLog.aspx?UserID={1}\">{2}</a></li>",
                        cg.Rank, cg.UserID, cg.UserName.Trim());
            }
        }
    }
}