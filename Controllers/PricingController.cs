using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class PricingController : Controller
    {
        // GET: Pricing
        public ActionResult PriceList()
        {
            LibraryEntities db = new LibraryEntities();

            return View();
        }
    }
}