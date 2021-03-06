﻿using Autofac;
using Autofac.Integration.WebApi;
using LicenseManagement.Services.Logger;
using LicenseManagement.Services.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using LicenseManagement.Services.Email;
using System.Web.Mvc;

namespace LicenseManagement.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<LicenseService>().As<ILicenseService>().InstancePerRequest();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}