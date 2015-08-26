using System.Configuration;

using iArsenal.Service.Arsenal;

namespace iArsenal.Service.ServiceProvider
{
    public class RemoteServiceProvider
    {
        public static ServiceArsenal GetWebService()
        {
            var svc = new ServiceArsenal();

            svc.Url = ConfigurationManager.AppSettings["Arsenalcn.WebService.Url"].ToString();            

            return svc;
        }
    }
}
