using Auction.Business.Entities;
using Auction.Business.Services.Interfaces;
using Auction.Data.Interfaces;
using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Implementations
{
    public class BidService : IBidService
    {
        private string _path;
        IRepository _repository;
        private readonly IComponentContext _context;

        public BidService(string type, string path, IComponentContext context)
        {
            if (type == "JSON")
            {
                _context = context;
                _repository = _context.ResolveNamed<IRepository>(type, new NamedParameter("path", path));
            }
            else
            {
                _context = context;
                _repository = _context.ResolveNamed<IRepository>(type, new NamedParameter("path", path));
            }
        }

        public void AddBid(Bid bid)
        {
            bid.Id = Guid.NewGuid();
            _repository.Add(Mapper.Map<Data.Entities.Bid>(bid));
        }

        public Bid GetBid(Guid id)
        {
            var bid = _repository.Get<Data.Entities.Bid>(id);
            Bid b = Mapper.Map<Bid>(bid);
            return b;
        }

        public void DeleteBid(Guid id)
        {
            _repository.Delete<Data.Entities.Bid>(id);
        }

        public decimal GetPrice(Guid id)
        {
            decimal price = _repository.Get<Data.Entities.Bid>(id).Price;
            return price;
        }

        public DateTime GetTime(Guid id)
        {
            DateTime time = _repository.Get<Data.Entities.Bid>(id).Time;
            return time;
        }

        public IEnumerable<Bid> GetBids()
        {
            var bids = _repository.GetAll<Data.Entities.Bid>();
            return Mapper.Map<List<Bid>>(bids);
        }

        public void Update(Bid item)
        {
            var bid = Mapper.Map<Data.Entities.Bid>(item);
            _repository.Update(bid);
        }
    }
}
