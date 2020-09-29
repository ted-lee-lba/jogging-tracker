using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JoggingTracker.Startup))]
namespace JoggingTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
