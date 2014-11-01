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
                 //尊龙国际 广告 2014/08/28

                string adv_text = "<a href=\"http://zl699.com/33.html\" target=\"_blank\"><img src=\"/images/adv/zl500.gif\" alt=\"尊龙国际\" /></a>";

                string str = string.Format("<div class=\"acn_adv\" style=\"width: 960px; height: 90px; overflow:hidden; margin: 5px auto\">{0}</div>", adv_text);

                Response.Write(string.Format("document.write('{0}');", str));
            }

        }
    }
}