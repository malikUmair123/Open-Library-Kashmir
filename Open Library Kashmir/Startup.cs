using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Open_Library_Kashmir.Startup))]
namespace Open_Library_Kashmir
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
