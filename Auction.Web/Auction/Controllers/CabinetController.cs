using Auction.Business.Services.Interfaces;
using Auction.Filters;
using Auction.Models;
using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Auction.Controllers.ProductController;

namespace Auction.Controllers
{
    [Culture]
    public class CabinetController : Controller
    {
        IUserService _userService;
        IProductService _productService;
        IRoles _rolesService;
        IBidService _bidService;
        ICategoryService _categoryService;
        string _path;
        IComponentContext _context;

        public CabinetController(IComponentContext context)
        {
            _path = ConfigurationManager.AppSettings["JsonRepositoryPath"];
            _context = context;
            _userService = context.Resolve<IUserService>(new NamedParameter("path", _path));
            _productService = context.Resolve<IProductService>(new NamedParameter("path", _path));
            _rolesService = context.Resolve<IRoles>(new NamedParameter("path", _path));
            _categoryService = context.Resolve<ICategoryService>(new NamedParameter("path", _path));
        }


        // GET: Cabinet
        [Authorize]
        public ActionResult UserPanel()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult ManagerPanel()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult ShowAllUsers()
        {
            var users = _userService.GetUsers();
            List<LoginUserModel> newUsers = new List<LoginUserModel>();

            foreach(var n in users)
            {
                newUsers.Add(Mapper.Map<LoginUserModel>(n));
            }
            return View(newUsers);
        }

        public ActionResult ShowAllBannedProduct()
        {
            var products = _productService.GetProducts().Where(x => x.State.Equals(State.Banned));

            List<ProductDTOModel> newProducts = new List<ProductDTOModel>();
            foreach(var n in products)
            {
                newProducts.Add(Mapper.Map<ProductDTOModel>(n));
            }

            return View(newProducts);
        }

        [Authorize(Roles ="Admin, Manager")]
        public ActionResult ExtendAuction(Guid id)
        {
            var product = _productService.product(id);
            return View(Mapper.Map<ProductDTOModel>(product));
        }

        [HttpPost]
        public ActionResult ExtendAuction(ProductDTOModel productModel)
        {
            productModel.State = State.Selling;
            _productService.Update(Mapper.Map<Business.Entities.Product>(productModel));

            return RedirectToAction("Index", "Product");
        }

        public ActionResult ShowMyProduct()
        {
            string userName = User.Identity.Name;
            Guid id = _userService.GetUserId(userName);
            var products = _productService.GetProducts().Where(x => x.UserId.Equals(id));
            List<ProductDTOModel> myProduct = new List<ProductDTOModel>();

            foreach (var product in products)
            {
                product.StartDate = DateTime.Now;
                ProductDTOModel newProduct = new ProductDTOModel();
                newProduct = Mapper.Map<ProductDTOModel>(product);
                newProduct.TheRestOfTime = product.Duration.Subtract(product.StartDate);
                if (newProduct.TheRestOfTime <= TimeSpan.Zero)
                {
                    newProduct.TheRestOfTime = TimeSpan.Zero;
                    newProduct.State = State.Banned;
                }
                var name = _categoryService.category(product.СategoryId);
                newProduct.СategoryName = name.Name;
                myProduct.Add(newProduct);
            }

            return View(myProduct);
        }


    }
}