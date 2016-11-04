using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WepAPI.Startup))]
namespace WepAPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
