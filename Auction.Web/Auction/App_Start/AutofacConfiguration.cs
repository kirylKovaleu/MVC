using Auction.Business.Autofac;
using Auction.Data.Autofac;
using Auction.Autofac;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Web.Mvc;

namespace Auction
{
    [Obsolete]
    public class AutofacConfiguration
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(Global).Assembly);

            builder.RegisterModule(new AutofacDataModule());

            builder.RegisterModule(new AutofacBusinessModule());

            builder.RegisterModule(new AutofacPresentationModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}