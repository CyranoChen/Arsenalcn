using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.Common.Utility;
using iArsenal.Entity;

namespace iArsenal.Web
{
    public partial class AdminMemberView : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            tbAcnSessionKey.Text = this.SessionKey;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private int MemberID
        {
            get
            {
                int _memberID;
                if (!string.IsNullOrEmpty(Request.QueryString["MemberID"]) && int.TryParse(Request.QueryString["MemberID"], out _memberID))
                {
                    return _memberID;
                }
                else
                    return int.MinValue;
            }
        }

        private void InitForm()
        {
            if (MemberID > 0)
            {
                Member m = new Member();
                m.MemberID = MemberID;
                m.Select();

                lblMemberInfo.Text = string.Format("会员姓名<em>({0})</em>:", MemberID.ToString());

                tbName.Text = m.Name;
                cbIsActive.Checked = m.IsActive;
                ddlEvalution.SelectedValue = ((int)m.Evalution).ToString();
                ddlMemberType.SelectedValue = ((int)m.MemberType).ToString();
                tbMemberCardNo.Text = m.MemberCardNo;
                tbAcnID.Text = m.AcnID.ToString();
                tbAcnName.Text = m.AcnName;
                tbTaobaoName.Text = m.TaobaoName;
                tbIP.Text = m.IP;
                lblIPInfo.Text = IPLocation.GetIPInfo(m.IP);

                tbIDCardNo.Text = m.IDCardNo;
                tbJoinDate.Text = m.JoinDate.ToString("yyyy-MM-dd");
                tbLastLoginTime.Text = m.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime _birthday;
                if (m.Birthday.HasValue && DateTime.TryParse(m.Birthday.ToString(), out _birthday))
                    tbBirthday.Text = _birthday.ToString("yyyy-MM-dd");
                else
                    tbBirthday.Text = string.Empty;

                rblGender.SelectedValue = m.Gender.ToString().ToLower();
                tbPassportName.Text = m.PassportName;
                tbPassportNo.Text = m.PassportNo;
                tbMSN.Text = m.MSN;
                tbQQ.Text = m.QQ;

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

                tbCareer.Text = m.Career;
                tbMobile.Text = m.Mobile;
                tbTelephone.Text = m.Telephone;
                tbEmail.Text = m.Email;
                tbZipCode.Text = m.Zipcode;
                tbAddress.Text = m.Address;

                tbDescription.Text = m.Description;
                tbRemark.Text = m.Remark;

                // Bind MemberPeriod data of this Member
                BindItemData();
            }
            else
            {
                lblMemberInfo.Text = "会员姓名:";
            }
        }

        private void BindItemData()
        {
            List<MemberPeriod> list = MemberPeriod.GetMemberPeriods(MemberID);

            gvMemberPeriod.DataSource = list;
            gvMemberPeriod.DataBind();
        }

        protected void gvMemberPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMemberPeriod.SelectedIndex != -1)
            {
                Response.Redirect(string.Format("AdminMemberPeriodView.aspx?MemberPeriodID={0}", gvMemberPeriod.DataKeys[gvMemberPeriod.SelectedIndex].Value.ToString()));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Member m = new Member();

                m.Name = tbName.Text.Trim();
                m.IsActive = cbIsActive.Checked;
                m.Evalution = (MemberEvalution)Enum.Parse(typeof(MemberEvalution), ddlEvalution.SelectedValue);
                m.MemberType = (MemberType)Enum.Parse(typeof(MemberType), ddlMemberType.SelectedValue);
                m.MemberCardNo = tbMemberCardNo.Text.Trim();
                m.AcnID = Convert.ToInt32(tbAcnID.Text.Trim());
                m.AcnName = tbAcnName.Text.Trim();
                m.TaobaoName = tbTaobaoName.Text.Trim();
                m.IP = tbIP.Text.Trim();

                m.IDCardNo = tbIDCardNo.Text.Trim();

                if (!string.IsNullOrEmpty(tbJoinDate.Text.Trim()))
                    m.JoinDate = Convert.ToDateTime(tbJoinDate.Text.Trim());
                else
                    m.JoinDate = DateTime.Now;

                if (!string.IsNullOrEmpty(tbLastLoginTime.Text.Trim()))
                    m.LastLoginTime = Convert.ToDateTime(tbLastLoginTime.Text.Trim());
                else
                    m.LastLoginTime = m.JoinDate;

                DateTime _birthday;
                if (!string.IsNullOrEmpty(tbBirthday.Text) && DateTime.TryParse(tbBirthday.Text.Trim(), out _birthday))
                    m.Birthday = _birthday;
                else
                    m.Birthday = null;

                m.Gender = Convert.ToBoolean(rblGender.SelectedValue);
                m.PassportName = tbPassportName.Text.Trim();
                m.PassportNo = tbPassportNo.Text.Trim();
                m.MSN = tbMSN.Text.Trim();
                m.QQ = tbQQ.Text.Trim();

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

                m.Career = tbCareer.Text.Trim();
                m.Mobile = tbMobile.Text.Trim();
                m.Telephone = tbTelephone.Text.Trim();
                m.Email = tbEmail.Text.Trim();
                m.Zipcode = tbZipCode.Text.Trim();
                m.Address = tbAddress.Text.Trim();

                m.Description = tbDescription.Text.Trim();
                m.Remark = tbRemark.Text.Trim();

                #region Filter Member By AcnID
                int tmpMID = int.MinValue;
                Member tmpM = new Member();
                tmpM.Select(m.AcnID);

                if (tmpM != null && tmpM.MemberID > 0)
                    tmpMID = tmpM.MemberID;
                #endregion

                if (MemberID > 0 && (MemberID == tmpMID || tmpMID == int.MinValue))
                {
                    m.MemberID = MemberID;
                    m.Update();
                    Member.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    if (tmpMID > 0)
                    {
                        string msg = string.Format("if (confirm('该会员的AcnID已经注册,是否跳转?')) {{ window.location.href = 'AdminMemberView.aspx?MemberID={0}'; }}", tmpMID.ToString());
                        ClientScript.RegisterClientScriptBlock(typeof(string), "failed", msg, true);
                        //ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("JumpToMemberViewer('{0}')", tmpMID.ToString()), true);
                        throw new Exception(string.Format("AcnID in use for Member(No.{0})", tmpMID.ToString()));
                    }
                    else
                    {
                        m.Insert();
                        Member.Cache.RefreshCache();

                        ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminMember.aspx'", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MemberID > 0)
            {
                Response.Redirect("AdminMember.aspx?MemberID=" + MemberID.ToString());
            }
            else
            {
                Response.Redirect("AdminMember.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MemberID > 0)
                {
                    Member m = new Member();
                    m.MemberID = MemberID;
                    m.Delete();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminMember.aspx'", true);
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