using System;
using System.Web;
using System.Web.UI;

using iArsenal.Service;

namespace iArsenal.Web
{
    public partial class _Default : AcnPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["method"])
                    && Request.QueryString["method"].Equals("logout", StringComparison.OrdinalIgnoreCase))
                {
                    if (Request.Cookies["session_key"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["session_key"];
                        TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                        Response.Cookies.Remove("session_key");//清除 
                        Response.Cookies.Add(mycookie);//写入立即过期的*/
                        Response.Cookies["session_key"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["uid"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["uid"];
                        TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                        Response.Cookies.Remove("uid");//清除 
                        Response.Cookies.Add(mycookie);//写入立即过期的*/
                        Response.Cookies["uid"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["user_name"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["user_name"];
                        TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                        Response.Cookies.Remove("user_name");//清除 
                        Response.Cookies.Add(mycookie);//写入立即过期的*/
                        Response.Cookies["user_name"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["mid"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["mid"];
                        TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                        Response.Cookies.Remove("mid");//清除 
                        Response.Cookies.Add(mycookie);//写入立即过期的*/
                        Response.Cookies["mid"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["member_name"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["member_name"];
                        TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                        Response.Cookies.Remove("member_name");//清除 
                        Response.Cookies.Add(mycookie);//写入立即过期的*/
                        Response.Cookies["member_name"].Expires = DateTime.Now.AddDays(-1);
                    }

                    Response.Redirect("/default.aspx");
                }
            }
            catch
            {
                Response.Redirect("/default.aspx");
            }
        }
    }
}
