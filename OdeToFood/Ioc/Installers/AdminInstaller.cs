#region Using statemetns

using System.Data.Entity;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

#endregion

namespace OdeToFood.Ioc.Installers
{
    public class AdminInstaller : IWindsorInstaller
    {
        private const string WebAssembletyName = "OdeToFood";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed(WebAssembletyName)
                .BasedOn<IController>()
                .LifestyleTransient()
                .Configure(x => x.Named(x.Implementation.FullName)));

            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<WindsorControllerFactory>(),
                Component.For<IRepository<BaseEntity>>().ImplementedBy<Repository<BaseEntity>>());


            container.Register(Component.For<DbContext>().ImplementedBy<OdeToFoodContext>().LifestyleSingleton());
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifestyleTransient());


            container.Register(Component.For<DataContext>().LifestylePerWebRequest());
            container.Register(Component.For<RestaurantRepository>().LifestylePerWebRequest());
            container.Register(Component.For<RestaurantReviewRepository>().LifestylePerWebRequest());
            container.Register(Component.For<OrderRepository>().LifestylePerWebRequest());
            container.Register(Component.For<TableRepository>().LifestylePerWebRequest());
           
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

        }
    }
}