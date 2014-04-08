using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class iArsenalOrder_TicketBeijing : Common.MemberPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ConfigAdmin.IsPluginAdmin(this.UID))
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    InitForm();
                }
            }
        }

        private void InitForm()
        {
            lblMemberName.Text = string.Format("<b>{0}</b> (<em>NO.{1}</em>)", this.MemberName, this.MID.ToString());
            lblMemberACNInfo.Text = string.Format("<b>{0}</b> (<em>ID.{1}</em>)", this.Username, this.UID.ToString());

            //Fill Member draft information into textbox
            Member m = new Member();
            m.MemberID = this.MID;
            m.Select();

            tbIDCardNo.Text = m.IDCardNo;
            tbOrderMobile.Text = m.Mobile;
            tbAlipay.Text = m.TaobaoName;

            #region Set Member Nation & Region
            if (!string.IsNullOrEmpty(m.Nation))
            {
                if (m.Nation.Equals("中国"))
                {
                    ddlNation.SelectedValue = m.Nation;

                    string[] region = m.Region.Split('|');
                    if (region.Length > 1)
                    {
                        tbRegion1.Text = region[0];
                        tbRegion2.Text = region[1];
                    }
                    else
                    {
                        tbRegion1.Text = region[0];
                        tbRegion2.Text = string.Empty;
                    }
                }
                else
                {
                    ddlNation.SelectedValue = "其他";
                    if (m.Nation.Equals("其他"))
                        tbNation.Text = string.Empty;
                    else
                        tbNation.Text = m.Nation;
                }
            }
            else
            {
                ddlNation.SelectedValue = string.Empty;
            }
            #endregion

            List<Product> list = Product.Cache.GetActiveProductListByCodes(ConfigGlobal.CodeTicketBeijing);

            if (list != null)
            {
                ddlTicketBeijing.DataSource = list;
                ddlTicketBeijing.DataValueField = "ProductGuid";
                ddlTicketBeijing.DataBind();

                ddlTicketBeijing.Items.Insert(0, new ListItem("--请选择球票类型--", string.Empty));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlTicketBeijing.SelectedValue))
                {
                    Member m = new Member();
                    m.MemberID = this.MID;
                    m.Select();

                    // Update Current Member Info
                    m.IDCardNo = tbIDCardNo.Text.Trim();

                    #region Get Member Nation & Region
                    string _nation = ddlNation.SelectedValue;

                    if (!string.IsNullOrEmpty(_nation))
                    {
                        if (_nation.Equals("中国"))
                        {
                            m.Nation = _nation;
                            if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(tbRegion2.Text.Trim()))
                                {
                                    m.Region = string.Format("{0}|{1}", tbRegion1.Text.Trim(), tbRegion2.Text.Trim());
                                }
                                else
                                {
                                    m.Region = tbRegion1.Text.Trim();
                                }
                            }
                            else
                            {
                                m.Region = string.Empty;
                            }
                        }
                        else if (!string.IsNullOrEmpty(tbNation.Text.Trim()))
                        {
                            m.Nation = tbNation.Text.Trim();
                            m.Region = string.Empty;
                        }
                        else
                        {
                            m.Nation = _nation;
                            m.Region = string.Empty;
                        }
                    }
                    else
                    {
                        m.Nation = string.Empty;
                        m.Region = string.Empty;
                    }
                    #endregion

                    m.Update();

                    //New Order
                    Order o = new Order();

                    o.MemberID = m.MemberID;
                    o.MemberName = m.Name;
                    o.Mobile = tbOrderMobile.Text.Trim();
                    o.Address = string.Empty;
                    o.Payment = "{" + rblOrderPayment.SelectedValue + "|" + tbAlipay.Text.Trim() + tbBankName.Text.Trim() + tbBankAccount.Text.Trim() + "}";
                    o.Price = 0f;
                    o.Sale = null;
                    o.Deposit = null;
                    o.Postage = 0f;
                    o.Status = OrderStatus.Draft;
                    o.Rate = 0;
                    o.CreateTime = DateTime.Now;
                    o.UpdateTime = DateTime.Now;
                    o.IsActive = true;
                    o.Description = tbOrderDescription.Text.Trim();
                    o.Remark = string.Empty;

                    //Get the Order ID after Insert new one
                    o.Insert();
                    o.Select();

                    if (o.OrderID > 0)
                    {
                        //New Order Item for Tickets
                        Product pTicketBeijing = new Product();
                        pTicketBeijing.ProductGuid = new Guid(ddlTicketBeijing.SelectedValue);
                        pTicketBeijing.Select();

                        if (pTicketBeijing == null)
                            throw new Exception("无相关商品或缺货，请联系管理员");

                        string oiRemark = string.Empty;

                        if (cbDaytimeEvent.Checked)
                            oiRemark += (" {" + cbDaytimeEvent.Text.Trim() + "}");

                        if (cbEveningEvent.Checked)
                            oiRemark += " {" + cbEveningEvent.Text.Trim() + "}";

                        OrderItem.WishOrderItem(m, pTicketBeijing, o, ddlSeatLevel.SelectedValue, Convert.ToInt32(tbQuantity.Text.Trim()), null, oiRemark.Trim());
                    }

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", string.Format("alert('订单({0})保存成功');window.location.href = 'iArsenalOrderView_TicketBeijing.aspx?OrderID={0}'", o.OrderID.ToString()), true);

                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void ddlTicketBeijing_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in (sender as DropDownList).Items)
            {
                Product p = new Product();
                p.ProductGuid = new Guid(li.Value);
                p.Select();

                li.Text = string.Format("({0}) {1} - 售价{3}元", p.Code, p.DisplayName, p.Name, Product.Cache.GetProductPriceByID(p.ProductGuid).ToString("f2"));
            }
        }
    }
}