﻿using System.Web.ApplicationServices;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace OdeToFood.IoC.Installers
{
    public class ServiceInstaller : IWindsorInstaller 
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            
        }
    }
}