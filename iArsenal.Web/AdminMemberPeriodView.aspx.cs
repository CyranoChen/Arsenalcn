using System;
using System.Collections.Generic;
using System.Linq;

using Arsenalcn.Core;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminMemberPeriodView : AdminPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private int MemberPeriodID
        {
            get
            {
                int _MemberPeriodID;
                if (!string.IsNullOrEmpty(Request.QueryString["MemberPeriodID"]) && int.TryParse(Request.QueryString["MemberPeriodID"], out _MemberPeriodID))
                {
                    return _MemberPeriodID;
                }
                else
                    return int.MinValue;
            }
        }

        private void InitForm()
        {
            if (MemberPeriodID > 0)
            {
                MemberPeriod mp = repo.Single<MemberPeriod>(MemberPeriodID);

                // Set Visible for the member back button for this memberPeriod
                if (mp.MemberID > 0)
                    btnBackMember.Visible = true;

                tbMemberID.Text = mp.MemberID.ToString();
                tbMemberName.Text = mp.MemberName;
                cbIsActive.Checked = mp.IsActive;
                ddlMemberClass.SelectedValue = ((int)mp.MemberClass).ToString();
                tbMemberCardNo.Text = mp.MemberCardNo;

                if (mp.OrderID.HasValue)
                {
                    Order o = repo.Single<Order>(mp.OrderID.Value);

                    tbOrderID.Text = o.ID.ToString();

                    o.CalcOrderPrice();
                    tbOrderPrice.Text = o.PriceInfo;
                }
                else
                {
                    tbOrderID.Text = string.Empty;
                    tbOrderPrice.Text = string.Empty;
                }

                tbStartDate.Text = mp.StartDate.ToString("yyyy-MM-dd");
                tbEndDate.Text = mp.EndDate.ToString("yyyy-MM-dd");

                tbDescription.Text = mp.Description;
                tbRemark.Text = mp.Remark;
            }
            else
            {
                //lblMemberPeriodInfo.Text = "会员姓名:";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                MemberPeriod mp = new MemberPeriod();

                if (MemberPeriodID > 0)
                {
                    mp = repo.Single<MemberPeriod>(MemberPeriodID);
                }

                if (!string.IsNullOrEmpty(tbMemberID.Text.Trim()))
                    mp.MemberID = Convert.ToInt32(tbMemberID.Text.Trim());
                else
                    throw new Exception("Member ID can't be empty");

                mp.MemberName = tbMemberName.Text.Trim();
                mp.IsActive = cbIsActive.Checked;

                if (!string.IsNullOrEmpty(ddlMemberClass.SelectedValue))
                    mp.MemberClass = (MemberClassType)Enum.Parse(typeof(MemberClassType), ddlMemberClass.SelectedValue);
                else
                    throw new Exception("MemberClass can't be empty");

                mp.MemberCardNo = tbMemberCardNo.Text.Trim();

                if (!string.IsNullOrEmpty(tbOrderID.Text.Trim()))
                    mp.OrderID = Convert.ToInt32(tbOrderID.Text.Trim());
                else
                    mp.OrderID = null;

                mp.StartDate = Convert.ToDateTime(tbStartDate.Text.Trim());
                mp.EndDate = Convert.ToDateTime(tbEndDate.Text.Trim());
                mp.Description = tbDescription.Text.Trim();
                mp.Remark = tbRemark.Text.Trim();

                if (MemberPeriodID > 0)
                {
                    repo.Update(mp);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    // Whether this MemberPeriod Exists
                    var list = repo.Query<MemberPeriod>(x => x.MemberID.Equals(mp.MemberID)).ToList();

                    if (list.Any(x => x.StartDate <= mp.StartDate && x.EndDate >= mp.EndDate))
                        throw new Exception(string.Format("The Member Period in active for this Member(No.{0})", mp.MemberID.ToString()));

                    repo.Insert(mp);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminMemberPeriod.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MemberPeriodID > 0)
            {
                Response.Redirect("AdminMemberPeriod.aspx?MemberPeriodID=" + MemberPeriodID.ToString());
            }
            else
            {
                Response.Redirect("AdminMemberPeriod.aspx");
            }
        }

        protected void btnBackMember_Click(object sender, EventArgs e)
        {
            MemberPeriod mp = repo.Single<MemberPeriod>(MemberPeriodID);

            if (mp.MemberID > 0)
                Response.Redirect("AdminMemberView.aspx?MemberID=" + mp.MemberID.ToString());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MemberPeriodID > 0)
                {
                    repo.Delete<MemberPeriod>(MemberPeriodID);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminMemberPeriod.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }
    }
}