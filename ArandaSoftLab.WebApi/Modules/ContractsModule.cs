using ArandaSoftLab.Infrastructure.Data.Base;
using Autofac;
using Core.UseCase.Base;

namespace Application.WebApi.Modules
{
    public class ContractsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType(typeof(IExceptionLogger)).As(typeof(UnhandledExceptionLogger)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();

            //hangFire
        }
    }
}