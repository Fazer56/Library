using Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
	public class LibrarianController : Controller
	{
		// GET: Librarian
		public ActionResult StaffLogin()
		{
			return View();

		}

		//Tells runtime where to send form post data back to
		[HttpPost]
		//Protects against hackers
		[ValidateAntiForgeryToken]
		public ActionResult StaffLogin(Librarian libUser)
		{
			//Check that form data is in correct format for the model and obeys all the rules
			if (ModelState.IsValid)
			{
				//Database object for accessing data
				using (LibraryEntities db = new LibraryEntities())
				{
					//Check the password and ID match
					var obj = db.Librarians.Where(a => a.LibrarianID.Equals(libUser.LibrarianID) && a.Password.Equals(libUser.Password)).FirstOrDefault();
					Debug.WriteLine(obj.Name.ToString());
					if (obj != null)
					{
						//Set the session variables with values from the database
						Session["libID"] = obj.LibrarianID.ToString();
						Session["libLocation"] = obj.LibLocation.ToString();
						Session["libName"] = obj.Name.ToString();
						Session["pos"] = obj.Position.ToString();
						//redirect to the librarian page
						return RedirectToAction("StaffArea");

					}

				}
			}
			return View(libUser);

		}

		public ActionResult StaffArea()
		{
			return View();
			/*
			if (Session["libID"] != null)
			{

				return View();
			}
			else
			{
				return RedirectToAction("StaffLogin");
			} */
		}

		/*[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult StaffArea()
		{


			return View();

		}*/
	}
}