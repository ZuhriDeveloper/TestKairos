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
using System.Text;

namespace KairosTest.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private IConfiguration Configuration { get; }
        private IBook BookMgr { get; }

        public BookController(IConfiguration configuration)
        {
            Configuration = configuration;
            BookMgr = new BookRepo(Configuration);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Book> data = BookMgr.GetListBook();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookMgr.Create(data);
                    TempData["Message"] = "Data created successfully";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("You have a errors:");

                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            sb.Append(error.ErrorMessage);
                        }
                    }

                    TempData["Error"] = sb.ToString();

                    return View();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int ID)
        {
            Book data = new Book();
            data = BookMgr.GetBookByID(ID);
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookMgr.Update(data);
                    TempData["Message"] = "Data updated successfully";
                    ModelState.Clear();
                    data = BookMgr.GetBookByID(data.ID);
                    return View(data);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("You have a errors:");

                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            sb.Append(error.ErrorMessage);
                        }
                    }

                    TempData["Error"] = sb.ToString();
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult GetBookDetail(int bookId)
        {
            Book data = BookMgr.GetBookByID(bookId);

            return Json(data);


        }

        public IActionResult Delete(int id)
        {
            BookMgr.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
