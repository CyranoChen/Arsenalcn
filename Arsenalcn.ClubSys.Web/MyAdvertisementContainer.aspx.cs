using System;

using Discuz.Forum;
using Discuz.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyAdvertisementContainer : Discuz.Forum.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //管理人员或100积分以上会员看不到广告
            if (this.useradminid > 0 || this.usergroupid > 12)
            {
                string str = "<div class=\"acn_adv\"></div>";

                Response.Write(string.Format("document.write('{0}');", str));

                Response.Write("jQuery(\"div.acn_adv\").parent().hide();");

            }
            else
            {
                // 泛亚娱乐 广告过期 2014/07/12

                //string adv_text = "<a href=\"http://www.fy88.com/f/50\" target=\"_blank\"><img src=\"/images/adv/fy88.gif\" alt=\"泛亚娱乐\" /></a>";

                //string str = string.Format("<div class=\"acn_adv\" style=\"width: 960px; height: 60px; overflow:hidden; margin: 5px auto\">{0}</div>", adv_text);

                //Response.Write(string.Format("document.write('{0}');", str));
            }

        }
    }
}