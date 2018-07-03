using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PLC_GenCo.Startup))]
namespace PLC_GenCo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
