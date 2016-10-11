using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eus.UI.Startup))]
namespace eus.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
