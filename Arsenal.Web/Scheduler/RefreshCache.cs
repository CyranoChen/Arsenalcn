using System;

using Arsenalcn.Core;
using Arsenalcn.Core.Scheduler;
using Arsenal.Service;

namespace Arsenal.Scheduler
{
    class RefreshCache : ISchedule
    {
        public void Execute(object state)
        {
            Config.Cache.RefreshCache();

            League.Cache.RefreshCache();
            Match.Cache.RefreshCache();
            Player.Cache.RefreshCache();
            Team.Cache.RefreshCache();
            Video.Cache.RefreshCache();

            RelationLeagueTeam.Clean();
        }
    }
}