using Application.WebApi.Modules;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;

namespace Application.WebApi.App_Start
{
    public class AutoFacConfig
    {
        protected AutoFacConfig() { }

        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new EfModule());

            builder.RegisterModule(new UseCaseModule());

            builder.RegisterModule(new ContractsModule());

            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}