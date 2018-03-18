using Auction.Data.Implements;
using Auction.Data.Interfaces;
using Autofac;

namespace Auction.Data.Autofac
{
    public class AutofacDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonRepository>().Named<IRepository>("JSON");
            builder.RegisterType<EFRepository>().Named<IRepository>("SQL");
            base.Load(builder);
        }
    }
}
