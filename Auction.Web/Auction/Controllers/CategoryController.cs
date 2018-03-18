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
using System.Web.Configuration;
using System.Web.Mvc;

namespace Auction.Controllers
{
    public class CategoryController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();

        private ICategoryService _categoryService;
        private IProductService _productService;
        private string _path;
        private string name;
        private string type;
        private readonly IComponentContext _context;

        public CategoryController(IComponentContext context)
        {
                _context = context;
                logger.Trace("Constructor CategoryController");
        }

        // GET: Category
        public ActionResult GetCategorys()
        {
            List<CategoryModel> categorys = new List<CategoryModel>();
            var categ = _categoryService.GetCategorys();
            if (categ.Count() == 0)
            {
                return View();
            }
            else
            {
                categorys = Mapper.Map<List<CategoryModel>>(categ);
                return View(categorys);
            }
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryModel category)
        {
            try
            {
                _categoryService.addCategory(Mapper.Map<Business.Entities.Category>(category));

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection appSettings = config.AppSettings;
                appSettings.Settings.Add(category.Name, category.Step.ToString());
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return RedirectToAction("GetCategorys", "Category");
            }

            catch(ConfigurationErrorsException e)
            {
                throw new ConfigurationErrorsException(e.Message);
            }
        }

        public ActionResult DeletCategory(Guid id)
        {
            var categ = _categoryService.category(id);
            CategoryModel category = Mapper.Map<CategoryModel>(categ);
            return View(category);
        }

        [HttpPost]
        public ActionResult DeletCategory(CategoryModel category)
        {
            try
            {
                var products = _productService.GetProducts().Where(x => x.СategoryId.Equals(category.Id));
                foreach (var p in products)
                {
                    _productService.RemoveProduct(p.Id);
                }

                var categName = _categoryService.category(category.Id).Name;
                _categoryService.deleteCategory(category.Id);

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection appSettings = config.AppSettings;
                appSettings.Settings.Remove(categName);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return RedirectToAction("GetCategorys", "Category");
            }
            catch(ConfigurationErrorsException e)
            {
                throw new ConfigurationErrorsException(e.Message);
            }
        }

        public ActionResult DiscriptionShow(Guid id)
        {
            var discription = _categoryService.category(id);
            return View(Mapper.Map<CategoryModel>(discription));
        }

        [Authorize(Roles ="Admin")]
        public ActionResult EditCategory(Guid id)
        {
            var item = _categoryService.category(id);
            var category = Mapper.Map<CategoryModel>(item);
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryModel category)
        {
            _categoryService.Update(Mapper.Map<Business.Entities.Category>(category));
            return RedirectToAction("AdminPanel", "AdminCabinet");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _productService = _context.Resolve<IProductService>(new NamedParameter("type", type), new NamedParameter("path", _path));
        }

    }
}