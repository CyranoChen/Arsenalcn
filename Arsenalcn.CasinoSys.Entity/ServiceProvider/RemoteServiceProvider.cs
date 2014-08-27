﻿using System.Configuration;

using Arsenalcn.CasinoSys.Entity.Arsenal;

namespace Arsenal.Entity.ServiceProvider
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
