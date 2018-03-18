using Auction.Business.Services.Interfaces;
using Auction.Configure;
using Auction.Filters;
using Auction.Models;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Auction.Controllers
{
    public class UserController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GetAuctioneHouse _getAuctionesHouses = new GetAuctioneHouse();

        IUserService _userService;
        private string _path;
        private string name;
        private string type;
        private readonly IComponentContext _context;
        private ICategoryService _categoryService;
        private UserManager<LoginUserModel, string> _userManager;
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private IRoles _iRole;

        public UserController(IComponentContext context)
        {
            _context = context;
            logger.Trace("Constructor UserController");
        }


        // GET: User
        public ActionResult Index()
        {
            //IAuthenticationManager
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserModel loginUserModel)
        {
            var users = _userService.GetUsers().ToList();
            if(users.Count == 0)
            await _userService.SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(loginUserModel.Login, loginUserModel.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", App_LocalResources.GlobalRes.ЧтоНеПроскочил);
                    return View();
                }

                var fullUserInfo = _userService.user(Guid.Parse(user.Id));
                ChangeCulture(fullUserInfo.Locale);
                GetZone(fullUserInfo.TZone);

                var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                
                AuthenticationManager.SignIn(identity);
                return RedirectToAction("Index","Home");
            }
            ModelState.AddModelError("", "");
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Lang = GetLanguages();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterUserModel registerModelUser)
        {
            var users = _userService.GetUsers().ToList();
            if (users.Count == 0)
                await _userService.SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                var user = Mapper.Map<LoginUserModel>(registerModelUser);
                var item = await _userManager.CreateAsync(user);
                if (item.Succeeded)
                {
                    ChangeCulture(user.Locale);
                    ViewBag.Message = App_LocalResources.GlobalRes.Поздравляю + registerModelUser.Login + "ты с нами";
                    return PartialView("SuccessRegister");
                }
                else
                {
                    ModelState.AddModelError("", App_LocalResources.GlobalRes.УНасУжеТакойЕсть);
                    ViewBag.Lang = GetLanguages();
                    return View();
                }
            }

            ViewBag.Lang = GetLanguages();
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUsers(Guid id)
        {
            LoginUserModel user = await _userManager.FindByIdAsync(id.ToString());
            return View(user);
        }
        
        [HttpPost]
        public async Task<ActionResult> DeleteUsers(LoginUserModel userModel)
        {
            var result = await _userManager.DeleteAsync(userModel);

            if(result.Succeeded)
            return RedirectToAction("ShowAllUsers", "AdminCabinet");

            else
            {
                ModelState.AddModelError("", App_LocalResources.GlobalRes.ТыДажеУдалитьТолкомНичегоНеМожешь );
                return View();
            }
        }

        [Authorize(Roles ="Admin")]
        public ActionResult UpStatusUser(Guid id)
        {
            AuctionHouseRoleModel houseRole = new AuctionHouseRoleModel();
            houseRole.UserId = id;
            ViewBag.categorys = GetCategorys();

            return View(houseRole);
        }

        [HttpPost]
        public ActionResult UpStatusUser(AuctionHouseRoleModel houseRole)
        {
            _iRole.AddUserToRole(Guid.Parse(houseRole.UserId.ToString()), houseRole.role, houseRole.CategoryId);
            return RedirectToAction("ShowAllUsers", "AdminCabinet");
        }

        public ActionResult EditUser()
        {
            Guid userId = _userService.GetUserId(User.Identity.Name);
            var user = Mapper.Map<UserModel>(_userService.user(userId));
            ViewBag.Tzone = GetTimeZones();
            ViewBag.Lang = GetLanguages();

            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _userManager.Update(Mapper.Map<LoginUserModel>(userModel));
                ChangeCulture(userModel.Locale);

                return RedirectToAction("PersonalData", "UserCabinet");
            }
            else
            {
                ViewBag.Tzone = GetTimeZones();
                ViewBag.Lang = GetLanguages();

                return View();
            }
        }

        public ActionResult ChangeCulture(string lang)
        {
            try
            {

                string returnUrl = Request.UrlReferrer.AbsolutePath;
                // Список культур
                List<string> cultures = new List<string>() { "ru", "en", "br" };
                if (!cultures.Contains(lang))
                {
                    lang = "ru";
                }
                // Сохраняем выбранную культуру в куки
                HttpCookie cookie = Request.Cookies["lang"];
                if (cookie != null)
                    cookie.Value = lang;   // если куки уже установлено, то обновляем значение
                else
                {

                    cookie = new HttpCookie("lang");
                    cookie.HttpOnly = false;
                    cookie.Value = lang;
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Add(cookie);
                return Redirect(returnUrl);
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        private void GetZone(string zone)
        {
            HttpCookie cookie = Request.Cookies["TimeZone"];
            TimeSpan d = TimeSpan.Parse(zone);
            DateTime h = DateTime.UtcNow.Add(d);

            cookie = new HttpCookie("TimeZone");
            cookie.HttpOnly = false;
            cookie.Value = zone;

            cookie.Expires = h;
            Response.Cookies.Add(cookie);
        }

        private IEnumerable<SelectListItem> GetTimeZones()
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = timeZones.Select(c => new SelectListItem
            {
                Text = c.DisplayName,
                Value = c.BaseUtcOffset.ToString()
            });

            return timeZone;
        }

        private IEnumerable<SelectListItem> GetLanguages()
        {
            var lang = new[] 
            {
                new SelectListItem {Text = App_LocalResources.GlobalRes.Беларуский,Value = Languages.br.ToString()},
                new SelectListItem {Text=App_LocalResources.GlobalRes.Русский,Value=Languages.ru.ToString() },
                new SelectListItem {Text=App_LocalResources.GlobalRes.Англиский,Value=Languages.en.ToString() }
            };
            
            return lang;
        }

        private IEnumerable<SelectListItem> GetCategorys()
        {
            var categorys = _categoryService.GetCategorys();
            var SelectListCategorys = categorys.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return SelectListCategorys;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var item = _context.Resolve<IUserStore<LoginUserModel>>();
            _userManager = new UserManager<LoginUserModel, string>(item);
            name = RouteData.Values["house"].ToString();
            _path = _getAuctionesHouses.GetPath(name).Path;
            type = _getAuctionesHouses.GetPath(name).Type;
            _categoryService = _context.Resolve<ICategoryService>(new NamedParameter("type", type), new NamedParameter("path", _path));
            _userService = _context.Resolve<IUserService>( new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
            _iRole = _context.Resolve<IRoles>(new NamedParameter("path", ConfigurationManager.AppSettings["JsonRepositoryPath"]));
        }
    }
}