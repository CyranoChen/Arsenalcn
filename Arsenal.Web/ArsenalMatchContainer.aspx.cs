using System;
using System.Collections.Generic;
using Arsenal.Service;

namespace Arsenal.Web
{
    public partial class ArsenalMatchContainer : AcnPageBase
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                var lstBefore = Match.Cache.MatchList.FindAll(x => x.PlayTime < DateTime.Now);
                var lstAfter = Match.Cache.MatchList.FindAll(x => x.PlayTime > DateTime.Now);

                lstBefore.Sort((m1, m2) => Comparer<DateTime>.Default.Compare(m2.PlayTime, m1.PlayTime));
                lstAfter.Sort((m1, m2) => Comparer<DateTime>.Default.Compare(m1.PlayTime, m2.PlayTime));

                var strMatchInfoPrev =
                    "<div class=\"GameItemList\" style=\"padding-right:6px;border-right:1px dashed #ccc;\"><div class=\"Arsenal_CategoryName\"><a href=\"plugin/acncasino/CasinoGame.aspx?League={0}\" target=\"_blank\">{1}</a></div>";
                strMatchInfoPrev +=
                    "<div class=\"Arsenal_GameName\">{2}</div><div class=\"Arsenal_GameTime\">{3} <a href=\"plugin/acncasino/CasinoBetLog.aspx?Match={4}\" target=\"_blank\" style=\"display:{5}\">more...</a></div></div>";

                var strMatchInfoNext =
                    "<div class=\"GameItemList\"><div class=\"Arsenal_CategoryName\"><a href=\"plugin/acncasino/CasinoGame.aspx?League={0}\" target=\"_blank\">{1}</a></div>";
                strMatchInfoNext +=
                    "<div class=\"Arsenal_GameName\">{2}</div><div class=\"Arsenal_GameTime\">{3} <a href=\"plugin/acncasino/CasinoGameBet.aspx?Match={4}\" target=\"_blank\" style=\"display:{5}\">more...</a></div></div>";

                var strMatchTeamInfo =
                    "<a class=\"StrongLink\" href=\"plugin/acncasino/CasinoTeam.aspx?Team={6}\" title=\"{0}\" target=\"_blank\">{1}</a> <img src=\"plugin/acncasino/{2}\" />";
                strMatchTeamInfo +=
                    "<a href=\"plugin/acncasino/CasinoTeam.aspx?Match={8}\" title=\"查看历史交战记录\" target=\"_blank\"><em>vs</em></a>";
                strMatchTeamInfo +=
                    "<img src=\"plugin/acncasino/{3}\" /> <a class=\"StrongLink\" href=\"plugin/acncasino/CasinoTeam.aspx?Team={7}\" title=\"{4}\" target=\"_blank\">{5}</a>";

                string strTeamInfo;
                string strLeagueInfo;
                var strMatch1 = string.Empty;
                var strMatch2 = string.Empty;
                var strMatch3 = string.Empty;
                var teamArsenal = Team.Cache.Load(ConfigGlobal_Arsenal.ArsenalTeamGuid);
                Team tHome, tAway;

                // Output First Match Before DateTime.Now
                if (lstBefore.Count > 0)
                {
                    var m1 = lstBefore[0];
                    var t = Team.Cache.Load(m1.TeamGuid);

                    if (m1.Round.HasValue)
                    {
                        strLeagueInfo = $"{m1.LeagueName} 第{m1.Round.Value}轮";
                    }
                    else
                    {
                        strLeagueInfo = m1.LeagueName;
                    }

                    if (m1.IsHome)
                    {
                        tHome = teamArsenal;
                        tAway = t;
                    }
                    else
                    {
                        tHome = t;
                        tAway = teamArsenal;
                    }

                    strTeamInfo = string.Format(strMatchTeamInfo, tHome.TeamEnglishName, tHome.TeamDisplayName,
                        tHome.TeamLogo, tAway.TeamLogo, tAway.TeamEnglishName, tAway.TeamDisplayName, tHome.ID, tAway.ID,
                        m1.CasinoMatchGuid?.ToString() ?? string.Empty);

                    // Tackle with Match Result
                    if (m1.ResultHome.HasValue && m1.ResultAway.HasValue)
                        strTeamInfo =
                            $"<span style=\"left:0px\">{m1.ResultHome.Value}</span><span style=\"right:0px\">{m1.ResultAway.Value}</span>{strTeamInfo}";

                    strMatch1 = string.Format(strMatchInfoPrev, m1.LeagueGuid.Value, strLeagueInfo, strTeamInfo,
                        m1.PlayTime.ToString("yyyy-MM-dd HH:mm"),
                        m1.CasinoMatchGuid?.ToString() ?? string.Empty, m1.CasinoMatchGuid.HasValue ? "inline" : "none");
                }

                // Output First Match After DateTime.Now
                if (lstAfter.Count > 0)
                {
                    var m2 = lstAfter[0];
                    var t = Team.Cache.Load(m2.TeamGuid);

                    if (m2.Round.HasValue)
                    {
                        strLeagueInfo = $"{m2.LeagueName} 第{m2.Round.Value}轮";
                    }
                    else
                    {
                        strLeagueInfo = m2.LeagueName;
                    }

                    if (m2.IsHome)
                    {
                        tHome = teamArsenal;
                        tAway = t;
                    }
                    else
                    {
                        tHome = t;
                        tAway = teamArsenal;
                    }

                    strTeamInfo = string.Format(strMatchTeamInfo, tHome.TeamEnglishName, tHome.TeamDisplayName,
                        tHome.TeamLogo, tAway.TeamLogo, tAway.TeamEnglishName, tAway.TeamDisplayName, tHome.ID, tAway.ID,
                        m2.CasinoMatchGuid?.ToString() ?? string.Empty);
                    strMatch2 = string.Format(strMatchInfoNext, m2.LeagueGuid.Value, strLeagueInfo, strTeamInfo,
                        m2.PlayTime.ToString("yyyy-MM-dd HH:mm"),
                        m2.CasinoMatchGuid?.ToString() ?? string.Empty, m2.CasinoMatchGuid.HasValue ? "inline" : "none");
                }

                // Output Second Match After DateTime.Now
                if (lstAfter.Count > 1)
                {
                    var m3 = lstAfter[1];
                    var t = Team.Cache.Load(m3.TeamGuid);

                    if (m3.Round.HasValue)
                    {
                        strLeagueInfo = $"{m3.LeagueName} 第{m3.Round.Value}轮";
                    }
                    else
                    {
                        strLeagueInfo = m3.LeagueName;
                    }

                    if (m3.IsHome)
                    {
                        tHome = teamArsenal;
                        tAway = t;
                    }
                    else
                    {
                        tHome = t;
                        tAway = teamArsenal;
                    }

                    strTeamInfo = string.Format(strMatchTeamInfo, tHome.TeamEnglishName, tHome.TeamDisplayName,
                        tHome.TeamLogo, tAway.TeamLogo, tAway.TeamEnglishName, tAway.TeamDisplayName, tHome.ID, tAway.ID,
                        m3.CasinoMatchGuid?.ToString() ?? string.Empty);
                    strMatch3 = string.Format(strMatchInfoNext, m3.LeagueGuid.Value, strLeagueInfo, strTeamInfo,
                        m3.PlayTime.ToString("yyyy-MM-dd HH:mm"),
                        m3.CasinoMatchGuid?.ToString() ?? string.Empty, m3.CasinoMatchGuid.HasValue ? "inline" : "none");
                }

                Response.Write(
                    "document.write('<link href=\"../../App_Themes/Arsenalcn/Arsenal.css\" type=\"text/css\" rel=\"stylesheet\" />');");
                Response.Write(
                    $"document.write('<div class=\"Arsenal_Header\">{strMatch1}{strMatch2}{strMatch3}<div class=\"Clear\"></div>');");
            }
            catch
            {
                Response.Write("document.write('Loading --Arsenal Match Result & Preview-- Error');");
            }
        }
    }
}