using System.Web.Mvc;
using _1_AdressBook.Models;

namespace _1_AdressBook.Controllers
{
    public class PersonController : Controller
    {
        private readonly int _rowsPerPage = 3;


        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            TempData["CurrentPage"] = page;
            return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage));
        }
        [HttpPost]
        public ActionResult Index(int page = 1, string filter = "err")
        {
            TempData["CurrentPage"] = page;
            return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                SourceManager manager = new SourceManager();
                manager.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    return View(new SourceManager().Get(1,1));
        //}
    }
}