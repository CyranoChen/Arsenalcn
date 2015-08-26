//using Arsenal.Entity;

//using CasinoMatch = Arsenalcn.CasinoSys.Entity.Match;

namespace Arsenalcn.Event
{
    class CustomAPI
    {
        //public static void UpdateArsenalMatchResult()
        //{
        //    List<Match> list = Match.Cache.MatchList;

        //    if (list != null && list.Count > 0)
        //    {
        //        CasinoMatch cm = new CasinoMatch();

        //        foreach (Match m in list)
        //        {
        //            if (m.CasinoMatchGuid.HasValue)
        //            {
        //                //Casino MatchGuid Bound
        //                cm = new CasinoMatch(m.CasinoMatchGuid.Value);
        //            }
        //            else
        //            {
        //                //new Arsenal Match
        //                cm = CasinoMatch.GetMatchs().Find(delegate(CasinoMatch match)
        //                {
        //                    if (m.IsHome)
        //                    {
        //                        return m.TeamGuid.Equals(match.Away) && m.PlayTime.Equals(match.PlayTime);
        //                    }
        //                    else
        //                    {
        //                        return m.TeamGuid.Equals(match.Home) && m.PlayTime.Equals(match.PlayTime);
        //                    }
        //                });
        //            }

        //            if (cm != null)
        //            {
        //                m.ResultHome = cm.ResultHome;
        //                m.ResultAway = cm.ResultAway;
        //                m.PlayTime = cm.PlayTime;
        //                m.CasinoMatchGuid = cm.MatchGuid;
        //                m.Update();
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }

        //        Match.Cache.RefreshCache();
        //    }
        //    else
        //    {
        //        throw new Exception("No Valid Arsenal Match");
        //    }
        //}
    }
}
