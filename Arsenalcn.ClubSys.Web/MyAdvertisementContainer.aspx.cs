using System;
using Discuz.Forum;

namespace Arsenalcn.ClubSys.Web
{
    public partial class MyAdvertisementContainer : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //管理人员或100积分以上会员看不到广告
            if (useradminid > 0 || usergroupid > 12)
            {
                var str = "<div class=\"acn_adv\"></div>";

                Response.Write($"document.write('{str}');");

                Response.Write("jQuery(\"div.acn_adv\").parent().hide();");
            }
            else
            {
                //尊龙国际 广告 2014/08/28

                var adv_text =
                    "<a href=\"http://zl699.com/33.html\" target=\"_blank\"><img src=\"/images/adv/zl500.gif\" alt=\"尊龙国际\" /></a>";

                var str =
                    $"<div class=\"acn_adv\" style=\"width: 960px; height: 90px; overflow:hidden; margin: 5px auto\">{adv_text}</div>";

                Response.Write($"document.write('{str}');");
            }
        }
    }
}