using System.Configuration;

using iArsenal.Entity.Arsenal;

namespace iArsenal.Entity.ServiceProvider
{
    public class RemoteServiceProvider
    {
        public static ServiceArsenal GetWebService()
        {
            ServiceArsenal svc = new ServiceArsenal();

            svc.Url = ConfigurationManager.AppSettings["Arsenalcn.WebService.Url"].ToString();            

            return svc;
        }
    }
}
