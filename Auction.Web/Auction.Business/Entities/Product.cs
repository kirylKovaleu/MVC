using System;

namespace Auction.Business.Entities
{
    public class Product
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
