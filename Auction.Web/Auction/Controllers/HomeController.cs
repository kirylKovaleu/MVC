using Auction.Configure;
using Auction.Models;
using System.Web.Mvc;

namespace Auction.Controllers
{
    [Route("{house}/Home/Index")]
    public class HomeController : Controller
    {
        GetAuctioneHouse _getAuctioneHouses = new GetAuctioneHouse();
        // GET: Home
        public ActionResult Index(string house)
        {
            ViewBag.auctiones = _getAuctioneHouses.GetHouses();

            return View();
        }

        [HttpPost]
        public ActionResult Index(AuctionModel auctionModel)
        {
            if (auctionModel != null)
            {
                ViewBag.auctiones = _getAuctioneHouses.GetHouses();

                return RedirectToRoute("Auction", new { house = auctionModel.Name });
            }
            else
            {
                ViewBag.auctiones = _getAuctioneHouses.GetHouses();

                return View();
            }
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}