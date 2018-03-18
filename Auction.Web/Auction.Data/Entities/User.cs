using Auction.Data.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Auction.Data.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrionDate { get; set; }
        public string Locale { get; set; }
        public string TZone { get; set; }
        public string Password { get; set; }
        public int[] Roles { get; set; }

    }
}
