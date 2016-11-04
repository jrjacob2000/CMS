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
    public class BankAPIController : ApiController
    {
        // GET api/<controller>
        public object GetBankActivity(string bankId, string dateFilter, int page)
        {
            try
            {
                CMS.DataAccess.BankService bankService = new BankService();
                //DateTime? filter = null;
                //if (!string.IsNullOrEmpty(dateFilter))
                //    filter = DateTime.Parse(dateFilter);
                //object res;
                                

                var res = bankService.GetBankActivity(new Guid(bankId), page,DateTime.Parse(dateFilter));

                return res;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // GET api/<controller>
        public object GetBankInfo()
        {
            try
            {
                CMS.DataAccess.BankService bankService = new BankService();

                var res = bankService.GetBankBalance();

                return res;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }


        // POST api/<controller>
        public HttpResponseMessage Post(CMS.DataAccess.Model.Bank bank)
        {
            try
            {
                bank.Id = Guid.NewGuid();
                var service = new CMS.DataAccess.BankService();
                var id = service.Create(bank);

                var response = Request.CreateResponse<CMS.DataAccess.Model.Bank>(HttpStatusCode.Created, new CMS.DataAccess.Model.Bank { Id = bank.Id });
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(CMS.DataAccess.Model.Bank bank)
        {
            try
            {
                var service = new CMS.DataAccess.BankService();
                service.Update(bank);

                var response = Request.CreateResponse<CMS.DataAccess.Model.Bank>(HttpStatusCode.Created, new CMS.DataAccess.Model.Bank { Id = bank.Id });
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
                CMS.DataAccess.BankService service = new CMS.DataAccess.BankService();
                service.Delete(Id);
                var response = Request.CreateResponse<CMS.DataAccess.Model.Bank>(HttpStatusCode.OK, new CMS.DataAccess.Model.Bank { Id = Id });
                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }               
    }
   
}