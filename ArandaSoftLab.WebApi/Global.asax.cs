using Infrastructure.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ArandaSoftLab.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {         

            GlobalConfiguration.Configure(WebApiConfig.Register);

            new Inicializaciones().Seeders();



        }
    }
}
