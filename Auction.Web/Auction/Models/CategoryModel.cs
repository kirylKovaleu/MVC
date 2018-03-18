using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Models
{
    public class CategoryModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "ПолеНужноЗаполнить")]
        [Display(Name = "Название", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "какНибудьПоясни")]
        [Display(Name = "Описание", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Discription { get; set; }
        [Required]
        [Display(Name = "ШагСтавки", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public int Step { get; set; }
    }
}