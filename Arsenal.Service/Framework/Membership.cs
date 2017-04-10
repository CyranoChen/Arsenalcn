using System;
using System.Data.SqlTypes;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service
{
    [DbSchema("Arsenalcn_Membership", Key = "UserGuid", Sort = "CreateDate DESC")]
    public class Membership : Entity<Guid>
    {
        public void Default()
        {
            var defaultMinDate = Convert.ToDateTime(SqlDateTime.MinValue.ToString());

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
            LastPasswordChangedDate = DateTime.Now;
            LastLockoutDate = defaultMinDate;
            FailedPasswordAttemptCount = 0;
            FailedPasswordAttemptWindowStart = defaultMinDate;
            FailedPasswordAnswerAttemptCount = 0;
            FailedPasswordAnswerAttemptWindowsStart = defaultMinDate;
            Remark = string.Empty;
        }

        public object SignIn()
        {
            IRepository repo = new Repository();

            LastLoginDate = DateTime.Now;
            repo.Update(this);

            return ID;
        }

        #region Members and Properties

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("Password")]
        public string Password { get; set; }

        [DbColumn("PasswordFormat")]
        public int PasswordFormat { get; set; }

        [DbColumn("PasswordSalt")]
        public string PasswordSalt { get; set; }

        [DbColumn("Mobile")]
        public string Mobile { get; set; }

        [DbColumn("Email")]
        public string Email { get; set; }

        [DbColumn("PasswordQuestion")]
        public string PasswordQuestion { get; set; }

        [DbColumn("PasswordAnswer")]
        public string PasswordAnswer { get; set; }

        [DbColumn("IsApproved")]
        public bool IsApproved { get; set; }

        [DbColumn("IsLockedOut")]
        public bool IsLockedOut { get; set; }

        [DbColumn("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DbColumn("LastLoginDate")]
        public DateTime LastLoginDate { get; set; }

        [DbColumn("LastPasswordChangedDate")]
        public DateTime LastPasswordChangedDate { get; set; }

        [DbColumn("LastLockoutDate")]
        public DateTime LastLockoutDate { get; set; }

        [DbColumn("FailedPasswordAttemptCount")]
        public int FailedPasswordAttemptCount { get; set; }

        [DbColumn("FailedPasswordAttemptWindowStart")]
        public DateTime FailedPasswordAttemptWindowStart { get; set; }

        [DbColumn("FailedPasswordAnswerAttemptCount")]
        public int FailedPasswordAnswerAttemptCount { get; set; }

        [DbColumn("FailedPasswordAnswerAttemptWindowsStart")]
        public DateTime FailedPasswordAnswerAttemptWindowsStart { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        #endregion
    }
}