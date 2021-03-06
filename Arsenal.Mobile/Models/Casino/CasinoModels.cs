﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arsenal.Service;
using Arsenal.Service.Casino;
using Arsenalcn.Core;

namespace Arsenal.Mobile.Models.Casino
{
    public class IndexDto
    {
        public int CasinoValidDays { get; set; }

        public IEnumerable<MatchDto> Matches { get; set; }

        public Gambler Gambler { get; set; }
    }

    public class GameBetDto
    {
        public double MyCash { get; set; }

        public IEnumerable<BetDto> MyBets { get; set; }

        public IEnumerable<MatchDto> HistoryMatches { get; set; }

        public IEnumerable<object> Messages { get; set; }

        public MatchDto Match { get; set; }
    }

    public class MyCouponDto
    {
        public int CasinoValidDays { get; set; }

        public bool IsShowSubmitButton { get; set; }

        public IEnumerable<CouponDto> Coupons { get; set; }
    }

    public class MyBetDto : Searchable<BetDto>
    {
    }

    public class MyBonusDto : Searchable<BonusDto>
    {
    }

    public class ResultDto : Searchable<MatchDto>
    {
    }

    public class ContestDto
    {
        public League ContestLeague { get; set; }

        public int[] RankCondition { get; set; }

        public IEnumerable<GamblerDW> UpperGamblers { get; set; }

        public IEnumerable<GamblerDW> LowerGamblers { get; set; }
    }

    public class DetailDto
    {
        public IEnumerable<BetDto> Bets { get; set; }

        public MatchDto Match { get; set; }
    }

    public class MatchResultDto
    {
        [Required(ErrorMessage = "请填写{0}")]
        [Range(0, 20, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "主队比分")]
        public short ResultHome { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [Range(0, 20, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "客队比分")]
        public short ResultAway { get; set; }

        [Required]
        public Guid MatchGuid { get; set; }

        //public double CurrentCash { get; set; }

        public MatchDto Match { get; set; }
    }

    public class SingleChoiceDto
    {
        [Required(ErrorMessage = "请选择{0}")]
        [Domain("home", "away", "draw")]
        [Display(Name = "投注选项")]
        public string SelectedOption { get; set; }

        [Required(ErrorMessage = "请填写{0}")]
        [DataType(DataType.Currency, ErrorMessage = "请正确填写{0}")]
        [Range(10f, float.MaxValue, ErrorMessage = "请正确填写{0}")]
        [Display(Name = "投注金额")]
        public double BetAmount { get; set; }

        [Required]
        public Guid MatchGuid { get; set; }

        public double MyCash { get; set; }

        public double BetLimit { get; set; }

        public MatchDto Match { get; set; }
    }
}