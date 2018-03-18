using System;
using Auction.Controllers;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Auction.Models
{
    public class ProductModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [ScaffoldColumn(false)]
        public Guid UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "КакНазывать")]
        [Display(Name = "Название", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "СтыдноОписывать")]
        [Display(Name = "Описание", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Discription { get; set; }

        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "ВыставимНавсегда")]
        [Display(Name = "Продолжительность", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public DateTime Duration { get; set; }//продолжительность

        public ProductController.State State { get; set; }
        
        [Display(Name = "Изображение", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Base64Picture { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НезнаешьВоСколькоОценить")]
        [Display(Name = "НачальнаяЦена", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public decimal StartPrice { get; set; }

        [Required]
        public Guid СategoryId { get; set; }
    }
}