using System;
using System.Collections.Generic;
using System.Web;

using iArsenal.Entity;

namespace iArsenal.Web.Control
{
    public partial class iArsenalHeader : System.Web.UI.UserControl
    {
        private string _userName = string.Empty;
        /// <summary>
        /// Current User Name
        /// </summary>
        public string UserName
        {
            set
            {
                _userName = value;
            }
        }

        private int _userId = -1;
        /// <summary>
        /// Current User ID
        /// </summary>
        public int UserID
        {
            set
            {
                _userId = value;
            }
        }

        private string _userKey = string.Empty;
        /// <summary>
        /// Current User Key
        /// </summary>
        public string UserKey
        {
            set
            {
                _userKey = value;
            }
        }

        private string _memberName = string.Empty;
        /// <summary>
        /// Current Member Name
        /// </summary>
        public string MemberName
        {
            set
            {
                _memberName = value;
            }
        }

        private int _memberId = -1;
        /// <summary>
        /// Current Member ID
        /// </summary>
        public int MemberID
        {
            set
            {
                _memberId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId > 0)
            {
                pnlAnonymousUser.Visible = false;
                pnlLoginUser.Visible = true;

                Member member = new Member();
                member.Select(_userId);

                if (member != null && member.MemberID > 0)
                {
                    lblUserInfo.Text = string.Format("欢迎访问，<b>{0}</b> (<em>NO.{1}</em>)", member.Name, member.MemberID.ToString());
                }
                else
                {
                    lblUserInfo.Text = string.Format("欢迎访问，<b>{0}</b> (<em>ID.{1}</em>)", _userName, _userId.ToString());
                }

                if (ConfigAdmin.IsPluginAdmin(_userId))
                {
                    ltrlAdminConfig.Text = string.Format("<a href=\"AdminConfig.aspx\" target=\"_blank\">后台管理</a> - ");
                }
                else
                {
                    ltrlAdminConfig.Visible = false;
                }
            }
            else
            {
                pnlAnonymousUser.Visible = true;
                pnlLoginUser.Visible = false;

                hlLogin.NavigateUrl = string.Format("{0}?api_key={1}&next={2}", ConfigGlobal.APILoginURL, ConfigGlobal.APIAppKey, Request.Url.PathAndQuery);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                if (_userId > 0)
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
                }

                Response.Redirect("default.aspx");
            }
            catch
            {
                Response.Redirect("default.aspx");
            }
        }
    }
}