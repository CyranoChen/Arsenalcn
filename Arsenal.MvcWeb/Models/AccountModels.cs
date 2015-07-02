using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using Membership = Arsenal.Service.Membership;
using System.Diagnostics.Contracts;

namespace Arsenal.MvcWeb.Models
{
    public class MembershipDto : Membership
    {
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
        public static MembershipDto GetUser(object providerUserKey)
        {
            Contract.Requires(providerUserKey != null);

            IRepository repo = new Repository();

            var user = repo.Single<Membership>(providerUserKey);

            if (user != null)
            { return user as MembershipDto; }
            else
            { return null; }
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

            ht.Add("Username", username);
            ht.Add("Password", Encrypt.getMd5Hash(password));

            var query = repo.Query<Membership>(ht);

            if (query.Count > 0)
            {
                var user = query[0];

                user.LastLoginDate = DateTime.Now;
                repo.Update<Membership>(user);

                providerUserKey = user.ID;
            }
            else
            {
                providerUserKey = null;
            }

            return query.Count > 0;
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
        public static MembershipDto CreateUser(string username, string password, string mobile, string email, out MembershipCreateStatus status)
        {
            using (SqlConnection conn = new SqlConnection(DataAccess.ConnectString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    IRepository repo = new Repository();

                    var user = new MembershipDto();

                    var _defaultMinDate = Convert.ToDateTime(SqlDateTime.MinValue.ToString());

                    user.Username = username;
                    user.Password = Encrypt.getMd5Hash(password);
                    user.PasswordFormat = 1;
                    user.PasswordSalt = string.Empty;
                    user.Mobile = mobile;
                    user.Email = email;
                    user.PasswordQuestion = string.Empty;
                    user.PasswordAnswer = string.Empty;
                    user.IsApproved = true;
                    user.IsLockedOut = false;
                    user.CreateDate = DateTime.Now;
                    user.LastLoginDate = DateTime.Now;
                    user.LastPasswordChangedDate = _defaultMinDate;
                    user.LastLockoutDate = _defaultMinDate;
                    user.FailedPasswordAttemptCount = 0;
                    user.FailedPasswordAttemptWindowStart = _defaultMinDate;
                    user.FailedPasswordAnswerAttemptCount = 0;
                    user.FailedPasswordAnswerAttemptWindowsStart = _defaultMinDate;
                    user.Remark = string.Empty;

                    repo.Insert<MembershipDto>(user, trans);

                    trans.Commit();

                    status = MembershipCreateStatus.Success;

                    return user;
                }
                catch
                {
                    trans.Rollback();

                    status = MembershipCreateStatus.ProviderError;

                    return null;
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
