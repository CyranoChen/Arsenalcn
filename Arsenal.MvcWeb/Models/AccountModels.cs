using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using Membership = Arsenal.Service.Membership;

namespace Arsenal.MvcWeb.Models
{
    public class MembershipDto : Membership
    {
        public MembershipDto() : base() { }

        private void Init()
        {
            var _defaultMinDate = Convert.ToDateTime(SqlDateTime.MinValue.ToString());

            UserName = string.Empty;
            Password = string.Empty;
            PasswordFormat = 1;
            PasswordSalt = string.Empty;
            Mobile = string.Empty;
            Email = string.Empty;
            PasswordQuestion = string.Empty;
            PasswordAnswer = string.Empty;
            IsApproved = true;
            IsLockedOut = false;
            CreateDate = DateTime.Now;
            LastLoginDate = DateTime.Now;
            LastPasswordChangedDate = _defaultMinDate;
            LastLockoutDate = _defaultMinDate;
            FailedPasswordAttemptCount = 0;
            FailedPasswordAttemptWindowStart = _defaultMinDate;
            FailedPasswordAnswerAttemptCount = 0;
            FailedPasswordAnswerAttemptWindowsStart = _defaultMinDate;
            Remark = string.Empty;
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
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            //TODO
            return false;
        }

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
        public static MembershipDto GetMembership(object providerUserKey)
        {
            Contract.Requires(providerUserKey != null);

            IRepository repo = new Repository();

            var user = repo.Single<Membership>(providerUserKey);

            if (user != null)
            { return user as MembershipDto; }
            else
            { return null; }
        }

        public static User GetUser(object providerUserKey)
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
            else
            {
                return null;
            }
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
        public static bool ValidateUser(string username, string password, out object providerUserKey)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));

            IRepository repo = new Repository();

            var ht = new Hashtable();

            ht.Add("UserName", username);
            ht.Add("Password", Encrypt.getMd5Hash(password));

            var query = repo.Query<Membership>(ht);

            if (query.Count > 0)
            {
                var user = query[0];

                providerUserKey = user.ID;

                user.LastLoginDate = DateTime.Now;
                repo.Update<Membership>(user);
            }
            else
            {
                providerUserKey = null;
            }

            return query.Count > 0;
        }

        public static bool ValidateAcnUser(string username, string password)
        {
            if (!ConfigGlobal.AcnSync) { return false; }

            var client = new DiscuzApiClient();

            var uid = client.AuthValidate(username, Encrypt.getMd5Hash(password));

            return Convert.ToInt32(uid.Replace("\"", "")) > 0;
        }

        public static int GetAcnID(string username)
        {
            var client = new DiscuzApiClient();

            var uid = client.UsersGetID(username);

            return Convert.ToInt32(uid.Replace("\"", ""));
        }

        public void AcnSyncRegister(int uid, out object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.UserRejected;
            providerUserKey = null;

            if (!ConfigGlobal.AcnSync) { return; }

            #region Get Acn UserInfo to init the intance of MembershipDto
            var client = new DiscuzApiClient();

            int[] uids = { uid };
            string[] fields = { "user_name", "password", "email", "mobile", "join_date" };

            var responseResult = client.UsersGetInfo(uids, fields);

            if (string.IsNullOrEmpty(responseResult)) { return; }

            JArray jlist = JArray.Parse(responseResult);
            JToken json = jlist[0];

            this.Init();

            this.UserName = json["user_name"].ToString();
            this.Password = json["password"].ToString();
            this.Mobile = json["mobile"].ToString();
            this.Email = json["email"].ToString();
            this.CreateDate = Convert.ToDateTime(json["join_date"].ToString());
            this.Remark = string.Format("{{\"AcnID\": {0}}}", uid);
            #endregion

            using (SqlConnection conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    repo.Insert<MembershipDto>(this, out providerUserKey, trans);

                    var user = new User();

                    user.ID = (Guid)providerUserKey;
                    user.UserName = this.UserName;
                    user.IsAnonymous = false;
                    user.LastActivityDate = DateTime.Now;
                    user.AcnID = uid;
                    user.AcnUserName = this.UserName;
                    user.MemberID = null;
                    user.MemberName = string.Empty;
                    user.WeChatOpenID = null;
                    user.WeChatNickName = string.Empty;

                    repo.Insert<User>(user, trans);

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
        public void CreateUser(string username, string password, string mobile, string email, out object providerUserKey, out MembershipCreateStatus status)
        {
            using (SqlConnection conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    this.Init();


                    // TODO
                    this.UserName = username;
                    this.Password = Encrypt.getMd5Hash(password);
                    this.Mobile = mobile;
                    this.Email = email;

                    repo.Insert<MembershipDto>(this, out providerUserKey, trans);

                    var user = new User();

                    user.ID = (Guid)providerUserKey;
                    user.UserName = this.UserName;
                    user.IsAnonymous = false;
                    user.LastActivityDate = DateTime.Now;

                    #region Register Acn User
                    if (ConfigGlobal.AcnSync)
                    {
                        var client = new DiscuzApiClient();

                        var uid = client.AuthRegister(this.UserName, this.Password, this.Email);

                        user.AcnID = Convert.ToInt32(uid.Replace("\"", ""));
                        user.AcnUserName = this.UserName;
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

                    repo.Insert<User>(user, trans);

                    trans.Commit();

                    status = MembershipCreateStatus.Success;
                }
                catch
                {
                    trans.Rollback();

                    providerUserKey = null;

                    status = MembershipCreateStatus.ProviderError;
                }
            }
        }
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
