//using Autofac.Integration.Mvc;
//using Autofac;
using Open_Library_Kashmir.Models;
using System.Web.Mvc;
using AutoMapper;
using Autofac.Integration.Mvc;
using Autofac;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Open_Library_Kashmir.App_Start
{
    public class AutofacConfig
    {
        public static IContainer RegisterDependencies(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //// OPTIONAL: Register model binders that require DI.
            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            //builder.RegisterModelBinderProvider();

            //// OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            //// OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            //// OPTIONAL: Enable property injection into action filters.
            //builder.RegisterFilterProvider();

            //// OPTIONAL: Enable action method parameter injection (RARE).
            ////builder.InjectActionInvoker();

            // Register other dependencies
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Register AutoMapper
            builder.Register(ctx => AutoMapperConfig.Initialize()).As<IMapper>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    
    }
}