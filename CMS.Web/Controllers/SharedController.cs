using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web2.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Pagination()
        {
            return View();
        }
    }
}