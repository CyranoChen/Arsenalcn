using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace Arsenalcn.ClubSys.Entity
{
    public class Rank : ClubSysObject
    {
        #region Members and Properties

        private int _rankID;
        /// <summary>
        /// Rank Level ID
        /// </summary>
        [ClubSysDbColumn("RankLevelID")]
        public int ID
        {
            get
            {
                return _rankID;
            }
            set
            {
                _rankID = value;
            }
        }

        private int _maxClubFortune;
        /// <summary>
        /// Rank Level Max Club Fortune
        /// </summary>
        [ClubSysDbColumn("MaxClubFortune")]
        public int MaxClubFortune
        {
            get
            {
                return _maxClubFortune;
            }
            set
            {
                _maxClubFortune = value;
            }
        }

        private int _maxMember;
        /// <summary>
        /// Rank Level Max Member
        /// </summary>
        [ClubSysDbColumn("MaxMember")]
        public int MaxMember
        {
            get
            {
                return _maxMember;
            }
            set
            {
                _maxMember = value;
            }
        }

        private int _maxExecutor;
        /// <summary>
        /// Rank Level Max Executor
        /// </summary>
        [ClubSysDbColumn("MaxExecutor")]
        public int MaxExecutor
        {
            get
            {
                return _maxExecutor;
            }
            set
            {
                _maxExecutor = value;
            }
        }

        private double _memberCreditRankEvaluateValue;
        /// <summary>
        /// member credit evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberCreditRankEvaluateValue")]
        public double MemberCreditRankEvaluateValue
        {
            get
            {
                return _memberCreditRankEvaluateValue;
            }
            set
            {
                _memberCreditRankEvaluateValue = value;
            }
        }

        private double _memberFortuneRankEvaluateValue;
        /// <summary>
        /// member fortune evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberFortuneRankEvaluateValue")]
        public double MemberFortuneRankEvaluateValue
        {
            get
            {
                return _memberFortuneRankEvaluateValue;
            }
            set
            {
                _memberFortuneRankEvaluateValue = value;
            }
        }

        private double _memberRPRankEvaluateValue;
        /// <summary>
        /// member rp evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberRPRankEvaluateValue")]
        public double MemberRPRankEvaluateValue
        {
            get
            {
                return _memberRPRankEvaluateValue;
            }
            set
            {
                _memberRPRankEvaluateValue = value;
            }
        }

        private double _memberLoyaltyRankEvaluateValue;
        /// <summary>
        /// member loyalty evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberLoyaltyRankEvaluateValue")]
        public double MemberLoyaltyRankEvaluateValue
        {
            get
            {
                return _memberLoyaltyRankEvaluateValue;
            }
            set
            {
                _memberLoyaltyRankEvaluateValue = value;
            }
        }

        #endregion

        public Rank()
        {
        }

        public Rank(DataRow dr)
            : base(dr)
        {
        }
    }
}
