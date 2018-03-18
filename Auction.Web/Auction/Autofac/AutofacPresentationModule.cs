using Auction.Filters;
using Auction.Models;
using Autofac;
using Microsoft.AspNet.Identity;

namespace Auction.Autofac
{
    public class AutofacPresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomUserStore>().As <IUserStore<LoginUserModel>>();
            base.Load(builder);
        }
    }
}