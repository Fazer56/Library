﻿using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class AboutController : Controller
    {
        // GET: Info
        public ActionResult Info()
        {
			LibraryEntities db = new LibraryEntities();
			
            return View();
        }
    }
}