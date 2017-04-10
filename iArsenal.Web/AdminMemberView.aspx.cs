using System;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class AdminMemberView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int MemberID
        {
            get
            {
                int memberID;
                if (!string.IsNullOrEmpty(Request.QueryString["MemberID"]) &&
                    int.TryParse(Request.QueryString["MemberID"], out memberID))
                {
                    return memberID;
                }
                return int.MinValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

            tbAcnSessionKey.Text = SessionKey;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            if (MemberID > 0)
            {
                var m = _repo.Single<Member>(MemberID);

                lblMemberInfo.Text = $"会员姓名<em>({MemberID})</em>:";

                tbName.Text = m.Name;
                cbIsActive.Checked = m.IsActive;
                ddlOfficialSync.SelectedValue = m.OfficialSync;
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

                DateTime birthday;
                if (m.Birthday.HasValue && DateTime.TryParse(m.Birthday.ToString(), out birthday))
                    tbBirthday.Text = birthday.ToString("yyyy-MM-dd");
                else
                    tbBirthday.Text = string.Empty;

                rblGender.SelectedValue = m.Gender.ToString().ToLower();
                tbPassportName.Text = m.PassportName;
                tbPassportNo.Text = m.PassportNo;
                tbWeChat.Text = m.WeChat;
                tbQQ.Text = m.QQ;

                #region Set Member Nation & Region

                if (!string.IsNullOrEmpty(m.Nation))
                {
                    if (m.Nation.Equals("中国"))
                    {
                        ddlNation.SelectedValue = m.Nation;

                        var region = m.Region.Split('|');
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
            var list = _repo.Query<MemberPeriod>(x => x.MemberID == MemberID);

            gvMemberPeriod.DataSource = list;
            gvMemberPeriod.DataBind();
        }

        protected void gvMemberPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMemberPeriod.SelectedIndex != -1)
            {
                var key = gvMemberPeriod.DataKeys[gvMemberPeriod.SelectedIndex];
                if (key != null)
                {
                    Response.Redirect($"AdminMemberPeriodView.aspx?MemberPeriodID={key.Value}");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var m = new Member();

                if (MemberID > 0)
                {
                    m = _repo.Single<Member>(MemberID);
                }

                m.Name = tbName.Text.Trim();
                m.IsActive = cbIsActive.Checked;
                m.OfficialSync = ddlOfficialSync.SelectedValue;
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

                DateTime birthday;
                if (!string.IsNullOrEmpty(tbBirthday.Text) && DateTime.TryParse(tbBirthday.Text.Trim(), out birthday))
                    m.Birthday = birthday;
                else
                    m.Birthday = null;

                m.Gender = Convert.ToBoolean(rblGender.SelectedValue);
                m.PassportName = tbPassportName.Text.Trim();
                m.PassportNo = tbPassportNo.Text.Trim();
                m.WeChat = tbWeChat.Text.Trim();
                m.QQ = tbQQ.Text.Trim();

                #region Get Member Nation & Region

                var nation = ddlNation.SelectedValue;

                if (!string.IsNullOrEmpty(nation))
                {
                    if (nation.Equals("中国"))
                    {
                        m.Nation = nation;
                        if (!string.IsNullOrEmpty(tbRegion1.Text.Trim()))
                        {
                            if (!string.IsNullOrEmpty(tbRegion2.Text.Trim()))
                            {
                                m.Region = $"{tbRegion1.Text.Trim()}|{tbRegion2.Text.Trim()}";
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
                        m.Nation = nation;
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

                var tmpMid = int.MinValue;

                var tmpMem = _repo.Query<Member>(x => x.AcnID == m.AcnID).FirstOrDefault();

                if (tmpMem != null)
                {
                    tmpMid = tmpMem.ID > 0 ? tmpMem.ID : int.MinValue;
                }

                #endregion

                if (MemberID > 0 && (MemberID.Equals(tmpMid) || tmpMid <= 0))
                {
                    _repo.Update(m);
                    Member.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href=window.location.href", true);
                }
                else
                {
                    if (tmpMid > 0)
                    {
                        var msg =
                            $"if (confirm('该会员的AcnID已经注册,是否跳转?')) {{ window.location.href = 'AdminMemberView.aspx?MemberID={tmpMid}'; }}";
                        ClientScript.RegisterClientScriptBlock(typeof(string), "failed", msg, true);
                        //ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("JumpToMemberViewer('{0}')", tmpMID.ToString()), true);
                        throw new Exception($"AcnID in use for Member(No.{tmpMid})");
                    }
                    _repo.Insert(m);
                    Member.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminMember.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (MemberID > 0)
            {
                Response.Redirect("AdminMember.aspx?MemberID=" + MemberID);
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
                    _repo.Delete<Member>(MemberID);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('删除成功');window.location.href='AdminMember.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }
    }
}