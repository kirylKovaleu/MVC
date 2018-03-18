using Auction.Business.Entities;
using AutoMapper;
using System;

namespace Auction.Business.Mappers
{
    [Obsolete]
    public class BusinessMapperSettings : Profile
    {
        [Obsolete]
        protected override void Configure()
        {
            CreateMap<Data.Entities.Product, Product>();
            CreateMap<Product, Data.Entities.Product>();

            CreateMap<UserDTO, Data.Entities.User>();
            CreateMap<Data.Entities.User, UserDTO>();

            CreateMap<Category, Data.Entities.Category>();
            CreateMap<Data.Entities.Category, Category>();

            CreateMap<Bid, Data.Entities.Bid>();
            CreateMap<Data.Entities.Bid, Bid>();
        }
    }
}
