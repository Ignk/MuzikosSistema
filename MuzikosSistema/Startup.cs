using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MuzikosSistema.Startup))]
namespace MuzikosSistema
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
