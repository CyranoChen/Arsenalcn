using System;
using System.Web;

using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web
{
    public class ServerVideoPreview : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["VideoGuid"]))
            {
                try
                {
                    Guid guid = new Guid(context.Request.QueryString["VideoGuid"]);

                    string _jsonVideo = string.Empty;
                    string _jsonGoalPlayer = string.Empty;
                    string _jsonAssistPlayer = string.Empty;
                    string _jsonMatch = string.Empty;

                    var v = Video.Cache.Load(guid);

                    if (v != null)
                    {
                        _jsonVideo = string.Format("{{ \"VideoGuid\": \"{0}\", \"VideoFilePath\": \"{1}\", \"VideoType\": \"{2}\", \"VideoWidth\": \"{3}\",  \"VideoHeight\": \"{4}\", \"GoalRank\": \"{5}\", \"TeamworkRank\": \"{6}\" }}",
                            v.ID.ToString(), v.VideoFilePath, v.VideoType.ToString(), v.VideoWidth.ToString(), v.VideoHeight.ToString(), v.GoalRank.ToString(), v.TeamworkRank.ToString());

                        if (v.GoalPlayerGuid.HasValue)
                        {
                            var pg = Player.Cache.Load(v.GoalPlayerGuid.Value);
                            _jsonGoalPlayer = string.Format("{{ \"PlayerGuid\": \"{0}\", \"DisplayName\": \"{1}\", \"PhotoURL\": \"{2}\" }}",
                                pg.ID.ToString(), pg.DisplayName, pg.PhotoURL);
                        }

                        if (v.AssistPlayerGuid.HasValue)
                        {
                            var pa = Player.Cache.Load(v.AssistPlayerGuid.Value);
                            _jsonAssistPlayer = string.Format("{{ \"PlayerGuid\": \"{0}\", \"DisplayName\": \"{1}\", \"PhotoURL\": \"{2}\" }}",
                                pa.ID.ToString(), pa.DisplayName, pa.PhotoURL);
                        }

                        if (v.ArsenalMatchGuid.HasValue)
                        {
                            var m = Match.Cache.Load(v.ArsenalMatchGuid.Value);
                            var to = Team.Cache.Load(m.TeamGuid);
                            var ta = Team.Cache.Load(Entity.ConfigGlobal.ArsenalTeamGuid);

                            string _strMatchInfo = "{{ \"MatchGuid\": \"{0}\", \"HomeTeam\": \"{1}\", \"HomeTeamLogo\": \"{2}\", \"ResultHome\": \"{3}\", \"ResultAway\": \"{4}\", \"PlayTime\": \"{5}\", \"AwayTeam\": \"{6}\", \"AwayTeamLogo\": \"{7}\" }}";
                            string _strAcnCasinoPath = "/plugin/AcnCasino/";

                            if (m.IsHome)
                            {
                                _jsonMatch = string.Format(_strMatchInfo,
                                    m.ID.ToString(),
                                    ta.TeamEnglishName,
                                    _strAcnCasinoPath + ta.TeamLogo,
                                    m.ResultHome.Value.ToString(),
                                    m.ResultAway.Value.ToString(),
                                    m.PlayTime.ToString("yyyy/MM/dd HH:mm"),
                                    to.TeamEnglishName,
                                    _strAcnCasinoPath + to.TeamLogo);
                            }
                            else
                            {
                                _jsonMatch = string.Format(_strMatchInfo,
                                    m.ID.ToString(),
                                    to.TeamEnglishName,
                                    _strAcnCasinoPath + to.TeamLogo,
                                    m.ResultHome.Value.ToString(),
                                    m.ResultAway.Value.ToString(),
                                    m.PlayTime.ToString("yyyy/MM/dd HH:mm"),
                                    ta.TeamEnglishName,
                                    _strAcnCasinoPath + ta.TeamLogo);
                            }
                        }

                        responseText = string.Format("{{ \"Video\": {0}, \"GoalPlayer\": {1}, \"AssistPlayer\": {2}, \"Match\": {3} }}",
                            _jsonVideo,
                            !string.IsNullOrEmpty(_jsonGoalPlayer) ? _jsonGoalPlayer : "\"\"",
                            !string.IsNullOrEmpty(_jsonAssistPlayer) ? _jsonAssistPlayer : "\"\"",
                            !string.IsNullOrEmpty(_jsonMatch) ? _jsonMatch : "\"\"");
                    }
                    else
                    {
                        throw new Exception("invalid Arsenal Video");
                    }
                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}