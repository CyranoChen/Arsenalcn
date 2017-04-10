using System;
using System.Web.UI.WebControls;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using iArsenal.Service;

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
                var list = repo.Query<MemberPeriod>(x => x.MemberID == Mid).FindAll(x =>
                {
                    var returnValue = true;
                    var tmpString = string.Empty;

                    if (ViewState["MemberCardNo"] != null)
                    {
                        tmpString = ViewState["MemberCardNo"].ToString();
                        if (!string.IsNullOrEmpty(tmpString))
                            returnValue = returnValue &&
                                          x.MemberCardNo.Equals(tmpString, StringComparison.OrdinalIgnoreCase);
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
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void gvMemberPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var mp = e.Row.DataItem as MemberPeriod;

                var btnUpgrade = e.Row.FindControl("btnUpgrade") as HyperLink;
                var btnRenew = e.Row.FindControl("btnRenew") as HyperLink;

                if (btnUpgrade != null)
                {
                    btnUpgrade.Visible = mp.IsCurrentSeason() && mp.MemberClass.Equals(MemberClassType.Core);
                }

                if (btnRenew != null)
                {
                    btnRenew.Visible = mp.IsCurrentSeason(-1);
                }

                if (mp.IsCurrentSeason())
                {
                    e.Row.CssClass = "SelectedRow";
                }
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