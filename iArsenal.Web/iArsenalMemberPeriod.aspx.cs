using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using iArsenal.Service;
using Arsenalcn.Core;

namespace iArsenal.Web
{
    public partial class iArsenalMemberPeriod : MemberPageBase
    {
        private readonly IRepository repo = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            tbMemberCardNo.Attributes["placeholder"] = "--会籍卡号--";

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                var list = repo.Query<MemberPeriod>(x => x.MemberID.Equals(this.MID)).ToList().FindAll(x =>
                {
                    Boolean returnValue = true;
                    string tmpString = string.Empty;

                    if (ViewState["MemberCardNo"] != null)
                    {
                        tmpString = ViewState["MemberCardNo"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue && x.MemberCardNo.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
                    }

                    // Find all MemberPeriods which belong to Current Member & Active
                    returnValue = returnValue && x.IsActive;

                    return returnValue;
                });

                gvMemberPeriod.DataSource = list;
                gvMemberPeriod.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMemberCardNo.Text.Trim()))
                ViewState["MemberCardNo"] = tbMemberCardNo.Text.Trim();
            else
                ViewState["MemberCardNo"] = string.Empty;

            gvMemberPeriod.PageIndex = 0;

            BindData();
        }
    }
}