using Microsoft.Owin;
using MVC;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}