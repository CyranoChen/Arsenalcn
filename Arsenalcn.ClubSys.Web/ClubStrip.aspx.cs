using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;


namespace Arsenalcn.ClubSys.Web
{
    public partial class ClubStrip : Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var club = ClubLogic.GetClubInfo(ClubID);

            if (club != null && this.Title.IndexOf("{0}") >= 0)
                this.Title = string.Format(this.Title, club.FullName);

            #region SetControlProperty

            ctrlLeftPanel.UserID = this.userid;
            ctrlLeftPanel.UserName = this.username;
            ctrlLeftPanel.UserKey = this.userkey;

            ctrlFieldToolBar.UserID = this.userid;
            ctrlFieldToolBar.UserName = this.username;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.ClubSys.Web.Control.ClubMenuItem.ClubStrip;
            ctrlMenuTabBar.ClubID = ClubID;

            ctrlClubSysHeader.UserID = this.userid;
            ctrlClubSysHeader.ClubID = ClubID;
            ctrlClubSysHeader.UserName = this.username;

            #endregion

            BindStripHistory();

            this.ltlClubBingoStrip.Text =
                $"<span title=\"抽取装备计数\">今天(累计)尝试:<em>{PlayerStrip.GetClubBingoPlayCountToday(ClubID).ToString()}({PlayerStrip.GetClubBingoPlayCount(ClubID).ToString("N0")})</em>次 | 库存:<em>{PlayerStrip.GetClubRemainingEquipment(ClubID).ToString()}/{ConfigGlobal.DailyClubEquipmentCount.ToString()}</em>件装备</span>";
            this.ltlClubStripCount.Text =
                $"<span class=\"ClubSys_Strip SHIRT\">球衣:</span><em>{ShirtCount}</em><span class=\"ClubSys_Strip SHORTS\">球裤:</span><em>{ShortsCount}</em><span class=\"ClubSys_Strip SOCK\">球袜:</span><em>{SockCount}</em><span class=\"ClubSys_Strip CARD\">球星卡:</span><em>{CardCount}</em><span class=\"ClubSys_Strip VIDEO\">视频卡:</span><em>{VideoCount}</em>";
        }

        public int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    Response.Redirect("ClubPortal.aspx");

                    return -1;
                }
            }
        }

        public int ShirtCount
        {
            get
            {
                return PlayerStrip.GetClubEquipmentGainCount(ClubID, BingoResultType.Strip, "shirt");
            }
        }

        public int ShortsCount
        {
            get
            {
                return PlayerStrip.GetClubEquipmentGainCount(ClubID, BingoResultType.Strip, "shorts");
            }
        }

        public int SockCount
        {
            get
            {
                return PlayerStrip.GetClubEquipmentGainCount(ClubID, BingoResultType.Strip, "sock");
            }
        }

        public int CardCount
        {
            get
            {
                return PlayerStrip.GetClubEquipmentGainCount(ClubID, BingoResultType.Card, "card");
            }
        }

        public int VideoCount
        {
            get
            {
                return PlayerStrip.GetClubEquipmentGainCount(ClubID, BingoResultType.Card, "legend");
            }
        }

        private List<BingoHistory> _list;
        private void BindStripHistory()
        {
            if (_list == null)
            {

                _list = PlayerStrip.GetClubBingoHistory(ClubID);

                foreach (var bh in _list)
                {
                    var br = new BingoResult(bh.Result, bh.ResultDetail);
                    switch (br.Result)
                    {
                        case BingoResultType.Strip:
                            if (br.ResultDetail == "strip")
                                bh.AdditionalData = "<span class=\"ClubSys_Strip SHIRT\">球衣</span><span class=\"ClubSys_Strip SHORTS\">球裤</span><span class=\"ClubSys_Strip SOCK\">球袜</span>";
                            else if (br.ResultDetail.Contains("strips"))
                                bh.AdditionalData = "<span class=\"ClubSys_Strip SHIRT\">球衣</span><span class=\"ClubSys_Strip SHORTS\">球裤</span><span class=\"ClubSys_Strip SOCK\">球袜</span><span class=\"ClubSys_Strip RATE\">×5</span>";
                            else
                                bh.AdditionalData = BingoUtil.ShowBothBingoDetail(("<span class=\"ClubSys_Strip " + br.ResultDetail.ToUpper() + "\">{0}</span>"), br);
                            break;
                        case BingoResultType.Card:
                            if (br.ResultDetail == "legend")
                                bh.AdditionalData = "<span class=\"ClubSys_Strip VIDEO\">视频</span>";
                            else
                                bh.AdditionalData =
                                    $"<span class=\"ClubSys_Strip CARD\">{Player.Cache.Load(new Guid(br.ResultDetail)).DisplayName}</span>";
                            break;
                        case BingoResultType.Cash:
                            bh.AdditionalData = "<span class=\"ClubSys_Strip CASH\">枪手币: " + br.ResultDetail + "</span>";
                            break;
                        case BingoResultType.Both:
                            bh.AdditionalData = BingoUtil.ShowBothBingoDetail("<span class=\"ClubSys_Strip " + br.ResultDetail.Substring(0, br.ResultDetail.IndexOf("+")).ToUpper() + "\">{0}</span><span class=\"ClubSys_Strip CASH\">枪手币: {1}</span>", br);
                            break;
                    }
                }
            }

            gvClubStrip.DataSource = _list;
            gvClubStrip.DataBind();
        }

        protected void gvClubStrip_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClubStrip.PageIndex = e.NewPageIndex;

            BindStripHistory();
        }
    }
}
