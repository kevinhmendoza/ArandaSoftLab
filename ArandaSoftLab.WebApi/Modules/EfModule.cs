using Autofac;
using ArandaSoftLab.Infrastructure.Data.Base;
using Core.UseCase.Base;
using ArandaSoftLab.Infrastructure.Audit.Base;
using ArandaSoftLab.Infrastructure.Audit;
using ArandaSoftLab.Infrastructure.Data;

namespace Application.WebApi.Modules
{
    public class EfModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterType(typeof(ArandaSoftLabContext)).As(typeof(IDbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(ArandaSoftLabAuditLogContext)).As(typeof(IDbContextAudit)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerRequest();
        }
    }
}