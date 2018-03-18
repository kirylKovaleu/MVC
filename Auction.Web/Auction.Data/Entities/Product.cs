using Auction.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Data.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Duration { get; set; }//продолжительность
        public string State { get; set; }
        public string Base64Picture { get; set; }
        public decimal StartPrice { get; set; }
        public Guid СategoryId { get; set; }
    }
}
