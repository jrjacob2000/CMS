using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using CMS.DataAccess;
using CMS;

namespace Web2.Controllers
{
    public class TransactionAPIController : ApiController
    {
        //// GET api/<controller>
        public object GetByTransactionType(int type, string dateFilter, int page)
        {
            try
            {
                DateTime? filter = null;
                object res = null;
                if (!string.IsNullOrEmpty(dateFilter))
                    filter = DateTime.Parse(dateFilter);

                CMS.DataAccess.TransactionService service = new CMS.DataAccess.TransactionService();
                switch(type)
                {
                    case 1:
                        res = service.GetContributions(filter, page);
                        break;
                    case 2:
                        res = service.GetExpenses(filter, page);
                        break;
                }
                

                //CMS.DataAccess.BankEntryService bankService = new BankEntryService();
                //DateTime? filter = null;
                //if (!string.IsNullOrEmpty(dateFilter))
                //    filter = DateTime.Parse(dateFilter);
                //object res;
                //if (type == "DEPOSIT" || type == "REIMBURSE")
                //    res = bankService.GetBankEntriesList(type == "REIMBURSE" ? "WITHDRAWAL" : type, filter, page);
                //else
                //    res = service.GetByTransactionType(type, filter, page);

                return res;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post(CMS.DataAccess.Model.Transaction trans)
        {
            try
            {
                trans.Id = Guid.NewGuid();
                var service = new CMS.DataAccess.TransactionService();                
                var id = service.Create(trans);

                var response = Request.CreateResponse<CMS.DataAccess.Transaction>(HttpStatusCode.Created, new CMS.DataAccess.Transaction { Id = trans.Id });
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(CMS.DataAccess.Model.Transaction trans)
        {
            try
            {
                var service = new CMS.DataAccess.TransactionService();
                var id = service.Update(trans);

                var response = Request.CreateResponse<CMS.DataAccess.Transaction>(HttpStatusCode.Created, new CMS.DataAccess.Transaction { Id = trans.Id });
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(Guid Id)
        {
            try
            {
                CMS.DataAccess.TransactionService service = new CMS.DataAccess.TransactionService();
                service.Delete(Id);
                var response = Request.CreateResponse<CMS.DataAccess.Transaction>(HttpStatusCode.OK, new CMS.DataAccess.Transaction { Id = Id });
                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }               
    }
   
}