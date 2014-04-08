using System;
using System.Text.RegularExpressions;

using Arsenalcn.Common.Entity;
using iArsenal.Entity;

namespace iArsenal.Web.PageBase
{
    public class MemberPageBase : AcnPageBase
    {
        public int MID { get; set; }

        public string MemberName { get; set; }

        protected override void OnInitComplete(EventArgs e)
        {
            //_adminPage = false;
            AnonymousRedirect = true;

            base.OnInitComplete(e);

            if (UID > 0)
            {
                Member member = new Member();
                member.Select(UID);

                if (member != null && member.MemberID > 0)
                {
                    this.MID = member.MemberID;
                    this.MemberName = member.Name;

                    member.IP = IPLocation.GetIP();
                    member.LastLoginTime = DateTime.Now;

                    member.Update();
                }
                else
                {
                    Response.Clear();
                    Response.Redirect("iArsenalMemberRegister.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
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