using System.Configuration;

using Arsenalcn.ClubSys.Service.Arsenal;

namespace Arsenal.Entity.ServiceProvider
{
    public static class RemoteServiceProvider
    {
        public static ServiceArsenal GetWebService()
        {
            ServiceArsenal svc = new ServiceArsenal();

            svc.Url = ConfigurationManager.AppSettings["Arsenalcn.WebService.Url"].ToString();

            return svc;
        }
    }
}
