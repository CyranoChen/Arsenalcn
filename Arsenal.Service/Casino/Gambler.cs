using System;
using System.Data;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Gambler", Sort = "ID DESC")]
    public class Gambler : Entity<int>
    {
        public Gambler() : base() { }

        #region Members and Properties

        [DbColumn("UserID")]
        public int UserID
        { get; set; }

        [DbColumn("UserName")]
        public string UserName
        { get; set; }

        [DbColumn("Cash")]
        public double Cash
        { get; set; }

        [DbColumn("TotalBet")]
        public double TotalBet
        { get; set; }

        [DbColumn("Win")]
        public int Win
        { get; set; }

        [DbColumn("Lose")]
        public int Lose
        { get; set; }

        [DbColumn("RPBonus")]
        public int? RPBonus
        { get; set; }

        [DbColumn("ContestRank")]
        public int? ContestRank
        { get; set; }

        [DbColumn("TotalRank")]
        public int TotalRank
        { get; set; }

        [DbColumn("Banker")]
        public double? Banker
        { get; set; }

        [DbColumn("JoinDate")]
        public DateTime JoinDate
        { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        [DbColumn("Description")]
        public string Description
        { get; set; }

        [DbColumn("Remark")]
        public string Remark
        { get; set; }

        #endregion
    }
}
