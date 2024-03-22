//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
//using Microsoft.Owin.Security;
//using Open_Library_Kashmir.Models;
//using System.Web.Mvc;
//using Unity;
//using Unity.Injection;
//using Unity.Lifetime;
//using Unity.Mvc5;
//using System.Web;
//using Open_Library_Kashmir.Controllers;
//using System.Web.UI.WebControls;
//using System.Data.Entity;
//using AutoMapper;

//namespace Open_Library_Kashmir
//{
//    public static class UnityConfig
//    {
//        public static void RegisterComponents()
//        {
//            var container = new UnityContainer();

//            // Create a new MapperConfiguration
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<ApplicationUser, RecipientViewModel>();
//            });

//            // Register IMapper instance
//            container.RegisterInstance(config.CreateMapper());

//            //perhaps in Startup.Auth.cs need to register components instead of creating OwinContext
//            #region OWIN
//            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
//            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
//            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
//            container.RegisterFactory<IAuthenticationManager>(
//                c => HttpContext.Current.GetOwinContext().Authentication);
//            container.RegisterType<ApplicationSignInManager>(new HierarchicalLifetimeManager());
//            container.RegisterType<ApplicationRoleManager>(new HierarchicalLifetimeManager());
//            #endregion

//            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

//        }
//    }
//}