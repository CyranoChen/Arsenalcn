using System;
using Arsenal.Service.Club;
using AutoMapper;

namespace Arsenal.Mobile.Models.Club
{
    public class LogSignInDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LogSignIn, LogSignInDto>());

            return config;
        }

        #region Members and Properties

        public DateTime SignInTime { get; set; }

        public double Bonus { get; set; }

        public int SignInDays { get; set; }

        public string Description { get; set; }

        #endregion
    }
}