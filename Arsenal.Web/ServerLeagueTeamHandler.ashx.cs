using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Arsenal.Service;

namespace Arsenal.Web
{
    public class ServerLeagueTeamHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText;

            try
            {
                var teams = from t in Team.Cache.TeamList
                            orderby t.TeamEnglishName
                            select new { value = t.ID, label = t.TeamNameInfo };

                if (teams.Any())
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    responseText = jsonSerializer.Serialize(teams);
                }
                else
                {
                    throw new Exception("invalid product code");
                }
            }
            catch (Exception ex)
            {
                responseText = $"{{  \"result\": \"error\", \"error_msg\": \"{ex.Message}\" }}";
            }


            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }

        public bool IsReusable => true;
    }
}