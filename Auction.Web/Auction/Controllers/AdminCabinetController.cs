using Auction.Business.Entities;
using Auction.Business.Services.Interfaces;
using Auction.Configure;
using Auction.Filters;
using Auction.Models;
using Autofac;
using AutoMapper;
using NLog;
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
    public class AdminCabinetController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();

        IUserService _userService;
        IProductService _productService;
        IRoles _rolesService;
        IBidService _bidService;
        ICategoryService _categoryService;
        string _path;
        string name;
        string type;
        IComponentContext _context;

        public AdminCabinetController(IComponentContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
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
                var user = Mapper.Map<LoginUserModel>(n);
                var roles = _rolesService.GetUserRoles(Guid.Parse(user.Id)).ToList();
                newUsers.Add(user);
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
            return View(Mapper.Map<ProductModel>(product));
        }

        [HttpPost]
        public ActionResult ExtendAuction(ProductModel productModel)
        {
            productModel.State = State.Selling;
            _productService.Update(Mapper.Map<Business.Entities.Product>(productModel));

            return RedirectToAction("Index", "Product");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _userService = _context.Resolve<IUserService>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
            _productService = _context.Resolve<IProductService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _rolesService = _context.Resolve<IRoles>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
        }

    }
}