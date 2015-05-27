using System;

using Arsenalcn.Core;
using Arsenalcn.Core.Scheduler;
using iArsenal.Service;

namespace iArsenal.Scheduler
{
    class RefreshCache : ISchedule
    {
        public void Execute(object state)
        {
            Config.Cache.RefreshCache();

            Arsenal_Match.Cache.RefreshCache();
            Arsenal_Player.Cache.RefreshCache();
            Arsenal_Team.Cache.RefreshCache();

            MatchTicket.Cache.RefreshCache();
            Member.Cache.RefreshCache();
            Product.Cache.RefreshCache();
        }
    }
}