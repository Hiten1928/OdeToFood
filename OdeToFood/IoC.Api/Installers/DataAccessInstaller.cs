using System.Data.Entity;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Repositories;

namespace OdeToFood.IoC.Installers
{
    public class DataAccessInstaller : IWindsorInstaller 
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<DbContext>().ImplementedBy<OdeToFoodContext>().LifestyleSingleton());
//            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifestyleTransient());
//
//
//            container.Register(Component.For<DataContext>().LifestylePerWebRequest());
//            container.Register(Component.For<RestaurantRepository>().LifestylePerWebRequest());
//            container.Register(Component.For<RestaurantReviewRepository>().LifestylePerWebRequest());
//            container.Register(Component.For<OrderRepository>().LifestylePerWebRequest());
        }
    }
}