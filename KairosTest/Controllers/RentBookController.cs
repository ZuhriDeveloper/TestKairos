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

namespace KairosTest.Controllers
{
    [Authorize(Roles ="Tenant")]
    public class RentBookController : Controller
    {
        private IConfiguration Configuration { get; }
        private IBook BookMgr { get; }

        public RentBookController(IConfiguration configuration)
        {
            Configuration = configuration;
            BookMgr = new BookRepo(configuration);
        }
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

    }
}
