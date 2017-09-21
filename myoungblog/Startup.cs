using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myoungblog.Startup))]
namespace myoungblog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
