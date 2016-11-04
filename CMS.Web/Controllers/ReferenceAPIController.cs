using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web2.Controllers
{
    public class ReferenceAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<object> Get(int id)
        {
            try
            {
                CMS.DataAccess.AccountServices service = new CMS.DataAccess.AccountServices();
                IEnumerable<object> result = null;
                switch(id)
                {
                    case 1:
                        result = service.GetFundAccount("INCOME");
                        break;
                    case 2:
                        result = service.GetFundAccount("EXPENSE");
                        break;
                    case 3:
                        result = service.GetBankAccount();
                        break;
                }
                   
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        
    }
}