using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityMvc.Startup))]
namespace IdentityMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
