using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OdeToFood.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace OdeToFood
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
