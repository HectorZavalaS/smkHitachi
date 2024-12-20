using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(smtLocations.Startup))]
namespace smtLocations
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
