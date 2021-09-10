using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopApp.WebUI.Startup))]
namespace ShopApp.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
