using Autofac.Integration.Mvc;
using Autofac;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using Open_Library_Kashmir.Models;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Open_Library_Kashmir.App_Start;

[assembly: OwinStartupAttribute(typeof(Open_Library_Kashmir.Startup))]
namespace Open_Library_Kashmir
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Register Autofac DI
            IContainer container =  AutofacConfig.RegisterDependencies(app);

            // OWIN MVC configuration
            ConfigureAuth(app);

            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}
