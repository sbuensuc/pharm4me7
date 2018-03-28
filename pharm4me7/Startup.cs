using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pharm4me7.Startup))]
namespace pharm4me7
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
