using Auction.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Interfaces
{
    public interface IBidService
    {
        Bid GetBid(Guid id);
        void AddBid(Bid bid);
        void DeleteBid(Guid id);
        DateTime GetTime(Guid id);
        decimal GetPrice(Guid id);
        IEnumerable<Bid> GetBids();
        void Update(Bid item);
    }
}
