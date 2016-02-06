using System;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Service
{
    public class RankAlgorithm
    {
        private readonly Club currentClub;
        private readonly int memberCount = -1;

        public RankAlgorithm(Club club)
        {
            currentClub = club;
            memberCount = ClubLogic.GetClubMemberCount(currentClub.ID.Value);
        }

        public int MemberCountRank
        {
            get
            {
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var tmp = memberCount/(float) currentRank.MaxMember;

                var returnValue = (int) (Math.Sqrt(tmp*100)*10*(1 + (1 - tmp)/2.8));

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int ClubFortuneRank
        {
            get
            {
                var clubFortune = currentClub.Fortune.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var tmp = clubFortune/(float) currentRank.MaxClubFortune;

                var returnValue = (int) (Math.Sqrt(tmp*100)*10*(1 + (1 - tmp)/3));

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int MemberCreditRank
        {
            get
            {
                var memberCredit = currentClub.MemberCredit.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue =
                    (int)
                        (((memberCredit*(1 + currentRank.ID/(float) 20))/memberCount)/
                         (float) currentRank.MemberCreditRankEvaluateValue*100);

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int MemberFortuneRank
        {
            get
            {
                var memberFortune = currentClub.MemberFortune.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue =
                    (int)
                        (((memberFortune*(1 + currentRank.ID/(float) 20))/memberCount)/
                         (float) currentRank.MemberFortuneRankEvaluateValue*100);

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int MemberRPRank
        {
            get
            {
                var memberRP = currentClub.MemberRP.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue = (int) (memberRP/(float) memberCount/(float) currentRank.MemberRPRankEvaluateValue*100);

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int MemberEquipmentRank
        {
            get
            {
                var list = Video.Cache.VideoList_Legend.FindAll(x =>
                {
                    var _gRank = int.MinValue;
                    if (int.TryParse(x.GoalRank, out _gRank))
                        return (_gRank >= 1) && (_gRank <= 3);
                    return false;
                });

                var videoClubHaveGetCount = UserVideo.GetUserVideoByClubID(currentClub.ID.Value).Rows.Count;
                var videoCanGetCount = list.Count;

                var returnValue = (int) ((videoClubHaveGetCount/(float) videoCanGetCount)*100f);

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int MemberLoyaltyRank
        {
            get
            {
                var memberLoyalty = currentClub.MemberLoyalty.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue =
                    (int)
                        (((memberLoyalty*(1 + currentRank.ID/(float) 20))/memberCount)/
                         (float) currentRank.MemberLoyaltyRankEvaluateValue*100);

                if (returnValue > 100)
                    return 100;
                if (returnValue < 0)
                    return 0;
                return returnValue;
            }
        }

        public int SummaryRankPoint
        {
            get
            {
                var memberCountWeight = ConfigGlobal.SummaryRankPoint_MemberCountWeight;
                var clubFortuneWeight = ConfigGlobal.SummaryRankPoint_ClubFortuneWeight;
                var memberCreditWeight = ConfigGlobal.SummaryRankPoint_MemberCreditWeight;
                var memberRPWeight = ConfigGlobal.SummaryRankPoint_MemberRPWeight;
                var memberEquipmentWeight = ConfigGlobal.SummaryRankPoint_MemberEquipmentWeight;

                return (memberCountWeight*MemberCountRank + clubFortuneWeight*ClubFortuneRank +
                        memberCreditWeight*MemberCreditRank + memberRPWeight*MemberRPRank +
                        memberEquipmentWeight*MemberEquipmentRank)/
                       (memberCountWeight + clubFortuneWeight + memberCreditWeight + memberRPWeight +
                        memberEquipmentWeight);
            }
        }
    }
}