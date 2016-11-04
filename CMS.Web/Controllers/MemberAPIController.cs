using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS;
using CMS.DataAccess;



namespace Web2.Controllers
{
    public class MemberAPIController : ApiController
    {
        // GET api/values
        public IEnumerable<CMS.DataAccess.Model.Member> Get()
        {
            try
            {
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var members = service.List();

                var result = members.ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // GET api/values/5
        public HttpResponseMessage Get(string id)
        {
            try
            {
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var member = service.Get(new Guid(id));
                var response = Request.CreateResponse<CMS.DataAccess.Member>(HttpStatusCode.OK, member);

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/values
        public HttpResponseMessage Post(CMS.DataAccess.Model.Member member)
        {
            try
            {
                member.Id = Guid.NewGuid();
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var id = service.Create(member);

                var response = Request.CreateResponse<CMS.DataAccess.Member>(HttpStatusCode.Created, new CMS.DataAccess.Member { Id = member.Id });
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/values/5
        public HttpResponseMessage Put(CMS.DataAccess.Model.Member member)
        {
            try
            {
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var id = service.Update(member);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
