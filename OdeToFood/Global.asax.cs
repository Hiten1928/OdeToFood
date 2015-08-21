using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FluentValidation.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using OdeToFood.Data;
using OdeToFood.Ioc;

namespace OdeToFood
{
    public class MvcApplication : System.Web.HttpApplication
    {
        WindsorContainer _windsorContainer = new WindsorContainer();

        protected void Application_Start()
        {
            InitializeWindsor();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FluentValidationModelValidatorProvider.Configure();

            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new WindsorFluentValidatorFactory(_windsorContainer.Kernel));
            Database.SetInitializer<OdeToFoodContext>(null);
        }

        protected void Application_End()
        {
            if (_windsorContainer != null)
            {
                _windsorContainer.Dispose();
            }
        }

        private void InitializeWindsor()
        {
            var _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_windsorContainer.Kernel));
        }
    }
}
