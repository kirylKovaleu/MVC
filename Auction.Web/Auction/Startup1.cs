using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Auction.Startup))]

namespace Auction
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
