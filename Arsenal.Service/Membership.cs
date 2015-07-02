﻿using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    [DbTable("Arsenalcn_Membership", Key = "MembershipGuid", Sort = "CreateDate DESC")]
    public class Membership : Entity<Guid>
    {
        public Membership() : base() { }

        public Membership(DataRow dr) : base(dr) { }

        #region Members and Properties

        [DbColumn("Username")]
        public string Username
        { get; set; }

        [DbColumn("Password")]
        public string Password
        { get; set; }

        [DbColumn("PasswordFormat")]
        public int PasswordFormat
        { get; set; }

        [DbColumn("PasswordSalt")]
        public string PasswordSalt
        { get; set; }

        [DbColumn("Mobile")]
        public string Mobile
        { get; set; }

        [DbColumn("Email")]
        public string Email
        { get; set; }

        [DbColumn("PasswordQuestion")]
        public string PasswordQuestion
        { get; set; }

        [DbColumn("PasswordAnswer")]
        public string PasswordAnswer
        { get; set; }

        [DbColumn("IsApproved")]
        public bool IsApproved
        { get; set; }

        [DbColumn("IsLockedOut")]
        public bool IsLockedOut
        { get; set; }

        [DbColumn("CreateDate")]
        public DateTime CreateDate
        { get; set; }

        [DbColumn("LastLoginDate")]
        public DateTime LastLoginDate
        { get; set; }

        [DbColumn("LastPasswordChangedDate")]
        public DateTime LastPasswordChangedDate
        { get; set; }

        [DbColumn("LastLockoutDate")]
        public DateTime LastLockoutDate
        { get; set; }

        [DbColumn("FailedPasswordAttemptCount")]
        public int FailedPasswordAttemptCount
        { get; set; }

        [DbColumn("FailedPasswordAttemptWindowStart")]
        public DateTime FailedPasswordAttemptWindowStart
        { get; set; }

        [DbColumn("FailedPasswordAnswerAttemptCount")]
        public int FailedPasswordAnswerAttemptCount
        { get; set; }

        [DbColumn("FailedPasswordAnswerAttemptWindowsStart")]
        public DateTime FailedPasswordAnswerAttemptWindowsStart
        { get; set; }

        [DbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}
