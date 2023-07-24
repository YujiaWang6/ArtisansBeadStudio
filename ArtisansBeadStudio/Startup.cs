using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtisansBeadStudio.Startup))]
namespace ArtisansBeadStudio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
