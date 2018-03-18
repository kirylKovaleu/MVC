using Auction.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Models
{
    public class AuctionHouseRoleModel
    {
        public Guid UserId { get; set; }
        public Role role { get; set; }
        [Display(Name = "Категория", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public Guid CategoryId { get; set; }
    }
}