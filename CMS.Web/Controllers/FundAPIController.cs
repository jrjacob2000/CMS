using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web2.Controllers
{
    public class FundAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CMS.DataAccess.Model.BalanceFund> Get()
        {
            var service = new CMS.DataAccess.AccountServices();

            return service.GetBalanceFundList();
            
        }
        
        // POST api/<controller>
        public HttpResponseMessage Post(CMS.DataAccess.Model.Fund fund)
        {
            try
            {
                fund.Id = Guid.NewGuid();
                var service = new CMS.DataAccess.AccountServices();

                service.CreateFundAccount(fund);
                
                var response = Request.CreateResponse<CMS.DataAccess.Transaction>(HttpStatusCode.Created, new CMS.DataAccess.Transaction { Id = fund.Id });
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}