using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

using ArsenalPlayer = Arsenal.Entity.Player;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminStrip : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;

            BindStripHistory();
        }

        private List<BingoHistory> _list;
        private void BindStripHistory()
        {
            if (_list == null)
            {

                _list = PlayerStrip.GetAllBingoHistory();
            }

            gvClubStrip.DataSource = _list;
            gvClubStrip.DataBind();
        }

        protected void gvClubStrip_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubStrip.PageIndex = e.NewPageIndex;

            BindStripHistory();
        }

        protected void gvClubStrip_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                BingoHistory bh = e.Row.DataItem as BingoHistory;

                BingoResult br = new BingoResult(bh.Result, bh.ResultDetail);

                string finalResult = string.Empty;

                switch (br.Result)
                {
                    case BingoResultType.Strip:
                        if (br.ResultDetail == "strip")
                            finalResult = "<span class=\"ClubSys_Strip SHIRT\">球衣</span><span class=\"ClubSys_Strip SHORTS\">球裤</span><span class=\"ClubSys_Strip SOCK\">球袜</span>";
                        else if (br.ResultDetail.Contains("strips"))
                            finalResult = "<span class=\"ClubSys_Strip SHIRT\">球衣</span><span class=\"ClubSys_Strip SHORTS\">球裤</span><span class=\"ClubSys_Strip SOCK\">球袜</span><span class=\"ClubSys_Strip RATE\">×5</span>";
                        else
                            finalResult = BingoUtil.ShowBothBingoDetail(("<span class=\"ClubSys_Strip "+br.ResultDetail.ToUpper()+"\">{0}</span>"), br);
                        break;
                    case BingoResultType.Card:
                        if (br.ResultDetail == "legend")
                            finalResult = "<span class=\"ClubSys_Strip VIDEO\">视频</span>";
                        else
                            finalResult = string.Format("<span class=\"ClubSys_Strip CARD\">{0}</span>", ArsenalPlayer.Cache.Load(new Guid(br.ResultDetail)).DisplayName);
                        break;
                    case BingoResultType.Cash:
                        finalResult = "<span class=\"ClubSys_Strip CASH\">枪手币: " + br.ResultDetail.ToString() + "</span>";
                        break;
                    case BingoResultType.Both:
                        finalResult = BingoUtil.ShowBothBingoDetail("<span class=\"ClubSys_Strip " + br.ResultDetail.Substring(0, br.ResultDetail.IndexOf("+")).ToUpper() + "\">{0}</span> <span class=\"ClubSys_Strip CASH\">枪手币: {1}</span>", br);
                        break;
                }

                Literal ltrlStrip = e.Row.FindControl("ltrlStrip") as Literal;
                ltrlStrip.Text = finalResult;
            }
        }
    }
}
