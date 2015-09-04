using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WebApiBlog.Core;

namespace OdeToFood.IoC.Installers
{
    public class WebWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
//            container.Register(Classes
//                .FromAssembly(typeof(ContainerApplication).Assembly)
//                .BasedOn<IRestApiController>()
//                .LifestyleScoped());

            container.Register(Classes
                .FromAssembly(typeof (ContainerApplication).Assembly)
                .BasedOn<ApiController>()
                .LifestylePerWebRequest());
        }
    }
}