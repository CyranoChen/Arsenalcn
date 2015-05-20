using System;
using System.Data;
using System.Linq;
using System.Web;

using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class MemberPageBase : AcnPageBase
    {
        private readonly IRepository repo = new Repository();

        public int MID
        {
            get
            {
                if (Request.Cookies["mid"] != null && !string.IsNullOrEmpty(Request.Cookies["mid"].Value))
                {
                    //already login
                    return int.Parse(Request.Cookies["mid"].Value);
                }
                else
                {
                    return -1;
                }
            }
        }

        public string MemberName
        {
            get
            {
                if (Request.Cookies["member_name"] != null && !string.IsNullOrEmpty(Request.Cookies["member_name"].Value))
                    return HttpUtility.UrlDecode(Request.Cookies["member_name"].Value);
                else
                    return string.Empty;
            }
        }

        public int CurrentMemberPeriodID
        {
            get
            {
                if (Request.Cookies["current_mpid"] != null && !string.IsNullOrEmpty(Request.Cookies["current_mpid"].Value))
                {
                    return int.Parse(Request.Cookies["current_mpid"].Value);
                }
                else
                {
                    return -1;
                }
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
                if (this.MID <= 0)
                {
                    var pcMember = new PropertyCollection();

                    pcMember.Add("AcnID", this.UID);

                    Member m = repo.Query<Member>(pcMember).FirstOrDefault();

                    if (m != null && m.ID > 0)
                    {
                        Response.SetCookie(new HttpCookie("mid", m.ID.ToString()));
                        Response.SetCookie(new HttpCookie("member_name", HttpUtility.UrlEncode(m.Name.ToString())));

                        m.IP = IPLocation.GetIP();
                        m.LastLoginTime = DateTime.Now;

                        repo.Update(m);

                        if (this.CurrentMemberPeriodID < 0)
                        {
                            var pcMemberPeriod = new PropertyCollection();

                            pcMemberPeriod.Add("MemberID", this.MID);
                            pcMemberPeriod.Add("IsActive", true);

                            var mp = repo.Query<MemberPeriod>(pcMemberPeriod).FirstOrDefault(x =>
                                x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);

                            Response.SetCookie(new HttpCookie("current_mpid", mp.ID.ToString()));
                        }
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
            if (this.Master != null && this.Master is iArsenalMaster)
            {
                iArsenalMaster masterPage = this.Master as iArsenalMaster;

                masterPage.MemberID = MID;
                masterPage.MemberName = MemberName;
            }
        }
    }
}