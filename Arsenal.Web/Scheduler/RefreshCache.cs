using System;

using Arsenalcn.Core;
using Arsenalcn.Core.Scheduler;

namespace Arsenal.Scheduler
{
    class RefreshCache : ISchedule
    {
        public void Execute(object state)
        {
            Config.Cache.RefreshCache();

            Service.League.Cache.RefreshCache();
            Service.Match.Cache.RefreshCache();
            Service.Player.Cache.RefreshCache();
            Service.Team.Cache.RefreshCache();
            Service.Video.Cache.RefreshCache();

            Service.RelationLeagueTeam.Clean();
        }
    }
}