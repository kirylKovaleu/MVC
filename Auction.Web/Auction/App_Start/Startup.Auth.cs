using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.Owin;
using Auction.Models;

namespace Auction
{
    public partial class Startup
    {
            public void ConfigureAuth(IAppBuilder app)
            {

                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/"),
                    Provider = new CookieAuthenticationProvider
                    {
                        OnValidateIdentity = SecurityStampValidator
                        .OnValidateIdentity<UserManager<LoginUserModel, string>, LoginUserModel>(
                            validateInterval: TimeSpan.FromMinutes(10),
                            regenerateIdentity: (manager, user) => manager
                            .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie))
                    }
                });
            }
    }
}
