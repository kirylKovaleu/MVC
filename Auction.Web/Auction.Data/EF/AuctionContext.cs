using Auction.Data.Entities;
using System.Data.Entity;

namespace Auction.Data.EF
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(string path) : base(path) { }

        public virtual DbSet<Bid> Bid { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Product> Product { get; set; }
    }
}
