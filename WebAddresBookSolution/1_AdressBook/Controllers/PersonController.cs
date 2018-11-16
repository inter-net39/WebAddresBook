using _1_AdressBook.Models;
using System;
using System.Web.Mvc;

namespace _1_AdressBook.Controllers
{
    public class PersonController : Controller
    {
        private readonly int _rowsPerPage = 3;


        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            try
            {
                TempData["CurrentPage"] = page;
                return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("NotFound404", new { info = e.Message });
            }
        }
        [HttpPost]
        public ActionResult Index(int page = 1, string filter = "err")
        {
            try
            {
                TempData["CurrentPage"] = page;
                return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage));
            }
            catch (Exception e)
            {
                return RedirectToAction("NotFound404", new { info = e.Message });
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                TempData["succes"] = -1;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("NotFound404", new{info = e.Message} );
            }
        }

        [HttpPost]
        public ActionResult Add(PersonModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SourceManager manager = new SourceManager();
                    
                    TempData["succes"] = manager.Add(model);

                    return View();
                }

                TempData["succes"] = -1;
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("NotFound404", new { info = e.Message });
            }
        }



        public ActionResult NotFound404(string info)
        {
            Response.StatusCode = 404;
            TempData["info"] = info;
            return View(TempData["info"]);
        }
    }
}