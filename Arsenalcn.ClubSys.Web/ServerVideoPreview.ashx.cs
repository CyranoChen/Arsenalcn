using System;
using System.Web;

using Arsenalcn.ClubSys.Service;

namespace Arsenalcn.ClubSys.Web
{
    public class ServerVideoPreview : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["VideoGuid"]))
            {
                try
                {
                    var guid = new Guid(context.Request.QueryString["VideoGuid"]);

                    var _jsonVideo = string.Empty;
                    var _jsonGoalPlayer = string.Empty;
                    var _jsonAssistPlayer = string.Empty;
                    var _jsonMatch = string.Empty;

                    var v = Video.Cache.Load(guid);

                    if (v != null)
                    {
                        _jsonVideo =
                            $"{{ \"VideoGuid\": \"{v.ID.ToString()}\", \"VideoFilePath\": \"{v.VideoFilePath}\", \"VideoType\": \"{v.VideoType.ToString()}\", \"VideoWidth\": \"{v.VideoWidth.ToString()}\",  \"VideoHeight\": \"{v.VideoHeight.ToString()}\", \"GoalRank\": \"{v.GoalRank.ToString()}\", \"TeamworkRank\": \"{v.TeamworkRank.ToString()}\" }}";

                        if (v.GoalPlayerGuid.HasValue)
                        {
                            var pg = Player.Cache.Load(v.GoalPlayerGuid.Value);
                            _jsonGoalPlayer =
                                $"{{ \"PlayerGuid\": \"{pg.ID.ToString()}\", \"DisplayName\": \"{pg.DisplayName}\", \"PhotoURL\": \"{pg.PhotoURL}\" }}";
                        }

                        if (v.AssistPlayerGuid.HasValue)
                        {
                            var pa = Player.Cache.Load(v.AssistPlayerGuid.Value);
                            _jsonAssistPlayer =
                                $"{{ \"PlayerGuid\": \"{pa.ID.ToString()}\", \"DisplayName\": \"{pa.DisplayName}\", \"PhotoURL\": \"{pa.PhotoURL}\" }}";
                        }

                        if (v.ArsenalMatchGuid.HasValue)
                        {
                            var m = Match.Cache.Load(v.ArsenalMatchGuid.Value);
                            var to = Team.Cache.Load(m.TeamGuid);
                            var ta = Team.Cache.Load(Entity.ConfigGlobal.ArsenalTeamGuid);

                            var _strMatchInfo = "{{ \"MatchGuid\": \"{0}\", \"HomeTeam\": \"{1}\", \"HomeTeamLogo\": \"{2}\", \"ResultHome\": \"{3}\", \"ResultAway\": \"{4}\", \"PlayTime\": \"{5}\", \"AwayTeam\": \"{6}\", \"AwayTeamLogo\": \"{7}\" }}";
                            var _strAcnCasinoPath = "/plugin/AcnCasino/";

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

                        responseText =
                            $"{{ \"Video\": {_jsonVideo}, \"GoalPlayer\": {(!string.IsNullOrEmpty(_jsonGoalPlayer) ? _jsonGoalPlayer : "\"\"")}, \"AssistPlayer\": {(!string.IsNullOrEmpty(_jsonAssistPlayer) ? _jsonAssistPlayer : "\"\"")}, \"Match\": {(!string.IsNullOrEmpty(_jsonMatch) ? _jsonMatch : "\"\"")} }}";
                    }
                    else
                    {
                        throw new Exception("invalid Arsenal Video");
                    }
                }
                catch (Exception ex)
                {
                    responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
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