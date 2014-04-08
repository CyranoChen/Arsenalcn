using System;

using Arsenalcn.Common.Entity;
using iArsenal.Entity;


namespace iArsenal.Web
{
    public partial class iArsenalMemberRegister : PageBase.AcnPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        private int MemberID
        {
            get
            {
                Member mem = new Member();
                mem.Select(this.UID);

                if (mem != null && mem.MemberID > 0)
                {
                    return mem.MemberID;
                }
                else
                    return int.MinValue;
            }
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
            lblACNInfo.Text = string.Format("<em>(ACN ID: {0})</em> ", Username);

            if (MemberID > 0)
            {
                Member m = new Member();
                m.MemberID = MemberID;
                m.Select();

                tbName.Text = m.Name;
                rblGender.SelectedValue = m.Gender.ToString().ToLower();
                tbIDCardNo.Text = m.IDCardNo;
                tbMobile.Text = m.Mobile;
                tbEmail.Text = m.Email;
                tbQQ.Text = m.QQ;
                tbAddress.Text = m.Address;
                tbDescription.Text = m.Description;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Member m = new Member();

                if (MemberID > 0)
                {
                    m.MemberID = MemberID;
                    m.Select();
                }
                else
                {
                    m.IsActive = true;
                    m.MemberType = MemberType.Buyer;
                    m.MemberCardNo = string.Empty;
                    m.AcnID = this.UID;
                    m.AcnName = this.Username;
                    m.IP = IPLocation.GetIP();
                    m.TaobaoName = string.Empty;
                    m.JoinDate = DateTime.Now;
                    m.LastLoginTime = m.JoinDate;
                    m.Birthday = null;
                    m.PassportName = string.Empty;
                    m.PassportNo = string.Empty;
                    m.MSN = string.Empty;
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
                m.QQ = tbQQ.Text.Trim();

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
                    m.MemberID = MemberID;
                    m.Update();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('您的实名会员信息更新成功');window.location.href=window.location.href+'#anchorBack'", true);
                }
                else
                {
                    m.Insert();
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('您的实名会员信息已注册成功');window.location.href= 'Default.aspx'", true);
                }
            }
            catch
            {
                //ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('您的输入有误，请重新填写或联系管理员')");
            }
        }
    }
}