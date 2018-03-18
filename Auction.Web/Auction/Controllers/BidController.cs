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
    public class BidController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();

        private IBidService _bidService;
        private IProductService _productService;
        private IUserService _userService;
        private ICategoryService _categoryService;
        private string _path;
        string name;
        string type;
        private readonly IComponentContext _context;
        
        public BidController(IComponentContext context)
        {
            _context = context;
            logger.Trace("Constructor BidController");
        }
        
        public DateTime GetTime(Guid id)
        {
            DateTime time = _bidService.GetTime(id);
            return time;
        }

        [Authorize]
        public ActionResult DeleteBid(Guid id)
        {
            Guid userId = _userService.GetUserId(User.Identity.Name);
            var bid = _bidService.GetBids().ToList().Find(x => x.ProductId.Equals(id) && x.UserId.Equals(userId));
            var product = _productService.GetProducts().ToList().Find(x => x.Id.Equals(bid.ProductId));
            if (product.State == State.Draft.ToString())
            {
                BidModel bidModel = Mapper.Map<BidModel>(bid);
                return View(bidModel);
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        public ActionResult DeleteBid(BidModel bidModel)
        {
            Guid id = bidModel.Id;
            _bidService.DeleteBid(id);
            return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        public ActionResult CreateBid(Guid Id, string url)
        {
            try
            {
                string[] keys = ConfigurationManager.AppSettings.AllKeys;
                var product = _productService.product(Id);
                string category = _categoryService.category(product.СategoryId).Name;
                Guid userId = _userService.GetUserId(User.Identity.Name);
                var bids = _bidService.GetBids().ToList().Find(x => x.ProductId.Equals(Id) && x.UserId.Equals(userId));
                if (bids== null)
                {
                    BidModel bid = new BidModel();
                    foreach (string s in keys)
                    {
                        if (s.Equals(category))
                        {
                            bid.Price = Convert.ToDecimal(ConfigurationManager.AppSettings[s]);
                            product.StartPrice += Convert.ToDecimal(ConfigurationManager.AppSettings[s]);
                            _productService.Update(product);
                        }
                    }

                    bid.ProductId = Id;
                    bid.UserId = userId;
                    bid.Time = DateTime.Now;

                    _bidService.AddBid(Mapper.Map<Business.Entities.Bid>(bid));
                }
               else
                {
                    foreach (string s in keys)
                    {
                        if (s.Equals(category))
                        {
                            bids.Price = Convert.ToDecimal(ConfigurationManager.AppSettings[s]);
                            product.StartPrice += Convert.ToDecimal(ConfigurationManager.AppSettings[s]);
                            _productService.Update(product);
                            _bidService.Update(bids);
                        }
                    }
                }

            }
            catch (ConfigurationErrorsException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return Redirect(url);

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _bidService = _context.Resolve<IBidService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _productService = _context.Resolve<IProductService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _userService = _context.Resolve<IUserService>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
        }

    }
}