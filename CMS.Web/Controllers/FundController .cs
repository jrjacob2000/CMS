using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web2.Controllers
{
    public class FundController : Controller
    {
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFund()
        {
            return View();
        } 
        
    }
}