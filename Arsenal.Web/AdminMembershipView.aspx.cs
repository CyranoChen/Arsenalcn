using System;
using System.Linq;
using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminMembershipView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid UserGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["UserGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["UserGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            if (UserGuid != Guid.Empty)
            {
                var mem = _repo.Single<Membership>(UserGuid);
                var user = _repo.Single<User>(UserGuid);

                if (mem != null && user != null)
                {
                    tbUserGuid.Text = UserGuid.ToString();
                    tbUserName.Text = mem.UserName;
                    tbMobile.Text = mem.Mobile;
                    tbEmail.Text = mem.Email;
                    tbCreateDate.Text = mem.CreateDate.ToString("yyyy-MM-dd");
                    tbLastPasswordChangedDate.Text = mem.LastPasswordChangedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    tbLastLoginDate.Text = mem.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss");

                    tbLastActivityDate.Text = user.LastActivityDate.ToString("yyyy-MM-dd HH:mm:ss");
                    tbAcnID.Text = user.AcnID.ToString();
                    tbAcnName.Text = user.AcnUserName;
                    tbMemberID.Text = user.MemberID.ToString();
                    tbMemberName.Text = user.MemberName;
                    tbWeChatOpenID.Text = user.WeChatOpenID;
                    tbWeChatNickName.Text = user.WeChatNickName;

                    tbRemark.Text = mem.Remark;

                    var userWeChat = _repo.Single<UserWeChat>(UserGuid);

                    if (userWeChat != null)
                    {
                        tbLastAuthorizeDate.Text = userWeChat.LastAuthorizeDate.ToString("yyyy-MM-dd HH:mm:ss");
                        tbAccessToken.Text = userWeChat.AccessToken;
                        tbAccessTokenExpiredDate.Text = userWeChat.AccessTokenExpiredDate.ToString("yyyy-MM-dd HH:mm:ss");
                        tbRefreshToken.Text = userWeChat.RefreshToken;
                        tbRefreshTokenExpiredDate.Text = userWeChat.RefreshTokenExpiredDate.ToString("yyyy-MM-dd HH:mm:ss");
                        tbGender.Text = userWeChat.Gender.ToString();
                        tbProvince.Text = userWeChat.Province;
                        tbCity.Text = userWeChat.City;
                        tbCountry.Text = userWeChat.Country;
                        tbHeadImgUrl.Text = userWeChat.HeadImgUrl;
                        tbPrivilege.Text = userWeChat.Privilege;
                        tbUnionID.Text = userWeChat.UnionID;
                    }

                    // Bind Avatar data of this Membership
                    BindItemData();
                }
                else
                {
                    tbUserGuid.Text = Guid.NewGuid().ToString();
                    tbCreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                tbUserGuid.Text = Guid.NewGuid().ToString();
                tbCreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void BindItemData()
        {
            var user = _repo.Single<User>(UserGuid);

            if (!string.IsNullOrEmpty(user?.WeChatOpenID))
            {
                var avatars = _repo.Query<User>(x => x.WeChatOpenID == user.WeChatOpenID);
                avatars.RemoveAll(x => x.ID == UserGuid);

                if (avatars.Count > 0)
                {
                    var query = from membership in _repo.All<Membership>()
                                join avatar in avatars on membership.ID equals avatar.ID
                                orderby membership.CreateDate descending
                                select membership;

                    gvAvatar.DataSource = query.ToList();
                    gvAvatar.DataBind();
                }
                else
                {
                    gvAvatar.Visible = false;
                }
            }
            else
            {
                gvAvatar.Visible = false;
            }
        }

        protected void gvAvatar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvAvatar.SelectedIndex != -1)
            {
                var key = gvAvatar.DataKeys[gvAvatar.SelectedIndex];

                if (key != null)
                {
                    Response.Redirect($"AdminMembershipView.aspx?UserGuid={key.Value}");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var mem = new Membership();
                var user = new User();

                //初始化Membership实例的所有字段
                mem.Default();

                if (!UserGuid.Equals(Guid.Empty))
                {
                    mem = _repo.Single<Membership>(UserGuid);
                    user = _repo.Single<User>(UserGuid);
                }

                if (!string.IsNullOrEmpty(tbUserName.Text.Trim()))
                {
                    mem.UserName = tbUserName.Text.Trim();
                    user.UserName = tbUserName.Text.Trim();
                }
                else
                {
                    throw new Exception("请输入统一用户名");
                }

                mem.Mobile = tbMobile.Text.Trim();
                mem.Email = tbEmail.Text.Trim();

                if (!string.IsNullOrEmpty(tbCreateDate.Text.Trim()))
                    mem.CreateDate = Convert.ToDateTime(tbCreateDate.Text.Trim());

                if (!string.IsNullOrEmpty(tbLastPasswordChangedDate.Text.Trim()))
                    mem.LastPasswordChangedDate = Convert.ToDateTime(tbLastPasswordChangedDate.Text.Trim());

                if (!string.IsNullOrEmpty(tbLastLoginDate.Text.Trim()))
                    mem.LastLoginDate = Convert.ToDateTime(tbLastLoginDate.Text.Trim());

                if (!string.IsNullOrEmpty(tbLastActivityDate.Text.Trim()))
                    user.LastActivityDate = Convert.ToDateTime(tbLastActivityDate.Text.Trim());
                else
                    user.LastActivityDate = DateTime.Now;

                if (!string.IsNullOrEmpty(tbAcnID.Text.Trim()))
                    user.AcnID = Convert.ToInt32(tbAcnID.Text.Trim());
                else
                    user.AcnID = null;

                user.AcnUserName = tbAcnName.Text.Trim();

                if (!string.IsNullOrEmpty(tbMemberID.Text.Trim()))
                    user.MemberID = Convert.ToInt32(tbMemberID.Text.Trim());
                else
                    user.MemberID = null;

                user.MemberName = tbMemberName.Text.Trim();

                user.WeChatOpenID = tbWeChatOpenID.Text.Trim();
                user.WeChatNickName = tbWeChatNickName.Text.Trim();

                mem.Remark = tbRemark.Text.Trim();

                if (UserGuid != Guid.Empty)
                {
                    _repo.Update(mem);
                    _repo.Update(user);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    user.IsAnonymous = false;

                    _repo.Insert(mem);
                    _repo.Insert(user);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "save", "alert('添加成功');window.location.href = 'AdminLeague.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserGuid != Guid.Empty)
            {
                Response.Redirect("AdminMembership.aspx?UserGuid=" + UserGuid);
            }
            else
            {
                Response.Redirect("AdminMembership.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserGuid != Guid.Empty)
                {
                    _repo.Delete<Membership>(UserGuid);
                    _repo.Delete<User>(UserGuid);
                    _repo.Delete<UserWeChat>(UserGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminMembership.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除失败')", true);
            }
        }
    }
}