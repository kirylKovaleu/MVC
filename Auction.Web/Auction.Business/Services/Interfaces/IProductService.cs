using Auction.Business.Entities;
using System;
using System.Collections.Generic;

namespace Auction.Business.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        void AddProduct(Product product);
        Product product(Guid id);
        void RemoveProduct(Guid id);
        decimal GetPrice(Guid id);
        void Update(Product item);
    }
}
