using Autofac;
using BlazorAreas.Data;
using BlazorAreas.Data.Entities;
using BlazorAreas.RadzenHelpers.OData;
using BlazorAreas.Services;
using Dependo.Autofac;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;
using Radzen;

namespace BlazorAreas.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ApplicationDbContextFactory>().As<IDbContextFactory>().SingleInstance();

            builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<ODataRegistrar>().As<IODataRegistrar>().SingleInstance();

            // Radzen
            builder.RegisterType<DialogService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TooltipService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ContextMenuService>().AsSelf().InstancePerLifetimeScope();

            // Services
            builder.RegisterType<PersonODataService>().As<IRadzenODataService<Person, int>>().SingleInstance();

            // Not working...
            //builder.RegisterGeneric(typeof(RadzenODataService<,>))
            //    .As(typeof(IRadzenODataService<,>))
            //    .InstancePerLifetimeScope();
        }
    }
}