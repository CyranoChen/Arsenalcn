using System;
using System.Collections.Generic;
using System.Web.Services;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.WebService
{
    [WebService(Namespace = "http://www.arsenal.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ServiceArsenal : System.Web.Services.WebService
    {
        [WebMethod(Description = "Arsenal.Service.Match.Cache.MatchList")]
        public List<Match> GetMatchs()
        {
            return Arsenal.Service.Match.Cache.MatchList;
        }

        [WebMethod(Description = "Arsenal.Service.Player.Cache.PlayerList")]
        public List<Player> GetPlayers()
        {
            return Arsenal.Service.Player.Cache.PlayerList;
        }

        [WebMethod(Description = "Arsenal.Service.Team.Cache.TeamList")]
        public List<Team> GetTeams()
        {
            return Arsenal.Service.Team.Cache.TeamList;
        }

        // TODO
        //[WebMethod(Description = "Arsenal.Service.Team.Cache.GetTeamsByLeagueGuid(Guid guid)")]
        //public List<Team> GetTeamsByLeagueGuid(Guid guid)
        //{
        //    return Arsenal.Service.Team.Cache.GetTeamsByLeagueGuid(guid);
        //}

        [WebMethod(Description = "Arsenal.Service.League.Cache.LeagueList")]
        public List<League> GetLeagues()
        {
            return Arsenal.Service.League.Cache.LeagueList;
        }

        [WebMethod(Description = "Arsenal.Service.Video.Cache.VideoList")]
        public List<Video> GetVideos()
        {
            return Arsenal.Service.Video.Cache.VideoList;
        }
    }
}
