using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			LibraryEntities db = new LibraryEntities();
			return View(db.Customers.ToList());
			
		}

		public ActionResult Login()
		{
			return View();

		}

		public ActionResult StaffLogin()
		{
			return View();

		}
	}
}