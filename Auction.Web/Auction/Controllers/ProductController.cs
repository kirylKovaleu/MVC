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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Auction.Controllers
{
    public class ProductController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public enum State { Draft,Selling,Banned}

        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IUserService _userSevice;
        private string _path;
        private string type;
        private string name;
        private readonly IComponentContext _context;

        public ProductController( IComponentContext context )
        {
            _context = context;
            logger.Trace("Constructor ProductController");
        }
        
        public ActionResult Index()
        {
            return View(GetProducts());
        }

        public ActionResult SortCategory(Guid id)
        {
            List<ProductDTOModel> products = new List<ProductDTOModel>();
            var category = _categoryService.category(id).Name;
            var tempProducts = from n in  GetProducts()
                               where n.СategoryName.Equals(category)
                               select n;
            products = Mapper.Map<List<ProductDTOModel>>(tempProducts);
            return View(products);
        }

        [Authorize]
        public ActionResult AddProduct()
        {
            ProductModel product = new ProductModel();
            ViewBag.categorys = GetCategorySelectList();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductModel product, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid && file != null)
            {
                product.Base64Picture = ConvertToBase64(file);
                product.UserId = GetUserId();
                product.State = State.Draft;
                product.StartDate = DateTime.Now.ToUniversalTime();
                if (product.StartDate > product.Duration)
                {
                    ViewBag.Message = App_LocalResources.GlobalRes.ПродолжительностьДоПозавчера;
                    return View();
                }
                _productService.AddProduct(Mapper.Map<Business.Entities.Product>(product));

                return RedirectToAction("index", "Home");
            }
            else
            {
                ViewBag.categorys = GetCategorySelectList();
                return View();
            }
        }
        
        [Authorize]
        public ActionResult GetProduct(Guid id)
        {
            var product = _productService.product(id);
            ProductDTOModel products = Mapper.Map<ProductDTOModel>(product);
            products.TheRestOfTime = product.Duration.Subtract(product.StartDate);
            if (products.TheRestOfTime <= TimeSpan.Zero)
            {
                products.TheRestOfTime = TimeSpan.Zero;
                products.State = State.Banned;
            }
            var name = _categoryService.category(product.СategoryId);
            products.СategoryName = name.Name;

            return View(products);
        }

        [Authorize]
        public ActionResult DeleteProduct(Guid Id, string url)
        {
            var product = Mapper.Map<ProductDTOModel>(_productService.product(Id));

            if (product.State == State.Draft || User.IsInRole("Admin, Manager"))
                return View(product);

            else
            {
                return Redirect(url);
            }
        }

        [HttpPost]
        public ActionResult DeleteProduct(ProductDTOModel product)
        {
            _productService.RemoveProduct(product.Id);
            return RedirectToAction("Index","Product");
        }
        
        [Authorize]
        public ActionResult EditProduct(Guid id, string url)
        {
            var product =  Mapper.Map<ProductModel>(_productService.product(id));
            if (product.State == State.Draft || product.State == State.Banned)
            {
                ViewBag.categorys = GetCategorySelectList();
                return View(product);
            }
            return Redirect(url);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductModel product, HttpPostedFileBase file)
       {
            product.StartDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    product.Base64Picture = ConvertToBase64(file);
                }
                
                product.State = State.Draft;
                _productService.Update(Mapper.Map<Business.Entities.Product>(product));
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.categorys = GetCategorySelectList();
                return View();
            }
        }

        static string ConvertToBase64(HttpPostedFileBase file)
        {
            if(file == null)
            {
                return "Изображение отсутствует";
            }
            var binaryReader = new BinaryReader(file.InputStream);

            byte[] image = binaryReader.ReadBytes(file.ContentLength);
            string base64String = Convert.ToBase64String(image);
            return base64String;
        }

        List<ProductDTOModel> GetProducts()
        {
            List<ProductDTOModel> products = new List<ProductDTOModel>();
            var tempProducts = _productService.GetProducts();
            foreach (var product in tempProducts)
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

                    products.Add(newProduct);
            }

            return products;
        }

        public ActionResult SortProductsName()
        {
            List<ProductDTOModel> products = GetProducts();
            products.Sort();
            return View(products);
        }

        Guid GetUserId()
        {
            string userName = User.Identity.Name;
            return _userSevice.GetUserId(userName);
        }

        [Authorize(Roles ="Admin, Manager")]
        public ActionResult ApproveProduct(Guid id)
        { 
            var product = _productService.product(id);
            product.State = State.Selling.ToString();
            _productService.Update(product);

            return RedirectToAction("Index", "Product");
        }

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            var cc = _categoryService.GetCategorys();
            var category = _categoryService.GetCategorys()
                            .Select(c => new SelectListItem
                             {
                                 Text = c.Name,
                                 Value = c.Id.ToString()
                             });
            return category;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _productService = _context.Resolve<IProductService>(new NamedParameter("type",type), new NamedParameter("path", _path));
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _userSevice = _context.Resolve<IUserService>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
        }
    }
}