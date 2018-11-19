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
            TempData["CurrentPage"] = page;
            return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage));
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                TempData["succes"] = "";
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error500", new { info = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Add(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                SourceManager manager = new SourceManager();

                TempData["succes"] = "Dodano wpis o ID: " + manager.Add(model);
                return RedirectToAction("Index", "Person");
            }
            TempData["succes"] = "Niepowodzenie podczas dodawania uzytkownika";
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
                    TempData["succes"] = "Pomyślnie zaktualizowano użytkownika o ID = " + model.ID;
                    return RedirectToAction("Index", "Person");
                }
            }
            TempData["error"] = "Wystąpił błąd podczas edycji użytkownika";
            return RedirectToAction("Index", 1);
        }



      
    }
}