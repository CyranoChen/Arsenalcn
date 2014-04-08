using System;

using Arsenalcn.Common.Entity;

using Discuz.Forum.ScheduledEvents;

namespace Arsenalcn.Event
{
    class RegularEvent : IEvent
    {
        #region IEvent Members

        public void Execute(object state)
        {
            LogEvent.Logging(LogEventType.Success, "Regular Event Start!", string.Empty, string.Empty);

            // AcnClubSys Event
            #region User Club Statistics
            try
            {
                Arsenalcn.ClubSys.DataAccess.UserClubLogic.UserClubStatistics();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnClub)球会统计出错", ex.StackTrace, ex.Message);
            }
            #endregion

            // AcnCasinoSys Event
            #region Active Banker Statistics
            try
            {
                Arsenalcn.CasinoSys.Entity.Banker.ActiveBankerStatistics();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnCasino)庄家统计出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region Clean Object
            try
            {
                Arsenalcn.CasinoSys.Entity.ChoiceOption.CleanNoCasinoItemChoiceOption();
                Arsenalcn.CasinoSys.Entity.Bet.CleanNoCasinoItemBet();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnCasino)投注清理出错", ex.StackTrace, ex.Message);
            }
            #endregion

            LogEvent.Logging(LogEventType.Success, "Regular Event End!", string.Empty, string.Empty);
        }
        
        #endregion
    }
}