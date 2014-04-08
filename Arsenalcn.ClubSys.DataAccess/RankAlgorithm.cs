using System;
using System.Collections.Generic;
using System.Text;

using Arsenalcn.ClubSys.Entity;

using ArsenalPlayer = Arsenal.Entity.Team;
using ArsenalVideo = Arsenal.Entity.Video;

namespace Arsenalcn.ClubSys.DataAccess
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
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                float tmp = (float)memberCount / (float)currentRank.MaxMember;

                int returnValue = (int)(Math.Sqrt(tmp * 100) * 10 * (1 + (1 - tmp) / 2.8));

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
                int clubFortune = currentClub.Fortune.Value;
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                float tmp = (float)clubFortune / (float)currentRank.MaxClubFortune;

                int returnValue = (int)(Math.Sqrt(tmp * 100) * 10 * (1 + (1 - tmp) / 3));

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
                int memberCredit = currentClub.MemberCredit.Value;
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                int returnValue = (int)((((float)memberCredit * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberCreditRankEvaluateValue * 100);

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
                int memberFortune = currentClub.MemberFortune.Value;
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                int returnValue = (int)((((float)memberFortune * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberFortuneRankEvaluateValue * 100);

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
                int memberRP = currentClub.MemberRP.Value;
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                int returnValue = (int)((float)memberRP / (float)memberCount / (float)currentRank.MemberRPRankEvaluateValue * 100);

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
                List<ArsenalVideo> list = ArsenalVideo.Cache.VideoList_Legend.FindAll(delegate(ArsenalVideo v)
                {
                    int _gRank = int.MinValue;
                    if (int.TryParse(v.GoalRank, out _gRank))
                        return (_gRank >= 1) && (_gRank <= 3);
                    else
                        return false;
                });
                
                int videoClubHaveGetCount = UserVideo.GetUserVideoByClubID(currentClub.ID.Value).Rows.Count;
                int videoCanGetCount = list.Count;

                int returnValue = (int)(((float)videoClubHaveGetCount / (float)videoCanGetCount) * 100f);

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
                int memberLoyalty = currentClub.MemberLoyalty.Value;
                Rank currentRank = RankLevel.GetInstance().GetRank(currentClub.Fortune.Value);

                int returnValue = (int)((((float)memberLoyalty * ((float)1 + (float)currentRank.ID / (float)20)) / (float)memberCount) / (float)currentRank.MemberLoyaltyRankEvaluateValue * 100);

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
                int memberCountWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberCountWeight;
                int clubFortuneWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_ClubFortuneWeight;
                int memberCreditWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberCreditWeight;
                int memberRPWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberRPWeight;
                int memberEquipmentWeight = Arsenalcn.ClubSys.Entity.ConfigGlobal.SummaryRankPoint_MemberEquipmentWeight;

                return (memberCountWeight * MemberCountRank + clubFortuneWeight * ClubFortuneRank + memberCreditWeight * MemberCreditRank + memberRPWeight * MemberRPRank + memberEquipmentWeight * MemberEquipmentRank) / (memberCountWeight + clubFortuneWeight + memberCreditWeight + memberRPWeight + memberEquipmentWeight);
            }
        }
    }
}
