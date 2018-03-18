using Auction.Business.Services.Implementations;
using Auction.Business.Services.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<BidService>().As<IBidService>();
            builder.RegisterType<RoleService>().As<IRoles>();

            base.Load(builder);
        }
    }
}
