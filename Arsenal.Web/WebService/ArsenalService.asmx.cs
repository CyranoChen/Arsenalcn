using System;
using System.Collections.Generic;
using System.Web.Services;

using Arsenal.Entity;

namespace Arsenal.WebService
{
    [WebService(Namespace = "http://www.arsenal.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ServiceArsenal : System.Web.Services.WebService
    {
        [WebMethod(Description = "Arsenal.Entity.Match.Cache.MatchList")]
        public List<Match> GetMatchs()
        {
            return Arsenal.Entity.Match.Cache.MatchList;
        }

        [WebMethod(Description = "Arsenal.Entity.Player.Cache.PlayerList")]
        public List<Player> GetPlayers()
        {
            return Arsenal.Entity.Player.Cache.PlayerList;
        }

        [WebMethod(Description = "Arsenal.Entity.Team.Cache.TeamList")]
        public List<Team> GetTeams()
        {
            return Arsenal.Entity.Team.Cache.TeamList;
        }

        [WebMethod(Description = "Arsenal.Entity.Team.Cache.GetTeamsByLeagueGuid(Guid guid)")]
        public List<Team> GetTeamsByLeagueGuid(Guid guid)
        {
            return Arsenal.Entity.Team.Cache.GetTeamsByLeagueGuid(guid);
        }

        [WebMethod(Description = "Arsenal.Entity.League.Cache.LeagueList")]
        public List<League> GetLeagues()
        {
            return Arsenal.Entity.League.Cache.LeagueList;
        }

        [WebMethod(Description = "Arsenal.Entity.Video.Cache.VideoList")]
        public List<Video> GetVideos()
        {
            return Arsenal.Entity.Video.Cache.VideoList;
        }
    }
}
