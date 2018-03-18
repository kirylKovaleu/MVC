using System;
using System.Globalization;

namespace Auction.Business.Entities
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Role role { get; set; }
        public DateTime RegistrionDate { get; set; }
        public string Locale { get; set; }
        public string TZone { get; set; }
    }
}
