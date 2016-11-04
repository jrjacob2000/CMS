using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web2.Controllers
{
    public class FinancialController : Controller
    {
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateContribution()
        {
            return View();
        }

        public ActionResult CreateContribution()
        {
            return View();
        }

        public ActionResult CreateExpense()
        {
            return View();
        }

        public ActionResult Bank()
        {
            return View();
        }

        public ActionResult UpdateDeposit()
        {
            return View();
        }        

        public ActionResult Reimbursement()
        {
            return View();
        }
              
        public ActionResult UpsertBank()
        {
            return View();
        }

        public ActionResult BankTransfer()
        {
            return View();
        }
    }
}