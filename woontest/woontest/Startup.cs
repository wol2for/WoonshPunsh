using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(woontest.Startup))]
namespace woontest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
