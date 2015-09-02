using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
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
        private WindsorContainer _windsorContainer;


        protected void Application_Start()
        {
            InitializeWindsor();

            GlobalConfiguration.Configuration.Services.Replace(
            typeof(IHttpControllerActivator),
            new WindsorCompositionRoot(_windsorContainer));

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

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
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_windsorContainer.Kernel));
        }
    }
}
