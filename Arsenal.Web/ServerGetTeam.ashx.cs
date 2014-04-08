using System;
using System.Collections.Generic;
using System.Web;

using Arsenal.Entity;

namespace Arsenal.Web
{
    public class ServerGetTeam : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;
            string xmlContent = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["LeagueID"]))
            {
                //Guid leagueID = new Guid(context.Request.QueryString["LeagueID"]);
                Guid leagueID = new Guid("afa5eb24-0165-4295-90d1-3dde4e559256");
                {
                    List<RelationLeagueTeam> list = RelationLeagueTeam.GetRelationLeagueTeams().FindAll(delegate(RelationLeagueTeam rlt) { return rlt.LeagueGuid.Equals(leagueID); });

                    if (list.Count > 0)
                    {
                        foreach (RelationLeagueTeam rlt in list)
                        {
                            xmlContent += string.Format("<TeamItem ID=\"{0}\" Name=\"{1}\" />", rlt.TeamGuid, Team.Cache.Load(rlt.TeamGuid).TeamDisplayName);
                        }

                        responseText = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><Team>{0}</Team>", xmlContent);
                    }
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