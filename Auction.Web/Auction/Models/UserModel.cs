using Auction.Business.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Auction.Models
{
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Логин", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НиЧеНеПропустил")]
        [Display(Name = "Пароль", ResourceType = typeof(App_LocalResources.GlobalRes))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НиЧеНеПропустил")]
        [Display(Name = "Пароль", ResourceType = typeof(App_LocalResources.GlobalRes))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Имя", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string LastName { get; set; }

        public DateTime RegistrionDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "КакойЯзык")]
        [Display(Name = "Язык", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Locale { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НуИГдеВЫНаходитесь")]
        [Display(Name = "ЧасовойПояс", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string TZone { get; set; }
    }

    public class RegisterUserModel
    {
        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "ЗарегишсяБумбараш")]
        [Display(Name = "Логин", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НиЧеНеПропустил")]
        [Display(Name = "Пароль", ResourceType = typeof(App_LocalResources.GlobalRes))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "НиЧеНеПропустил")]
        [Display(Name = "Пароль", ResourceType = typeof(App_LocalResources.GlobalRes))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "ПлохоеИмя")]
        [Display(Name = "Имя", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "УкажитеФамилию")]
        [Display(Name = "Фамилия", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string LastName { get; set; }

        public DateTime RegistrionDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "КакойЯзык")]
        [Display(Name = "Язык", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Locale { get; set; }

        //[Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
        //          ErrorMessageResourceName = "НуИГдеВЫНаходитесь")]
        //[Display(Name = "ЧасовойПояс", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string TZone { get; set; }
    }
    

    public class LoginUserModel:IUser<string>
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "Всего2Поля")]
        [Display(Name = "Логин", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Login { get; set; }
        [Required(ErrorMessageResourceType = typeof(App_LocalResources.GlobalRes),
                  ErrorMessageResourceName = "Всего2Поля")]
        [Display(Name = "Пароль", ResourceType = typeof(App_LocalResources.GlobalRes))]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role role { get; set; }
        public DateTime RegistrionDate { get; set; }
        public string Locale { get; set; }
        public string TZone { get; set; }
        public string UserName
        {
            get { return Login; }
            set { Login = value; }
        }

    }
}