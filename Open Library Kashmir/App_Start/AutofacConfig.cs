//using Autofac.Integration.Mvc;
//using Autofac;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace Open_Library_Kashmir.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            //var builder = new ContainerBuilder();

            //// Register controllers
            //builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// Register other dependencies
            //builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();

            //// Register UserManager and SignInManager
            //builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

            //// Register AutoMapper
            //builder.Register(ctx => AutoMapperConfig.Initialize()).As<IMapper>().SingleInstance();

            //// Build container
            //var container = builder.Build();

            //// Set dependency resolver
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    
    }
}