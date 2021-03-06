﻿using System;
using System.Web;
using System.Web.UI;
using iArsenal.Service;

namespace iArsenal.Web.Control
{
    public partial class DNTHeader : UserControl
    {
        private int _userId = -1;

        private string _userKey = string.Empty;


        private string _userName = string.Empty;

        /// <summary>
        ///     Current User Name
        /// </summary>
        public string UserName
        {
            set { _userName = value; }
        }

        /// <summary>
        ///     Current User ID
        /// </summary>
        public int UserID
        {
            set { _userId = value; }
        }

        /// <summary>
        ///     Current User Key
        /// </summary>
        public string UserKey
        {
            set { _userKey = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userId > 0)
            {
                phAnonymous.Visible = false;
                phAthenticated.Visible = true;

                ltrlUserName.Text = _userName;
                //linkLogout.NavigateUrl = string.Format("{0}{1}", linkLogout.NavigateUrl, this._userKey);
            }
            else
            {
                phAnonymous.Visible = true;
                phAthenticated.Visible = false;

                hlLogin.NavigateUrl =
                    $"{ConfigGlobal.APILoginURL}?api_key={ConfigGlobal.APIAppKey}&next={Request.Url.PathAndQuery}";
            }

            ltrlTitle.Text =
                $"<a href=\"http://www.arsenalcn.com\">{"阿森纳中国官方球迷会"}</a> &raquo; <a href=\"default.aspx\">{ConfigGlobal.PluginDisplayName}</a> &raquo; <strong>{Page.Title}</strong>";
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
                        var ts = new TimeSpan(0, 0, 0, 0); //时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts); //立即过期 
                        Response.Cookies.Remove("session_key"); //清除 
                        Response.Cookies.Add(mycookie); //写入立即过期的*/
                        Response.Cookies["session_key"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["uid"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["uid"];
                        var ts = new TimeSpan(0, 0, 0, 0); //时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts); //立即过期 
                        Response.Cookies.Remove("uid"); //清除 
                        Response.Cookies.Add(mycookie); //写入立即过期的*/
                        Response.Cookies["uid"].Expires = DateTime.Now.AddDays(-1);
                    }

                    if (Request.Cookies["user_name"] != null)
                    {
                        HttpCookie mycookie;
                        mycookie = Request.Cookies["user_name"];
                        var ts = new TimeSpan(0, 0, 0, 0); //时间跨度 
                        mycookie.Expires = DateTime.Now.Add(ts); //立即过期 
                        Response.Cookies.Remove("user_name"); //清除 
                        Response.Cookies.Add(mycookie); //写入立即过期的*/
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