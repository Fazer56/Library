using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult BookList()
        {
			LibraryEntities db = new LibraryEntities();
			
            return View();
        }
    }
}