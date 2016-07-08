using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Security;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenal.Service.Rest;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Utility;
using Newtonsoft.Json.Linq;
using Membership = Arsenal.Service.Membership;

namespace Arsenal.Mobile.Models
{
    public class MembershipDto : Membership
    {
        private readonly ILog _log = new AppLog();

        //
        // Summary:
        //     Gets the information from the data source for the membership user associated
        //     with the specified unique identifier.
        //
        // Parameters:
        //   providerUserKey:
        //     The unique user identifier from the membership data source for the user.
        //
        // Returns:
        //     A System.Web.Security.MembershipUser object representing the user associated
        //     with the specified unique identifier.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     providerUserKey is null.
        public static Membership Single(object providerUserKey)
        {
            Contract.Requires(providerUserKey != null);

            IRepository repo = new Repository();

            var membership = repo.Single<Membership>(providerUserKey);

            return membership;
        }

        public static Membership Single(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            IRepository repo = new Repository();

            var query = repo.Query<Membership>(x => x.UserName == username);

            return query.Count > 0 ? query[0] : null;
        }

        public object SignIn(Membership membership)
        {
            IRepository repo = new Repository();

            membership.LastLoginDate = DateTime.Now;
            repo.Update(membership);

            UserDto.SetSession(membership.ID);

            return membership.ID;
        }

        public static bool ValidateUser(string username, out Membership membership)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            IRepository repo = new Repository();

            var query = repo.Query<Membership>(x => x.UserName == username);

            if (query.Any())
            {
                membership = query[0];
            }
            else
            {
                membership = null;
            }

            return query.Any();
        }

        //
        // Summary:
        //     Verifies that the supplied user name and password are valid.
        //
        // Parameters:
        //   username:
        //     The name of the user to be validated.
        //
        //   password:
        //     The password for the specified user.
        //
        // Returns:
        //     true if the supplied user name and password are valid; otherwise, false.
        //public static bool ValidateUser(string username, string password, out object providerUserKey)
        //{
        //    Contract.Requires(!string.IsNullOrEmpty(username));
        //    Contract.Requires(!string.IsNullOrEmpty(password));

        //    IRepository repo = new Repository();

        //    var query = repo.Query<Membership>(x =>
        //        x.UserName == username && x.Password == Encrypt.GetMd5Hash(password));

        //    if (query.Count > 0)
        //    {
        //        var membership = query[0];

        //        providerUserKey = membership.ID;

        //        membership.LastLoginDate = DateTime.Now;
        //        repo.Update(membership);
        //    }
        //    else
        //    {
        //        providerUserKey = null;
        //    }

        //    return query.Count > 0;
        //}

        public static bool ValidateAcnUser(string username, string password, out int acnUid)
        {
            acnUid = -1;

            if (!ConfigGlobal_Arsenal.AcnSync)
            {
                return false;
            }

            var client = new DiscuzApiClient();

            var uid = client.AuthValidate(username, Encrypt.GetMd5Hash(password));

            return int.TryParse(uid.Replace("\"", ""), out acnUid);
        }

        public static int GetAcnId(string username)
        {
            if (!ConfigGlobal_Arsenal.AcnSync)
            {
                return -1;
            }

            var client = new DiscuzApiClient();

            var uid = client.UsersGetID(username);

            int acnUid;

            if (int.TryParse(uid.Replace("\"", ""), out acnUid))
            {
                return acnUid;
            }
            return 0;
        }

        // Summary:
        //     Adds a new user to the data store by exist Acn user.
        //
        public void CreateAcnUser(int uid, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.UserRejected;

            if (!ConfigGlobal_Arsenal.AcnSync)
            {
                return;
            }

            #region Get Acn UserInfo to init the intance of MembershipDto

            var client = new DiscuzApiClient();

            int[] uids = { uid };
            string[] fields = { "user_name", "password", "email", "mobile", "join_date" };

            var responseResult = client.UsersGetInfo(uids, fields);

            if (string.IsNullOrEmpty(responseResult))
            {
                return;
            }

            var jlist = JArray.Parse(responseResult);
            var json = jlist[0];

            Init();

            UserName = json["user_name"].ToString();
            Password = json["password"].ToString();
            Mobile = json["mobile"].ToString();
            Email = json["email"].ToString();
            CreateDate = Convert.ToDateTime(json["join_date"].ToString());
            Remark = $"{{\"AcnID\": {uid}}}";

            #endregion

            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    object providerUserKey;

                    repo.Insert<Membership>(this, out providerUserKey, trans);

                    var user = new User
                    {
                        ID = (Guid)providerUserKey,
                        UserName = UserName,
                        IsAnonymous = false,
                        LastActivityDate = DateTime.Now,
                        AcnID = uid,
                        AcnUserName = UserName,
                        MemberID = null,
                        MemberName = string.Empty,
                        WeChatOpenID = null,
                        WeChatNickName = string.Empty
                    };


                    repo.Insert(user, trans);

                    trans.Commit();

                    status = MembershipCreateStatus.Success;
                }
                catch
                {
                    trans.Rollback();

                    status = MembershipCreateStatus.ProviderError;
                }
            }
        }

        // Summary:
        //     Adds a new user to the data store.
        //
        // Parameters:
        //   username:
        //     The user name for the new user.
        //
        //   password:
        //     The password for the new user.
        //
        // Returns:
        //     A System.Web.Security.MembershipUser object for the newly created user.
        //
        // Exceptions:
        //   System.Web.Security.MembershipCreateUserException:
        //     The user was not created. Check the System.Web.Security.MembershipCreateUserException.StatusCode
        //     property for a System.Web.Security.MembershipCreateStatus value.
        public void CreateUser(string username, string email, string password, out object providerUserKey,
            out MembershipCreateStatus status)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();
                    providerUserKey = null;

                    Init();

                    #region Check username

                    if (string.IsNullOrEmpty(username))
                    {
                        status = MembershipCreateStatus.InvalidUserName;
                        return;
                    }

                    if (string.IsNullOrEmpty(email))
                    {
                        status = MembershipCreateStatus.InvalidEmail;
                        return;
                    }

                    if (ConfigGlobal_Arsenal.AcnSync && GetAcnId(username) > 0)
                    {
                        status = MembershipCreateStatus.DuplicateUserName;
                        return;
                    }

                    if (Single(username) != null)
                    {
                        status = MembershipCreateStatus.DuplicateUserName;
                        return;
                    }

                    UserName = username;

                    #endregion

                    Password = Encrypt.GetMd5Hash(password);
                    Mobile = string.Empty;
                    Email = email;

                    repo.Insert<Membership>(this, out providerUserKey, trans);

                    #region Check user in the data store

                    if (repo.Single<User>(providerUserKey) != null)
                    {
                        status = MembershipCreateStatus.DuplicateProviderUserKey;
                        return;
                    }

                    #endregion

                    var user = new User();

                    user.ID = (Guid)providerUserKey;
                    user.UserName = UserName;
                    user.IsAnonymous = false;
                    user.LastActivityDate = DateTime.Now;

                    #region Register new Acn User

                    if (ConfigGlobal_Arsenal.AcnSync)
                    {
                        var client = new DiscuzApiClient();

                        var uid = client.AuthRegister(UserName, Password, Email);

                        user.AcnID = Convert.ToInt32(uid.Replace("\"", ""));
                        user.AcnUserName = UserName;
                    }
                    else
                    {
                        user.AcnID = null;
                        user.AcnUserName = string.Empty;
                    }

                    #endregion

                    user.MemberID = null;
                    user.MemberName = string.Empty;
                    user.WeChatOpenID = null;
                    user.WeChatNickName = string.Empty;

                    repo.Insert(user, trans);

                    trans.Commit();

                    status = MembershipCreateStatus.Success;
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    _log.Error(ex, new LogInfo
                    {
                        MethodInstance = MethodBase.GetCurrentMethod(),
                        ThreadInstance = Thread.CurrentThread
                    });

                    providerUserKey = null;

                    status = MembershipCreateStatus.ProviderError;
                }
            }
        }

        // Summary:
        //     Updates the password for the membership user in the membership data store.
        //
        // Parameters:
        //   oldPassword:
        //     The current password for the membership user.
        //
        //   newPassword:
        //     The new password for the membership user.
        //
        // Returns:
        //     true if the update was successful; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     oldPassword is an empty string.-or-newPassword is an empty string.
        //
        //   System.ArgumentNullException:
        //     oldPassword is null.-or-newPassword is null.
        //
        //   System.PlatformNotSupportedException:
        //     This method is not available. This can occur if the application targets the
        //     .NET Framework 4 Client Profile. To prevent this exception, override the
        //     method, or change the application to target the full version of the .NET
        //     Framework.
        public static bool ChangePassword(Membership instance, string oldPassword, string newPassword)
        {
            Contract.Requires(!string.IsNullOrEmpty(oldPassword));
            Contract.Requires(!string.IsNullOrEmpty(newPassword));

            if (oldPassword.Equals(newPassword, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("新密码应与旧密码不同");
            }

            if (!instance.Password.Equals(Encrypt.GetMd5Hash(oldPassword)))
            {
                throw new Exception("用户旧密码验证不正确");
            }

            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    instance.Password = Encrypt.GetMd5Hash(newPassword);
                    instance.LastPasswordChangedDate = DateTime.Now;

                    repo.Update(instance, trans);

                    #region Sync Acn User Password

                    if (ConfigGlobal_Arsenal.AcnSync)
                    {
                        var user = repo.Single<User>(instance.ID);

                        if (user?.AcnID != null)
                        {
                            var client = new DiscuzApiClient();

                            var result = client.UsersChangePassword(user.AcnID.Value,
                                Encrypt.GetMd5Hash(oldPassword), Encrypt.GetMd5Hash(newPassword));

                            if (!Convert.ToBoolean(result.Replace("\"", "")))
                            {
                                throw new Exception("ACN同步失败");
                            }
                        }
                    }

                    #endregion

                    trans.Commit();

                    return true;
                }
                catch
                {
                    trans.Rollback();

                    throw;
                }
            }
        }
    }

    public class UserDto : User
    {
        public static User Single(object providerUserKey)
        {
            Contract.Requires(providerUserKey != null);

            IRepository repo = new Repository();

            return repo.Single<User>(providerUserKey);
        }

        public static User GetSession()
        {
            if (HttpContext.Current.Session["AuthorizedUser"] != null)
            {
                return HttpContext.Current.Session["AuthorizedUser"] as User;
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                // Get username from User.Indentity.Name
                var membership = MembershipDto.Single(HttpContext.Current.User.Identity.Name);

                if (membership == null)
                {
                    return null;
                }

                SetSession(membership.ID);
                return HttpContext.Current.Session["AuthorizedUser"] as User;
            }
            return null;
        }

        public static void SetSession(object providerUserKey, bool isAnonymous = false)
        {
            Contract.Requires(providerUserKey != null);

            var user = Single(providerUserKey);

            // update user lastActivityDate & isAnonymous
            IRepository repo = new Repository();

            user.LastActivityDate = DateTime.Now;
            user.IsAnonymous = isAnonymous;

            repo.Update(user);

            // set user session
            HttpContext.Current.Session["AuthorizedUser"] = user;

            UserGamblerSync(providerUserKey);
        }

        #region Casino Gambler Sync

        private static int UserGamblerSync(object providerUserKey)
        {
            Contract.Requires(providerUserKey != null);

            object gamblerKey = null;

            var user = Single(providerUserKey);

            if (user.AcnID > 0 && !string.IsNullOrEmpty(user.AcnUserName))
            {
                IRepository repo = new Repository();

                var query = repo.Query<Gambler>(x => x.UserID == user.AcnID);

                if (query != null && query.Count > 0)
                {
                    HttpContext.Current.Session["Gambler"] = query[0];

                    gamblerKey = query[0].ID;
                }
                else
                {
                    // Create new gambler instance
                    var gambler = new Gambler();
                    if (user.AcnID != null) gambler.UserID = user.AcnID.Value;
                    gambler.UserName = user.AcnUserName.Trim();
                    gambler.Cash = 1000f; // initial fund
                    gambler.TotalBet = 0f;
                    gambler.Win = 0;
                    gambler.Lose = 0;
                    gambler.RPBonus = null;
                    gambler.ContestRank = null;
                    gambler.TotalRank = 0;
                    gambler.Banker = null;
                    gambler.JoinDate = DateTime.Now;
                    gambler.IsActive = true;
                    gambler.Description = string.Empty;
                    gambler.Remark = "Mobile Sync";

                    repo.Insert(gambler, out gamblerKey);
                }
            }

            return gamblerKey != null ? Convert.ToInt32(gamblerKey) : 0;
        }

        #endregion
    }

    public class UserWeChatDto
    {
        public static UserWeChat Authorize(Guid userGuid, string accessToken, double expiresIn, string refreshToken,
            string openId, ScopeType scope, bool anonymous = false)
        {
            using (var conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    // 保存微信用户
                    var user = anonymous ? repo.Single<User>(userGuid) : UserDto.GetSession();

                    if (user != null && user.ID == userGuid)
                    {
                        var u = new UserWeChat();

                        if (repo.Any<UserWeChat>(userGuid))
                        {
                            u = repo.Single<UserWeChat>(userGuid);
                        }

                        u.ID = userGuid;
                        u.UserName = user.UserName;
                        u.LastAuthorizeDate = DateTime.Now;

                        u.AccessToken = accessToken;
                        u.AccessTokenExpiredDate = DateTime.Now.AddSeconds(expiresIn);
                        u.RefreshToken = refreshToken;
                        u.RefreshTokenExpiredDate = DateTime.Now.AddDays(30);

                        u.Gender = 0;

                        if (u.Province == null) u.Province = string.Empty;
                        if (u.City == null) u.City = string.Empty;
                        if (u.Country == null) u.Country = string.Empty;
                        if (u.HeadImgUrl == null) u.HeadImgUrl = string.Empty;
                        if (u.Privilege == null) u.Privilege = string.Empty;
                        if (u.UnionID == null) u.UnionID = string.Empty;

                        repo.Save(u, trans);

                        // 更新普通用户
                        user.WeChatOpenID = openId;

                        // 按scope，获取微信用户详情
                        if (scope.Equals(ScopeType.snsapi_userinfo))
                        {
                            var result = new WeChatSnsClient().GetUserInfo(accessToken, openId);

                            if (!string.IsNullOrEmpty(result))
                            {
                                var json = JToken.Parse(result);

                                user.WeChatNickName = json["nickname"].Value<string>();

                                u.Gender = json["sex"].Value<short>();
                                u.Province = json["province"].Value<string>();
                                u.City = json["city"].Value<string>();
                                u.Country = json["country"].Value<string>();
                                u.HeadImgUrl = json["headimgurl"].Value<string>();
                                u.Privilege = json["privilege"].Value<JArray>().ToString();
                                u.UnionID = json["unionid"] != null ? json["unionid"].Value<string>() : string.Empty;

                                repo.Update(u, trans);
                            }
                        }

                        // 更新user的openId, nickname
                        repo.Update(user, trans);

                        trans.Commit();

                        return u;
                    }

                    return null;
                }
                catch
                {
                    trans.Rollback();

                    throw;
                }
            }
        }
    }

    public class MyAvatarDto
    {
        public IEnumerable<UserDto> Avatars { get; set; }
    }

    public class UserProfileDto
    {
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(20, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "请填写{0}")]
        [DataType(DataType.Password, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(100, ErrorMessage = "{0}长度至少需要{2}位", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "新密码与确认密码不一致")]
        [DataType(DataType.Password, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "请填写{0}")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住密码")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "请填写{0}")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请正确填写{0}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(100, ErrorMessage = "{0}长度至少需要{2}位", MinimumLength = 7)]
        [DataType(DataType.Password, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "新密码与确认密码不一致")]
        [DataType(DataType.Password, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "密码确认")]
        public string ConfirmPassword { get; set; }
    }
}