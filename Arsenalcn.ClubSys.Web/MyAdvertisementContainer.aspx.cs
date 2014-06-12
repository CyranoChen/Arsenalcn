using System;

using Discuz.Forum;
using Discuz.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyAdvertisementContainer : Discuz.Forum.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "<div style=\"width: 980px; height: 60px; margin: 5px auto\"><a href=\"http://www.fy88.com/f/50\" target=\"_blank\"><img src=\"/images/adv/fy88.gif\" alt=\"泛亚娱乐\" /></a></div>";

            //管理人员或100积分以上会员看不到广告
            if (this.useradminid > 0 || this.usergroupid > 12)
            {
                //str = string.Format("userid: {0} | useradminid: {1} | usergroupid: {2}", userid, useradminid, usergroupid);
                str = string.Empty;
            }

            Response.Write(string.Format("document.write('{0}');", str));
        }
    }
}