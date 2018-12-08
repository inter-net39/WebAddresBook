using _1_AdressBook.Models;
using System.Web.Mvc;

namespace _1_AdressBook.Controllers
{
    public class PersonController : Controller
    {
        private readonly int _rowsPerPage = 3;

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            TempData["CurrentPage"] = page;
            return View();
        }
        [HttpPost]
        public ActionResult Index(int page = 1, string filter = "")
        {
            TempData["CurrentPage"] = page;
            ViewBag.Filter = filter;
            return View();
        }
        [HttpGet]
        public ActionResult Table(int page = 1, string filter = "")
        {
            SourceManager manager = new SourceManager();
            ViewBag.Filter = filter;
            TempData["CurrentPage"] = page;
            TempData["PageCount"] = ((manager.GetCount() - 1) / _rowsPerPage) + 1;
            if (filter != "")
            {
                TempData["CurrentPage"] = page;
                TempData["PageCount"] = ((manager.GetCount(filter) - 1) / _rowsPerPage) + 1;
                var lista = manager.Get((page - 1) * _rowsPerPage, _rowsPerPage, filter);
                return PartialView(lista);
            }
            return PartialView(manager.Get((page - 1) * _rowsPerPage, _rowsPerPage));
        }

        [HttpGet]
        public ActionResult Add()
        {
            TempData["success"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult Add(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                SourceManager manager = new SourceManager();

                TempData["success"] = "Dodano wpis o ID: " + manager.Add(model);
                return RedirectToAction("Index", "Person");
            }
            TempData["success"] = "Niepowodzenie podczas dodawania uzytkownika";
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SourceManager manager = new SourceManager();
            PersonModel model = manager.GetByID(id);
            if (model == null)
            {
                TempData["error"] = "Wystąpił błąd podczas edycji użytkownika";
                RedirectToAction("Index", "Person");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                SourceManager manager = new SourceManager();
                if (manager.Update(model) != -1)
                {
                    TempData["success"] = "Pomyślnie zaktualizowano użytkownika o ID = " + model.ID;
                    return RedirectToAction("Index", "Person");
                }
            }
            TempData["error"] = "Wystąpił błąd podczas edycji użytkownika";
            return RedirectToAction("Index", 1);
        }
        [HttpGet]
        public ActionResult Remove(int id)
        {
            SourceManager manager = new SourceManager();
            PersonModel model = manager.GetByID(id);
            if (model != null)
            {
                return View(model);
            }
            TempData["error"] = "Wystąpił błąd podczas usuwania użytkownika";
            return RedirectToAction("Index", 1);
        }
        [HttpPost]
        public ActionResult RemoveConfirm(int id)
        {
            SourceManager manager = new SourceManager();
            if (manager.Remove(id) != 1)
            {
                TempData["error"] = "Wystąpił błąd podczas usuwania użytkownika";
                return RedirectToAction("Index", 1);
            }
            TempData["success"] = "Pomyślnie Usunięto użytkownika użytkownika.";
            return RedirectToAction("Index", 1);
        }
    }
}