using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebRanker.WebMVC.Startup))]
namespace WebRanker.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
