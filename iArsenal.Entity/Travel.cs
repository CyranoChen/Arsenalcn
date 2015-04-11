using System;

namespace iArsenal.Entity
{
    public class TravelOption
    {
        public bool IsFlight { get; set; }
        public bool IsHotel { get; set; }
        public bool IsTraining { get; set; }
        public bool IsParty { get; set; }
        public bool IsSingapore { get; set; }
    }

    public class MatchOption
    {
        public DateTime KickOffDate { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }
    }

    public class Partner
    {
        public string Name
        { get; set; }

        public int Relation
        { get; set; }

        public bool Gender
        { get; set; }

        public string IDCardNo
        { get; set; }

        public string PassportNo
        { get; set; }

        public string PassportName
        { get; set; }
    }
}
