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
			if (Session["libID"] != null)
			{

				return View();
			}
			else
			{
				return RedirectToAction("StaffLogin");
			}

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult StaffArea(ItemLibrarianViewModel bookObj)
		{
			using (LibraryEntities db = new LibraryEntities())
			{
				// Create a new Order object.
				if (ModelState.IsValid)
				{
					Item it = new Item();
					it.AuthorID = bookObj.Author.AuthorID;
					it.Name = bookObj.Item.Name;
					it.Isbn = bookObj.Item.Isbn;
					it.Subject = bookObj.Item.Subject;
					it.Type = "Book";
					it.Year = bookObj.Item.Year;
					bookObj.Author.Isbn = bookObj.Item.Isbn;
					db.Authors.Add(bookObj.Author);
					db.Items.Add(it);
					db.SaveChanges();
					if (bookObj.Item.Isbn > 0)
					{
						ViewBag.Success = bookObj.Item.Name.ToString();

					}
					ModelState.Clear();
				}
				return View();
			}
		}
	}
}