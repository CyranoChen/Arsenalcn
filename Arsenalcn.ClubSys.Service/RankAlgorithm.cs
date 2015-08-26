using System;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Service
{
    public class RankAlgorithm
    {
        private Club currentClub = null;
        private int memberCount = -1;

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

                var tmp = (float)memberCount / (float)currentRank.MaxMember;

                var returnValue = (int)(Math.Sqrt(tmp * 100) * 10 * (1 + (1 - tmp) / 2.8));

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int ClubFortuneRank
        {
            get
            {
                var clubFortune = currentClub.Fortune.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var tmp = (float)clubFortune / (float)currentRank.MaxClubFortune;

                var returnValue = (int)(Math.Sqrt(tmp * 100) * 10 * (1 + (1 - tmp) / 3));

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int MemberCreditRank
        {
            get
            {
                var memberCredit = currentClub.MemberCredit.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue = (int)((((float)memberCredit * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberCreditRankEvaluateValue * 100);

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int MemberFortuneRank
        {
            get
            {
                var memberFortune = currentClub.MemberFortune.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue = (int)((((float)memberFortune * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberFortuneRankEvaluateValue * 100);

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int MemberRPRank
        {
            get
            {
                var memberRP = currentClub.MemberRP.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue = (int)((float)memberRP / (float)memberCount / (float)currentRank.MemberRPRankEvaluateValue * 100);

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
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
                    else
                        return false;
                });

                var videoClubHaveGetCount = UserVideo.GetUserVideoByClubID(currentClub.ID.Value).Rows.Count;
                var videoCanGetCount = list.Count;

                var returnValue = (int)(((float)videoClubHaveGetCount / (float)videoCanGetCount) * 100f);

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int MemberLoyaltyRank
        {
            get
            {
                var memberLoyalty = currentClub.MemberLoyalty.Value;
                var currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                var returnValue = (int)((((float)memberLoyalty * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberLoyaltyRankEvaluateValue * 100);

                if (returnValue > 100)
                    return 100;
                else if (returnValue < 0)
                    return 0;
                else
                    return returnValue;
            }
        }

        public int SummaryRankPoint
        {
            get
            {
                var memberCountWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberCountWeight;
                var clubFortuneWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_ClubFortuneWeight;
                var memberCreditWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberCreditWeight;
                var memberRPWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberRPWeight;
                var memberEquipmentWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberEquipmentWeight;

                return (memberCountWeight * MemberCountRank + clubFortuneWeight * ClubFortuneRank + memberCreditWeight * MemberCreditRank + memberRPWeight * MemberRPRank + memberEquipmentWeight * MemberEquipmentRank) / (memberCountWeight + clubFortuneWeight + memberCreditWeight + memberRPWeight + memberEquipmentWeight);
            }
        }
    }
}
