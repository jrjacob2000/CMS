using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WepAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
    public class Member
    {
        public Guid Id {get;set;}
        public string FirstName {get;set;}
        public string MiddleName {get;set;}
        public string LastName {get;set;}
        public int? Age {get;set;}
        public string Gender {get;set;}
        public System.DateTime? Birthday {get;set;}
        public string MobilePhone {get;set;}
        public string LandLine {get;set;}
        public string Address {get;set;}
        public string MaritalStatus {get;set;}
        public string NameOfSpouse {get;set;}
        public string SpouseContact {get;set;}
        public int? ChildrenCount {get;set;}
        public string MemberStatus {get;set;}
        public System.DateTime? BaptizedDate {get;set;}
        public string BaptizedPlace {get;set;}
        public string BaptizedMinister {get;set;}
        public string BelongsToGroups {get;set;}
        public string Positions {get;set;}
    }
}