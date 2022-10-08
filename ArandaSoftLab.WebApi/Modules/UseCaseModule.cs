using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.WebApi.Modules
{
    public class UseCaseModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load("ArandaSoftLab.Core.UseCase"))
             .Where(t => t.Name.EndsWith("Request"))
             .AsImplementedInterfaces()
             .AsSelf()
            .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("ArandaSoftLab.Core.UseCase"))
             .Where(t => t.Name.EndsWith("Interactor"))
             .AsImplementedInterfaces()
             .AsSelf()
             .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.Load("ArandaSoftLab.Core.UseCase"))
            .Where(t => t.Name.EndsWith("Validator"))
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerLifetimeScope();

      

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return c.TryResolve(t, out o) ? o : null;
                };
            }).InstancePerLifetimeScope();

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            }).InstancePerLifetimeScope();
        }
    }
}