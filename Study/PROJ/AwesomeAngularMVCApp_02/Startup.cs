using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AwesomeAngularMVCApp_02.Startup))]
namespace AwesomeAngularMVCApp_02
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
