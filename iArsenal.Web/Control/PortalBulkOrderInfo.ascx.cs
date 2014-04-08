using System;

using iArsenal.Entity;

namespace iArsenal.Web.Control
{
    public partial class PortalBulkOrderInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] strBulkOrderInfo = ConfigGlobal.BulkOrderInfo;

            if (!strBulkOrderInfo[0].Equals(string.Empty) && strBulkOrderInfo.Length > 0)
            {
                if (strBulkOrderInfo.Length > 0) { lblBulkOrderInfo_Period.Text = string.Format("<em>{0}</em>", strBulkOrderInfo[0]); }
                if (strBulkOrderInfo.Length > 1) { lblBulkOrderInfo_ExchangeRate.Text = string.Format("<em>{0}</em>", strBulkOrderInfo[1]); }
                if (strBulkOrderInfo.Length > 2) { lblBulkOrderInfo_Deadline.Text = string.Format("<em>{0}</em>", strBulkOrderInfo[2]); }
                if (strBulkOrderInfo.Length > 3) { lblBulkOrderInfo_ArrivalDate.Text = string.Format("<em>{0}</em>", strBulkOrderInfo[3]); }
            }
            else
            {
                pnlBulkOrderInfo.Visible = false;
            }

        }
    }
}