using Auction.Business.Entities;
using Auction.Filters;
using Auction.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;

namespace Auction.Mappers
{
    public class UIMapperSettings : Profile
    {
        [Obsolete]
        protected override void Configure()
        {
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();

            CreateMap<ProductModel, ProductDTOModel>();
            CreateMap<ProductDTOModel, ProductModel>();

            CreateMap<Product, ProductDTOModel>();
            CreateMap<ProductDTOModel, Product>();

            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();

            CreateMap<BidModel, Bid>();
            CreateMap<Bid, BidModel>();

            CreateMap<LoginUserModel, UserDTO>();
            CreateMap<UserDTO, LoginUserModel>();

            CreateMap<LoginUserModel, UserModel>();
            CreateMap<UserModel, LoginUserModel>();

            CreateMap<RegisterUserModel, UserDTO>();
            CreateMap<UserDTO, RegisterUserModel>();

            CreateMap<UserModel, UserDTO>();
            CreateMap<UserDTO, UserModel>();

            CreateMap<IUserStore<LoginUserModel, string>, CustomUserStore>();
            CreateMap<CustomUserStore, IUserStore<LoginUserModel, string>>();

            CreateMap<RegisterUserModel, LoginUserModel>();
            CreateMap<LoginUserModel, RegisterUserModel>();

        }
    }
}