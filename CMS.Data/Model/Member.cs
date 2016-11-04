using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess.Model
{
    public class Member
    {
        public Guid Id {get;set;}
        public string FirstName {get;set;}
        public string MiddleName {get;set;}
        public string LastName {get;set;}
        public int Age {get;set;}
        public string Gender {get;set;}
        public string Birthday {get;set;}
        public string MobilePhone {get;set;}
        public string LandLine {get;set;}
        public string Address {get;set;}
        public string MaritalStatus {get;set;}
        public string NameOfSpouse {get;set;}
        public string SpouseContact {get;set;}
        public int? ChildrenCount {get;set;}
        public string MemberStatus {get;set;}
        public string BaptizedDate {get;set;}
        public string BaptizedPlace {get;set;}
        public string BaptizedMinister {get;set;}
        public string BelongsToGroups {get;set;}
        public string Positions { get; set; }
    }
}