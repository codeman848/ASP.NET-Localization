using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OEC06.Startup))]
namespace OEC06
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
