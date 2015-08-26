using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class PortalWorkflowInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(JSONOrderStatusList))
            {
                var jsonSerializer = new JavaScriptSerializer();

                var list = jsonSerializer.Deserialize<List<OrderStatus_Workflow>>(JSONOrderStatusList);

                if (list.Count > 0 && list.Exists(wl => wl.StatusType.Equals(CurrOrderStatus)))
                {

                    CountOrderStatusList = list.Count;

                    rptrWorkflowInfo.DataSource = list;
                    rptrWorkflowInfo.DataBind();
                }
                else
                {
                    rptrWorkflowInfo.Visible = false;
                }
            }
            else
            {
                rptrWorkflowInfo.Visible = false;
            }
        }

        protected void rptrWorkflowInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var wl = e.Item.DataItem as OrderStatus_Workflow;

                var ltrlStateInfo = e.Item.FindControl("ltrlStateInfo") as Literal;

                if (ltrlStateInfo != null)
                {
                    ltrlStateInfo.Text = string.Format("<li style=\"width: {3}%\" id=\"Status-{0}\" {2}>{1}</li>", ((int)wl.StatusType).ToString(),
                        wl.StatusInfo.ToString(), currStatusActive ? "class=\"Active\"" : string.Empty, (100 / CountOrderStatusList).ToString("f0"));

                    if (wl.StatusType.Equals(CurrOrderStatus))
                    { currStatusActive = false; }
                }
            }
        }

        public int CountOrderStatusList = 5;

        public Boolean currStatusActive = true;

        public string JSONOrderStatusList
        { get; set; }

        public OrderStatusType CurrOrderStatus
        { get; set; }

        protected class OrderStatus_Workflow
        {
            public OrderStatus_Workflow() { }

            public OrderStatusType StatusType
            { get; set; }

            public string StatusInfo
            { get; set; }
        }
    }
}