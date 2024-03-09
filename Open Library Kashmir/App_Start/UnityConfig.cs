using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Open_Library_Kashmir.Controllers;
using Open_Library_Kashmir.Models;
using System.Web.Mvc;
//using Unity;
//using Unity.Injection;
//using Unity.Lifetime;
//using Unity.Mvc5;

namespace Open_Library_Kashmir
{
    public static class UnityConfig
    {
   //     public static void RegisterTypes(IUnityContainer container)
   //     {
   //         // Register ApplicationDbContext
   //         container.RegisterType<ApplicationDbContext>();

   //         // Register ApplicationUserManager
   //         container.RegisterType<ApplicationUserManager>();

   //         container.RegisterType<ApplicationSignInManager>();

   //         container.RegisterType<ApplicationRoleManager>();

   //         //// Register AccountController
   //         //container.RegisterType<AccountController>();

   //         //container.RegisterType<DonationController>();
   //     }

   //     public static void RegisterComponents()
   //     {
			//var container = new UnityContainer();

   //         // register all your components with the container here
   //         // it is NOT necessary to register your controllers

   //         // e.g. container.RegisterType<ITestService, TestService>();
   //         // ... other registrations ...

   //         // Register AutoMapper
   //         //container.RegisterInstance(AutoMapperConfig.Initialize());

   //         //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
   //         //container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
   //         //container.RegisterType<SignInManager<ApplicationUser, string>>(new HierarchicalLifetimeManager());

   //         //container.RegisterType<AccountController>(new InjectionConstructor());
   //         //container.RegisterType<DonationController>(new InjectionConstructor());

   //         RegisterTypes(container);

   //         DependencyResolver.SetResolver(new UnityDependencyResolver(container));
   //     }
    }
}