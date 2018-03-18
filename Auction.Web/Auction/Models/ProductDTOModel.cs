using System;
using Auction.Controllers;
using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class ProductDTOModel:IComparable<ProductDTOModel>
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Название", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Name { get; set; }
        [Display(Name = "Описание", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Discription { get; set; }
        [Display(Name ="ВремяДоОкончания", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public TimeSpan TheRestOfTime { get; set; }//продолжительность
        [Display(Name = "Статус", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public ProductController.State State { get; set; }
        [Display(Name = "Изображение", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Base64Picture { get; set; }
        [Display(Name = "ТекущаяЦена", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public decimal StartPrice { get; set; }
        [Display(Name = "Категория", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string СategoryName { get; set; }

        public int CompareTo(ProductDTOModel other)
        {
            if (other != null)
            {
                ProductDTOModel product = other as ProductDTOModel;
                return Name.CompareTo(other.Name);
            }
            throw new Exception();
        }
    }
}