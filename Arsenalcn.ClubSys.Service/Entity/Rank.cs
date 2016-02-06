using System.Data;

namespace Arsenalcn.ClubSys.Entity
{
    public class Rank : ClubSysObject
    {
        public Rank()
        {
        }

        public Rank(DataRow dr)
            : base(dr)
        {
        }

        #region Members and Properties

        /// <summary>
        ///     Rank Level ID
        /// </summary>
        [ClubSysDbColumn("RankLevelID")]
        public int ID { get; set; }

        /// <summary>
        ///     Rank Level Max Club Fortune
        /// </summary>
        [ClubSysDbColumn("MaxClubFortune")]
        public int MaxClubFortune { get; set; }

        /// <summary>
        ///     Rank Level Max Member
        /// </summary>
        [ClubSysDbColumn("MaxMember")]
        public int MaxMember { get; set; }

        /// <summary>
        ///     Rank Level Max Executor
        /// </summary>
        [ClubSysDbColumn("MaxExecutor")]
        public int MaxExecutor { get; set; }

        /// <summary>
        ///     member credit evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberCreditRankEvaluateValue")]
        public double MemberCreditRankEvaluateValue { get; set; }

        /// <summary>
        ///     member fortune evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberFortuneRankEvaluateValue")]
        public double MemberFortuneRankEvaluateValue { get; set; }

        /// <summary>
        ///     member rp evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberRPRankEvaluateValue")]
        public double MemberRPRankEvaluateValue { get; set; }

        /// <summary>
        ///     member loyalty evaluate value
        /// </summary>
        [ClubSysDbColumn("MemberLoyaltyRankEvaluateValue")]
        public double MemberLoyaltyRankEvaluateValue { get; set; }

        #endregion
    }
}