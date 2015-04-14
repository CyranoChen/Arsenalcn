using System;

namespace iArsenal.Entity
{
    public class TravelOption
    {
        public MatchOption MatchOption { get; set; }
        public bool IsFlight { get; set; }
        public bool IsHotel { get; set; }
        public bool IsTraining { get; set; }
        public bool IsParty { get; set; }
        public bool IsSingapore { get; set; }
    }

    public enum MatchOption
    {
        First = 1,
        Second = 2,
        All = 0
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
