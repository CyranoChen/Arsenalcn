using System;
using System.Data;
using System.Linq;

using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Web
{
    public class MemberPageBase : AcnPageBase
    {
        private readonly IRepository repo = new Repository();

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
                if (this.MID <= 0)
                {
                    Member m = Member.Cache.LoadByAcnID(this.UID);

                    if (m != null && m.ID > 0)
                    {
                        this.MID = m.ID;
                        this.MemberName = m.Name;

                        m.IP = IPLocation.GetIP();
                        m.LastLoginTime = DateTime.Now;

                        repo.Update(m);
                    }

                    if (this.CurrentMemberPeriod == null)
                    {
                        // TODO: change to cache mode
                        var pc = new PropertyCollection();

                        pc.Add("MemberID", this.MID);
                        pc.Add("IsActive", true);

                        this.CurrentMemberPeriod = repo.Query<MemberPeriod>(pc).First(x =>
                            x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);
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