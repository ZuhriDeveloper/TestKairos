using KairosTest.Interface;
using KairosTest.Models;
using KairosTest.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Calculator;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KairosTest.Controllers
{
    [Authorize]
    public class RentBookController : Controller
    {
        private IConfiguration Configuration { get; }
        private IBook BookMgr { get; }
        private IRentBook RentMgr { get; }

        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        public RentBookController(IConfiguration configuration, RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            Configuration = configuration;
            BookMgr = new BookRepo(configuration);
            RentMgr = new RentBookRepo(configuration);
            roleManager = roleMgr;
            userManager = userMrg;
        }

        [Authorize(Roles ="Tenant")]
        public ActionResult Index()
        {
            RentBook data = new RentBook();

            var listBooks = BookMgr.GetListBook();
            var bookOptions = new SelectList(listBooks.Select(org =>
            {
                return new SelectListItem { Text = org.BookTitle, Value = org.ID.ToString() };
            }),"Value","Text");

            ViewData["BookOptions"] = bookOptions;


            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RentBook data)
        {
          

            var listBooks = BookMgr.GetListBook();
            var bookOptions = new SelectList(listBooks.Select(org =>
            {
                return new SelectListItem { Text = org.BookTitle, Value = org.ID.ToString() };
            }), "Value", "Text");

            ViewData["BookOptions"] = bookOptions;

            var book = BookMgr.GetBookByID(data.BookID);
            data.RentLenght = (data.EndDate - data.StartDate).Days;
            data.PricePerDay = book.RentPrice;
            data.UserName = User.Identity.Name;
            
            RentMgr.Create(data);

            ModelState.Clear();
            TempData["Message"] = "rent successfully saved";

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculatePrice(int bookId,DateTime dateStart,DateTime dateEnd)
        {
            Book data = BookMgr.GetBookByID(bookId);
            int dateDiff = (dateEnd - dateStart).Days;
            try
            {
                CalculatorSoapClient calculator = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap12);
                var result = await calculator.MultiplyAsync(Convert.ToInt32(data.RentPrice), dateDiff);

                return Json(result); ;
            }
            catch (Exception)
            {
                var result = Convert.ToInt32(data.RentPrice) * dateDiff;
                return Json(result); ;
            }  
        }

        [Authorize(Roles = "Tenant,Admin")]
        public async Task<ActionResult> ReportAsync()
        {
            List<RentBook> listData = RentMgr.GetList();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var roles = await userManager.GetRolesAsync(user);

            if(!roles.Contains("Admin"))
            {
                listData = listData.Where(x => x.UserName == User.Identity.Name).ToList();
            }

            return View(listData);
        }

    }
}
