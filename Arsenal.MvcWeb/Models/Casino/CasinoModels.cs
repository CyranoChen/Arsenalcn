using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Arsenalcn.Core;
using Arsenal.Service.Casino;

namespace Arsenal.MvcWeb.Models.Casino
{
    public class IndexDto
    {
        public int CasinoValidDays { get; set; }

        public IEnumerable<MatchDto> Matches { get; set; }
    }

    public class GameBetDto
    {
        public double MyCash { get; set; }

        public IEnumerable<BetDto> MyBets { get; set; }

        public IEnumerable<MatchDto> HistoryMatches { get; set; }

        public IEnumerable<object> Messages { get; set; }

        public MatchDto Match { get; set; }
    }

    public class MyBetDto : SearchModel<BetDto> { }
    public class MyBonusDto : SearchModel<BetDto> { }
    public class ResultDto : SearchModel<MatchDto> { }

    public class DetailDto
    {
        public IEnumerable<BetDto> Bets { get; set; }

        public MatchDto Match { get; set; }
    }

    public class MatchResultDto
    {
        [Required]
        [Range(0, 10)]
        public short ResultHome { get; set; }

        [Required]
        [Range(0, 10)]
        public short ResultAway { get; set; }

        [Required]
        public Guid MatchGuid { get; set; }

        public double MyCash { get; set; }

        public MatchDto Match { get; set; }
    }

    public class SingleChoiceDto
    {
        [Required]
        [Domain("home", "away", "draw")]
        [Display(Name = "投注选项")]
        public string SelectedOption { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(10f, float.MaxValue)]
        [Display(Name = "投注金额")]
        public double BetAmount { get; set; }

        [Required]
        public Guid MatchGuid { get; set; }

        public double MyCash { get; set; }

        public double BetLimit { get; set; }

        public MatchDto Match { get; set; }
    }
}