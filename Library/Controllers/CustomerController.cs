using Library.Models;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Library.Controllers
{
	public class CustomerController : Controller
	{
		// GET: Customer
		public ActionResult Login()
		{
			/* To Display a list of fields in database
			LibraryEntities db = new LibraryEntities();
			return View(db.Customers.ToList());*/
			return View();
		}

		// GET: Customer
		/*The Index() method of the Home controller is the default method for an ASP.NET MVC application. 
		  When you run an ASP.NET MVC application, the Index() method is the first controller method that
		  is called.*/


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(Customer objUser)
		{
			//used to validate that the rules of the model are being applied. ie required fields and correct format 
			if (ModelState.IsValid)
			{
				
				using (LibraryEntities db = new LibraryEntities())
				{
					var obj = db.Customers.Where(a => a.CustEmail.Equals(objUser.CustEmail) && a.CPassword.Equals(objUser.CPassword)).FirstOrDefault();
					
					if (obj != null)
					{
						Session["custID"] = obj.CustID.ToString();
						Session["custName"] = obj.CustName.ToString();
						Session["custEmail"] = obj.CustEmail.ToString();
						Debug.WriteLine(Session["custID"].ToString());
						return RedirectToAction("UserArea");
					}
				}
			}
			return View(objUser);
		}

		[Route("customer/UserArea")]
		public ActionResult UserArea()
		{
			if (Session["custID"] != null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Login");
			}
		}
	}
}