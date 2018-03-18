using Auction.Business.Mappers;
using Auction.Mappers;
using AutoMapper;
using System;

namespace Auction
{
    [Obsolete]
    public class AutoMapperConfigurations
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UIMapperSettings>();
                cfg.AddProfile<BusinessMapperSettings>();
            });
        }
    }
}