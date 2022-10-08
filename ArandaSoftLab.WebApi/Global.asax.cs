using Application.WebApi.App_Start;
using System.Web.Http;

namespace ArandaSoftLab.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {         

            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoFacConfig.ConfigureContainer();


        }
    }
}
