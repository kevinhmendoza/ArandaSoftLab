using Microsoft.Owin;
using Owin;
using Infrastructure.Initialization;

[assembly: OwinStartup(typeof(Application.WebApi.Startup))]

namespace Application.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            new Inicializaciones().Seeders();


        }
    }
}
