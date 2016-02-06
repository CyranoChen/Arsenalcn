using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using Arsenal.Service;

namespace Arsenal.WebService
{
    [WebService(Namespace = "http://www.arsenal.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ServiceArsenal : System.Web.Services.WebService
    {
        [WebMethod(Description = "Arsenal.Service.Match.Cache.MatchList")]
        public List<Match> GetMatchs()
        {
            return Match.Cache.MatchList;
        }

        [WebMethod(Description = "Arsenal.Service.Player.Cache.PlayerList")]
        public List<Player> GetPlayers()
        {
            return Player.Cache.PlayerList;
        }

        [WebMethod(Description = "Arsenal.Service.Team.Cache.TeamList")]
        public List<Team> GetTeams()
        {
            return Team.Cache.TeamList;
        }

        [WebMethod(Description = "Arsenal.Service.Team.Cache.GetTeamsByLeagueGuid(Guid guid)")]
        public List<Team> GetTeamsByLeagueGuid(Guid guid)
        {
            return Team.Cache.GetTeamsByLeagueGuid(guid);
        }

        [WebMethod(Description = "Arsenal.Service.League.Cache.LeagueList")]
        public List<League> GetLeagues()
        {
            return League.Cache.LeagueList;
        }

        [WebMethod(Description = "Arsenal.Service.Video.Cache.VideoList")]
        public List<Video> GetVideos()
        {
            return Video.Cache.VideoList;
        }
    }
}