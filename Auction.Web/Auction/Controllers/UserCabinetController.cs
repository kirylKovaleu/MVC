using Auction.Business.Services.Interfaces;
using Auction.Configure;
using Auction.Filters;
using Auction.Models;
using Autofac;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using static Auction.Controllers.ProductController;

namespace Auction.Controllers
{
    public class UserCabinetController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();

        private IProductService _productService;
        private ICategoryService _categoryService;
        private IUserService _userSevice;
        private IBidService _bidService;
        private string _path;
        string name;
        string type;
        private readonly IComponentContext _context;

        public UserCabinetController(IComponentContext context)
        {
            _context = context;
        }
        // GET: UserCabinet
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditMyData(Guid id)
        {
            var user = Mapper.Map<UserModel>(_userSevice.user(id));
            return View(user);
        }

        [Authorize]
        public ActionResult ShowMyProduct()
        {
            string userName = User.Identity.Name;
            Guid id = _userSevice.GetUserId(userName);
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

        [Authorize]
        public ActionResult ShowMyBid()
        {
            List<ProductDTOModel> newProducts = new List<ProductDTOModel>();
            string user = User.Identity.Name;
            Guid id = _userSevice.GetUserId(user);
            var result = _bidService.GetBids().Where(x => x.UserId.Equals(id));
            if (result.Count() == 0)
            {
                return View();
            }

            else
            {
                var item = _productService.GetProducts();
                var products = from c in result
                                join x in item on
                                c.ProductId equals x.Id
                                select x;

                foreach(var product in products)
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
                    newProducts.Add(newProduct);
                }

                return View(newProducts);
            }
        }


        public ActionResult PersonalData()
        {
            Guid id = _userSevice.GetUserId(User.Identity.Name);
            var user = _userSevice.user(id);

            return View(Mapper.Map<UserModel>(user));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _productService = _context.Resolve<IProductService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _userSevice = _context.Resolve<IUserService>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
            _bidService = _context.Resolve<IBidService>(new NamedParameter("type", type), new NamedParameter("path", _path));
        }
    }
}