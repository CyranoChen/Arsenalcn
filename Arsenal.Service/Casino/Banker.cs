using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Banker")]
    public class Banker : Entity<Guid>
    {
        public Banker() : base() { }

        #region Members and Properties

        [DbColumn("BankerName")]
        public string BankerName
        { get; set; }

        [DbColumn("ClubID")]
        public int? ClubID
        { get; set; }

        [DbColumn("Cash")]
        public double Cash
        { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive
        { get; set; }

        #endregion

    }
}
