using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1_AdressBook.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index(int id = 1)
        {
            return View();
        }
    }
}