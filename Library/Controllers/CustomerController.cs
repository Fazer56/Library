﻿using Library.Models;
using System;
using System.Diagnostics;
using System.Dynamic;
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

		
		public ActionResult UserArea()
		{
			if (Session["custID"] != null)
			{
				if (TempData["borrowMessage"] != null)
				{
					ViewBag.BorrowedMessage = TempData["borrowMessage"].ToString();
				}

				if (TempData["returnMessage"] != null)
				{
					ViewBag.ReturnMessage = TempData["returnMessage"].ToString();
				}
				Item it = new Item();
				LibraryEntities db = new LibraryEntities();

				/*dynamic model = new ExpandoObject();
				model.Items = db.Items.ToList();
				model.Copies = db.Copies.ToList();
				model.Transactions = db.Transactions.ToList();*/

				
				int cID = Int32.Parse(Session["custID"].ToString());
				var showBooks = from i in db.Items
								join c in db.Copies on i.Isbn equals c.Isbn
								join t in db.Transactions on c.CopyID equals t.CopyID
								where c.CopyID == t.CopyID && t.CustID == cID && i.Isbn == c.Isbn && c.Borrowed != "y"
								select new CopyItemTransacViewModel { Name = i.Name, Borrow_Date = c.Borrow_Date, Return_Date = c.Return_Date, Isbn = i.Isbn};
				//select new ItemLibrarianViewModel { It = i, Cp = c, Tc = t };



				showBooks.ToList();
			
				return View(showBooks);
			}
			else
			{
				return RedirectToAction("Login");
			}
		}

		//[ActionName("AddBook")]
		public ActionResult BookBorrowed(int bookId)
		{
			if (Session["custID"] != null)
			{
				using (LibraryEntities db = new LibraryEntities())
				{
					
					int cID = Int32.Parse(Session["custID"].ToString());
					var checkTrans = from c in db.Copies
									 from t in db.Transactions 
									 where c.CopyID == t.CopyID && c.Isbn == bookId && t.CustID == cID
									 select c;

					foreach (Copy cp in checkTrans.ToList())
					{
						if (cp.Borrowed != "n")
						{
							TempData["borrowMessage"] = "Already Borrowed This Book"; 
							return RedirectToAction("UserArea");
						}
					}
					
					Transaction t1 = new Transaction();
					var copyQuery = from b in db.Copies
									where b.Isbn == bookId && b.Borrowed == "n"
									select b;
					if (ModelState.IsValid)
					{
						foreach (Copy cp1 in copyQuery.ToList())
						{
							if (cp1.Borrowed != "y")
							{

								cp1.Borrowed = "y";
								// Get date-only portion of date, without its time.
								//DateTime dateOnly = DateTime.Now.ToShortDateString();
								var date = DateTime.Now.ToShortDateString();
								DateTime today = DateTime.Now.Date;
								DateTime retDay = today.AddDays(30);
								cp1.Borrow_Date = today;
								cp1.Return_Date = retDay;
								t1.CopyID = cp1.CopyID;
								t1.CustID = Int32.Parse(Session["custID"].ToString());
								db.Transactions.Add(t1);
								db.SaveChanges();

								if (t1.CustID > 0)
								{
									var book = db.Items.Where(a => a.Isbn.Equals(cp1.Isbn)).FirstOrDefault();
									ViewBag.Success = book.Name.ToString();

								}

								break;
							}
							else
							{
								ViewBag.Fail = "No copies Available";
							}

						}//end foreach()
						ModelState.Clear();
					}//end ModelState IF
						
				}
					
				return View();

			}//end session If
			else
			{
				return RedirectToAction("Login");
			}
		}
		public ActionResult BookReturned(int bookId)
		{
			using (LibraryEntities db = new LibraryEntities())
			{
				
				if (ModelState.IsValid)
				{
					/*UPDATE DATA MODEL SO TRANSACTION HOLDS BORROW AND RETURN FIELDS COPY ONLY FOR COPY ID ISBN AND Y/N BORRED OR NOT*/
					int cuID = Int32.Parse(Session["custID"].ToString());
					var returnQuery = from c in db.Copies
									  join t in db.Transactions
									  on c.CopyID equals t.CopyID
									  where c.CopyID == t.CopyID && c.Isbn == bookId && t.CustID == cuID
									  select c;

					foreach (Copy cp in returnQuery.ToList())
					{
						cp.Borrow_Date = null;
						cp.Return_Date = null;
						cp.Borrowed = "n";
						db.SaveChanges();
						if (cp.Borrowed == "n")
						{
							TempData["returnMessage"] = "Book Returned";
							return RedirectToAction("UserArea");
						}
						
						

					}



				}//end model state if
				ModelState.Clear();

			}

				return RedirectToAction("UserArea");
		}
	}
}

/*<div class="row">
	<div class="col-sm-3">
		<div class="mt-3">
			<h2>Add Books</h2>
		</div>
	</div>
	<div class="col-sm-3"></div>
	<div class="col-sm-3">
		<div class="mt-3">
			<h2>Add Students</h2>
		</div>
	</div>
</div>
<div class="row">
	<div class="col-sm-3">
		<button type = "submit" class="btn btn-success" id="editBtn">Edit</button>
		<button type = "submit" class="btn btn-success" id="addBtn">Add</button>
	</div>
	<div class="col-sm-3"></div>
	<div class="col-sm-3">
		<button type = "submit" class="btn btn-success" id="editStdBtn">Edit</button>
		<button type = "submit" class="btn btn-success" id="addStdBtn">Add</button>
	</div>
</div>*/