using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Finance101.Startup))]
namespace Finance101
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
