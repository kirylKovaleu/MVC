using Auction.Business.Entities;
using Auction.Business.Services.Interfaces;
using Auction.Data.Interfaces;
using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace Auction.Business.Services.Implementations
{
    public class ProductService : IProductService
    {
        IRepository _repository;
        private readonly IComponentContext _context;

        public ProductService(string type, string path, IComponentContext context)
        {
                _context = context;
                _repository = _context.ResolveNamed<IRepository>(type, new NamedParameter("path", path));
           
        }

        public IEnumerable<Product> GetProducts()
        {
            var tempProducs = _repository.GetAll<Data.Entities.Product>();
            return Mapper.Map<List<Product>>(tempProducs);
        }

        public void AddProduct(Product product)
        {
            if (product != null)
            {
                product.Id = Guid.NewGuid();
                _repository.Add(Mapper.Map<Data.Entities.Product>(product));
            }
        }

        public Product product(Guid id)
        {
            var p = _repository.Get<Data.Entities.Product>(id);
            p.StartDate = DateTime.Now;
            Product z = Mapper.Map<Product>(p);
            return z;
        }

        public void RemoveProduct(Guid id)
        {
            _repository.Delete<Data.Entities.Product>(id);
        }

        public decimal GetPrice(Guid id)
        {
            var product = _repository.Get<Data.Entities.Product>(id);
            return product.StartPrice;
        }

        public void Update(Product item)
        {
            var product = Mapper.Map<Data.Entities.Product>(item);
            _repository.Update(product);
        }
    }
}
