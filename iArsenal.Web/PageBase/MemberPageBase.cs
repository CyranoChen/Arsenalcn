using System;
using System.Collections.Generic;

using Arsenalcn.Common.Utility;
using iArsenal.Entity;

namespace iArsenal.Web
{
    public class MemberPageBase : AcnPageBase
    {
        public int MID { get; set; }

        public string MemberName { get; set; }

        public MemberPeriod CurrentMemberPeriod { get; set; }

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

                    // Set the Current Available Member Period
                    List<MemberPeriod> list = MemberPeriod.GetMemberPeriods(this.MID).FindAll(mp => mp.IsActive);

                    if (list != null & list.Count > 0)
                    {
                        this.CurrentMemberPeriod = list.Find(mp => mp.StartDate <= DateTime.Now && mp.EndDate >= DateTime.Now);
                    }
                    else
                    {
                        this.CurrentMemberPeriod = null;
                    }
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