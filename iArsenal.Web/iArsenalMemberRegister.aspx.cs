using System;
using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class iArsenalMemberRegister : AcnPageBase
    {
        private readonly IRepository _repo = new Repository();

        private int MemberID
        {
            get
            {
                var m = Member.Cache.LoadByAcnID(UID);

                if (m != null && m.ID > 0)
                {
                    return m.ID;
                }
                return int.MinValue;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            lblACNInfo.Text = $"<em>(ACN ID: {Username})</em> ";

            if (MemberID > 0)
            {
                var m = _repo.Single<Member>(MemberID);

                tbName.Text = m.Name;
                rblGender.SelectedValue = m.Gender.ToString().ToLower();
                tbIDCardNo.Text = m.IDCardNo;
                tbMobile.Text = m.Mobile;
                tbEmail.Text = m.Email;
                tbWeChat.Text = m.WeChat;
                tbAddress.Text = m.Address;
                tbDescription.Text = m.Description;
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
                else
                {
                    m.IsActive = true;
                    m.OfficialSync = "0000";
                    m.Evalution = MemberEvalution.None;
                    m.MemberType = MemberType.None;
                    m.MemberCardNo = string.Empty;
                    m.AcnID = UID;
                    m.AcnName = Username;
                    m.IP = IPLocation.GetIP();
                    m.TaobaoName = string.Empty;
                    m.JoinDate = DateTime.Now;
                    m.LastLoginTime = m.JoinDate;
                    m.Birthday = null;
                    m.PassportName = string.Empty;
                    m.PassportNo = string.Empty;
                    m.QQ = string.Empty;
                    m.Nation = string.Empty;
                    m.Region = string.Empty;
                    m.Career = string.Empty;
                    m.Telephone = string.Empty;
                    m.Zipcode = string.Empty;
                    m.Remark = string.Empty;
                }

                if (!string.IsNullOrEmpty(tbName.Text.Trim()))
                    m.Name = tbName.Text.Trim();
                else
                    throw new Exception("请填写会员真实姓名");

                m.IDCardNo = tbIDCardNo.Text.Trim();
                m.Gender = Convert.ToBoolean(rblGender.SelectedValue);
                m.WeChat = tbWeChat.Text.Trim();

                if (!string.IsNullOrEmpty(tbMobile.Text.Trim()))
                    m.Mobile = tbMobile.Text.Trim();
                else
                    throw new Exception("请填写会员手机");

                if (!string.IsNullOrEmpty(tbEmail.Text.Trim()))
                    m.Email = tbEmail.Text.Trim();
                else
                    throw new Exception("请填写会员邮箱");

                m.Address = tbAddress.Text.Trim();
                m.Description = tbDescription.Text.Trim();

                if (MemberID > 0)
                {
                    _repo.Update(m);

                    Member.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        "alert('您的实名会员信息更新成功');window.location.href=window.location.href+'#anchorBack'", true);
                }
                else
                {
                    _repo.Insert(m);

                    Member.Cache.RefreshCache();

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                        "alert('您的实名会员信息已注册成功');window.location.href= 'Default.aspx'", true);
                }
            }
            catch
            {
                //ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('您的输入有误，请重新填写或联系管理员')");
            }
        }
    }
}