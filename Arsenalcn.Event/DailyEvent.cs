using System;

using Arsenalcn.Common.Entity;

using Discuz.Forum.ScheduledEvents;

namespace Arsenalcn.Event
{
    class DailyEvent : IEvent
    {
        #region IEvent Members

        public void Execute(object state)
        {
            LogEvent.Logging(LogEventType.Success, "Daily Event Start!", string.Empty, string.Empty);

            // AcnClubSys Event
            #region Calculator Club Fortune
            try
            {
                Arsenalcn.ClubSys.DataAccess.UserClubLogic.CalcClubFortuneIncrement();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnClub)球会财富日增出错", ex.StackTrace, ex.Message);
            }
            #endregion


            #region Generate LuckyPlayer
            try
            {
                Arsenalcn.ClubSys.DataAccess.LuckyPlayer.GenerateLuckyPlayer();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnClub)幸运球员生成出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region Generate Video on LeftPanel
            try
            {
                Guid? guid = Arsenalcn.ClubSys.DataAccess.UserVideo.SetDailyVideo();

                if (guid.HasValue)
                {
                    Config c = new Config();
                    c.ConfigSystem = ConfigSystem.AcnClub;
                    c.ConfigKey = "DailyVideoGuid";
                    c.ConfigValue = guid.Value.ToString();

                    c.Update();
                    Config.Cache.RefreshCache();
                }
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnClub)集锦随机生成出错", ex.StackTrace, ex.Message);
            }
            #endregion

            // AcnCasinoSys Event
            #region Group Table Statistics
            try
            {
                Arsenalcn.CasinoSys.Entity.Group.ActiveGroupTableStatistics();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnCasino)积分榜统计出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region CasinoItem Statistics
            try
            {
                Arsenalcn.CasinoSys.Entity.CasinoItem.ActiveCasinoItemStatistics();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnCasino)博彩项统计出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region Top Gambler Monthly Statistics
            try
            {
                Arsenalcn.CasinoSys.Entity.Gambler.GamblerStatistics();
                Arsenalcn.CasinoSys.Entity.Gambler.TopGamblerMonthlyStatistics();
                Arsenalcn.CasinoSys.Entity.Gambler.Cache.RefreshCache();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(AcnCasino)玩家统计出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region Clean Object
            try
            {
                Arsenal.Entity.RelationLeagueTeam.CleanRelationLeagueTeam();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(Arsenal)分类与球队关系清理出错", ex.StackTrace, ex.Message);
            }
            #endregion

            #region Update Arsenal Match Result
            try
            {
                CustomAPI.UpdateArsenalMatchResult();
            }
            catch (Exception ex)
            {
                LogEvent.Logging(LogEventType.Error, "(Arsenal)更新阿森纳比赛结果出错", ex.StackTrace, ex.Message);
            }
            #endregion

            LogEvent.Logging(LogEventType.Success, "Daily Event End!", string.Empty, string.Empty);

        }
        #endregion
    }
}