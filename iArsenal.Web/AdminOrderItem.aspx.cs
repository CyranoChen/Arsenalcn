using System;
using System.Linq;
using System.Web.UI.WebControls;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class AdminOrderItem : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;
            ctrlCustomPagerInfo.PageChanged += new Control.CustomPagerInfo.PageChangedEventHandler(ctrlCustomPagerInfo_PageChanged);

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private int _orderItemID = int.MinValue;
        private int OrderItemID
        {
            get
            {
                int _res;
                if (_orderItemID == 0)
                    return _orderItemID;
                else if (!string.IsNullOrEmpty(Request.QueryString["OrderItemID"]) && int.TryParse(Request.QueryString["OrderItemID"], out _res))
                    return _res;
                else
                    return int.MinValue;
            }
            set { _orderItemID = value; }
        }

        private void BindData()
        {
            var list = repo.All<OrderItem>().ToList().FindAll(x =>
            {
                Boolean returnValue = true;
                string tmpString = string.Empty;

                if (ViewState["OrderItemID"] != null)
                {
                    tmpString = ViewState["OrderItemID"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--许愿编号--"))
                        returnValue = returnValue && x.ID.Equals(Convert.ToInt32(tmpString));
                }

                if (ViewState["MemberName"] != null)
                {
                    tmpString = ViewState["MemberName"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--会员姓名--"))
                        returnValue = returnValue && x.MemberName.Contains(tmpString);
                }

                if (ViewState["Code"] != null)
                {
                    tmpString = ViewState["Code"].ToString();
                    if (!string.IsNullOrEmpty(tmpString) && !tmpString.Equals("--编码--"))
                        returnValue = returnValue && x.Code.ToLower().Contains(tmpString.ToLower());
                }

                return returnValue;
            });

            #region set GridView Selected PageIndex
            if (OrderItemID > 0)
            {
                int i = list.FindIndex(x => x.ID.Equals(OrderItemID));
                if (i >= 0)
                {
                    gvOrderItem.PageIndex = i / gvOrderItem.PageSize;
                    gvOrderItem.SelectedIndex = i % gvOrderItem.PageSize;
                }
                else
                {
                    gvOrderItem.PageIndex = 0;
                    gvOrderItem.SelectedIndex = -1;
                }
            }
            else
            {
                gvOrderItem.SelectedIndex = -1;
            }
            #endregion

            gvOrderItem.DataSource = list;
            gvOrderItem.DataBind();

            #region set Control Custom Pager
            if (gvOrderItem.BottomPagerRow != null)
            {
                gvOrderItem.BottomPagerRow.Visible = true;
                ctrlCustomPagerInfo.Visible = true;

                ctrlCustomPagerInfo.PageIndex = gvOrderItem.PageIndex;
                ctrlCustomPagerInfo.PageCount = gvOrderItem.PageCount;
                ctrlCustomPagerInfo.RowCount = list.Count;
                ctrlCustomPagerInfo.InitComponent();
            }
            else
            {
                ctrlCustomPagerInfo.Visible = false;
            }
            #endregion
        }

        protected void gvOrderItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrderItem.PageIndex = e.NewPageIndex;
            OrderItemID = 0;

            BindData();
        }

        protected void ctrlCustomPagerInfo_PageChanged(object sender, Control.CustomPagerInfo.DataNavigatorEventArgs e)
        {
            if (e.PageIndex > 0)
            {
                gvOrderItem.PageIndex = e.PageIndex;
                OrderItemID = 0;
            }

            BindData();
        }

        protected void gvOrderItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvOrderItem.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminOrderItemView.aspx?OrderItemID={0}", gvOrderItem.DataKeys[gvOrderItem.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbOrderItemID.Text.Trim()))
                ViewState["OrderItemID"] = tbOrderItemID.Text.Trim();
            else
                ViewState["OrderItemID"] = string.Empty;

            if (!string.IsNullOrEmpty(tbMemberName.Text.Trim()))
                ViewState["MemberName"] = tbMemberName.Text.Trim();
            else
                ViewState["MemberName"] = string.Empty;

            if (!string.IsNullOrEmpty(tbCode.Text.Trim()))
                ViewState["Code"] = tbCode.Text.Trim();
            else
                ViewState["Code"] = string.Empty;

            OrderItemID = 0;
            gvOrderItem.PageIndex = 0;

            BindData();
        }
    }
}
