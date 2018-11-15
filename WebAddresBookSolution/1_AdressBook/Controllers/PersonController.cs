using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using _1_AdressBook.Models;

namespace _1_AdressBook.Controllers
{
    public class PersonController : Controller
    {
        private int _rowsPerPage = 3;


        //[HttpGet]
        public ActionResult Index(int page = 1)
        {
            TempData["CurrentPage"] = page;
            return View(new SourceManager().Get((page - 1) * _rowsPerPage, _rowsPerPage ));
        }
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    return View(new SourceManager().Get(1,1));
        //}
    }
}