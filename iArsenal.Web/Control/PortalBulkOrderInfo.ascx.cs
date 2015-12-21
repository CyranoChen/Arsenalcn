using System;

using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class PortalBulkOrderInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var strBulkOrderInfo = ConfigGlobal.BulkOrderInfo;

            if (!strBulkOrderInfo[0].Equals(string.Empty) && strBulkOrderInfo.Length > 0)
            {
                if (strBulkOrderInfo.Length > 0) { lblBulkOrderInfo_Period.Text = $"<em>{strBulkOrderInfo[0]}</em>"; }
                if (strBulkOrderInfo.Length > 1) { lblBulkOrderInfo_ExchangeRate.Text =
                    $"<em>{strBulkOrderInfo[1]}</em>"; }
                if (strBulkOrderInfo.Length > 2) { lblBulkOrderInfo_Deadline.Text = $"<em>{strBulkOrderInfo[2]}</em>"; }
                if (strBulkOrderInfo.Length > 3) { lblBulkOrderInfo_ArrivalDate.Text = $"<em>{strBulkOrderInfo[3]}</em>"; }
            }
            else
            {
                pnlBulkOrderInfo.Visible = false;
            }

        }
    }
}