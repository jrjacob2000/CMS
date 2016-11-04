using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.DataAccess;



namespace WepAPI.Controllers
{
    public class MemberAPIController : ApiController
    {
    
        // GET api/values
        public IEnumerable<JsonMemberRequest> Get()
        {
            try
            {
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var members = service.List();

                var result = members.Select(x =>
                    x.ConvertToJson()).ToList();

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
                var response = Request.CreateResponse<JsonMemberRequest>(HttpStatusCode.OK, member.ConvertToJson());
                
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/values
        public HttpResponseMessage Post(CMS.DataAccess.Member member)
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
        public HttpResponseMessage Put(CMS.DataAccess.Member member)
        {
            try
            {
                CMS.DataAccess.MemberService service = new CMS.DataAccess.MemberService();
                var id = service.Update(member);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex.Message); 
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

public static class JsonDtoMapper
{
    public static CMS.DataAccess.Member ConvertToMemberEntity(this JsonMemberRequest request)
    {
        var Id = string.IsNullOrEmpty(request.Id) ? Guid.NewGuid() : new Guid(request.Id);
        return new CMS.DataAccess.Member()
        {
            Id = Id,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Age = request.Age.ToInt(),
            Gender = request.Gender,
            Birthday = request.Birthday.ToDateTime(),
            MobilePhone = request.MobilePhone,
            LandLine = request.LandLine,
            Address = request.Address,
            MaritalStatus = request.MaritalStatus,
            NameOfSpouse = request.NameOfSpouse,
            SpouseContact = request.SpouseContact,
            ChildrenCount = request.ChildrenCount.ToInt(),
            MemberStatus = request.MemberStatus,
            BaptizedDate = request.BaptizedDate.ToDateTime(),
            BaptizedPlace = request.BaptizedPlace,
            BaptizedMinister = request.BaptizedMinister,
            BelongsToGroups = request.BelongsToGroups,
            Positions = request.Positions

        };

    }

    public static JsonMemberRequest ConvertToJson(this CMS.DataAccess.Member member)
    {
        try
        {
            var json = new JsonMemberRequest();

            json.Id = member.Id.ToString();
            json.FirstName = member.FirstName;
            json.MiddleName = member.MiddleName.SafeToString();
            json.LastName = member.LastName.SafeToString();
            json.Age = member.Age.SafeToString();
            json.Gender = member.Gender.SafeToString();
            json.Birthday = member.Birthday.HasValue? member.Birthday.Value.ToShortDateString(): string.Empty;
            json.MobilePhone = member.MobilePhone.SafeToString();
            json.LandLine = member.LandLine.SafeToString();
            json.Address = member.Address.SafeToString();
            json.MaritalStatus = member.MaritalStatus.SafeToString();
            json.NameOfSpouse = member.NameOfSpouse.SafeToString();
            json.SpouseContact = member.SpouseContact.SafeToString();
            json.ChildrenCount = member.ChildrenCount.SafeToString();
            json.MemberStatus = member.MemberStatus.SafeToString();
            json.BaptizedDate = member.BaptizedDate.HasValue? member.BaptizedDate.Value.ToShortDateString(): string.Empty;
            json.BaptizedPlace = member.BaptizedPlace.SafeToString();
            json.BaptizedMinister = member.BaptizedMinister.SafeToString();
            json.BelongsToGroups = member.BelongsToGroups.SafeToString();
            json.Positions = member.Positions.SafeToString();
            return json;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public static List<JsonMemberRequest> ConvertToJsonList(this List<CMS.DataAccess.Member> members)
    {
        List<JsonMemberRequest> results = new List<JsonMemberRequest>();
        foreach (CMS.DataAccess.Member member in members)
        {
            results.Add(member.ConvertToJson());
        }

        return results;

    }

}
public static class Helper
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static DateTime? ToDateTime(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        DateTime result;
        if (DateTime.TryParse(str, out result))
            return result;
        else
            throw new Exception("Cannot convert string to datetime.");

    }

    public static int? ToInt(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        int result;
        if (int.TryParse(str, out result))
            return result;
        else
            throw new Exception("Cannot convert string to int.");

    }

    public static string SafeToString(this object obj)
    {
        return (obj ?? "").ToString();
    }
}

public class JsonMemberRequest
{
    public string Id;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string Age;
    public string Gender;
    public string Birthday;
    public string MobilePhone;
    public string LandLine;
    public string Address;
    public string MaritalStatus;
    public string NameOfSpouse;
    public string SpouseContact;
    public string ChildrenCount;
    public string MemberStatus;
    public string BaptizedDate;
    public string BaptizedPlace;
    public string BaptizedMinister;
    public string BelongsToGroups;
    public string Positions;

}

