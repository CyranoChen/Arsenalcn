using System.Text;
using System.Web;

namespace Arsenalcn.Core.Utility
{
    public static class BrowserInfo
    {
        public static string GetBrowser()
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var bc = HttpContext.Current.Request.Browser;

            var retValue = new StringBuilder();

            retValue.Append(bc.Type);

            if (!bc.MobileDeviceManufacturer.ToLower().Equals("unknown"))
            {
                retValue.AppendFormat(", {0}", bc.MobileDeviceManufacturer);
            }

            if (!bc.MobileDeviceModel.ToLower().Equals("unknown"))
            {
                retValue.AppendFormat(", {0}", bc.MobileDeviceModel);
            }

            return retValue.ToString();
        }
    }
}