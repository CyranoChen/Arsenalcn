using System;
using System.Linq;
using System.Web;
using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class MemberPageBase : AcnPageBase
    {
        private readonly IRepository _repo = new Repository();

        protected int MID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Cookies["mid"]?.Value))
                {
                    //already login
                    return int.Parse(Request.Cookies["mid"].Value);
                }

                return -1;
            }
        }

        protected string MemberName
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Cookies["member_name"]?.Value))
                {
                    return HttpUtility.UrlDecode(Request.Cookies["member_name"].Value);
                }

                return string.Empty;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            //_adminPage = false;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (UID > 0)
            {
                // TODO: change to cache mode, LOGOUT
                if (MID <= 0)
                {
                    var m = _repo.Query<Member>(x => x.AcnID == UID).FirstOrDefault();

                    if (m != null && m.ID > 0)
                    {
                        Response.SetCookie(new HttpCookie("mid", m.ID.ToString()));
                        Response.SetCookie(new HttpCookie("member_name", HttpUtility.UrlEncode(m.Name)));

                        m.IP = IPLocation.GetIP();
                        m.LastLoginTime = DateTime.Now;

                        _repo.Update(m);
                    }
                    else
                    {
                        Response.Clear();
                        Response.Redirect("iArsenalMemberRegister.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }

            //Set Master Page Info
            if (Master != null && Master is iArsenalMaster)
            {
                var masterPage = Master as iArsenalMaster;

                masterPage.MemberID = MID;
                masterPage.MemberName = MemberName;
            }
        }
    }
}