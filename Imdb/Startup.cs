using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Imdb.Startup))]
namespace Imdb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
