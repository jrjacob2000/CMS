using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess
{
    public class MemberService
    {
        public Guid Create(Model.Member memberEntity)
        {
            var context = new CMS_DataContext();

            var member = new DataAccess.Member() { 
                Id = memberEntity.Id,
                FirstName = memberEntity.FirstName,
                MiddleName = memberEntity.MiddleName,
                LastName = memberEntity.LastName,
                Age = memberEntity.Age,
                Gender = memberEntity.Gender,
                Birthday = string.IsNullOrEmpty(memberEntity.Birthday)? (DateTime?)null: DateTime.Parse(memberEntity.Birthday),
                MobilePhone  = memberEntity.MobilePhone,
                LandLine = memberEntity.LandLine,
                Address = memberEntity.Address,
                MaritalStatus = memberEntity.MaritalStatus,
                NameOfSpouse = memberEntity.NameOfSpouse,
                SpouseContact = memberEntity.SpouseContact,
                ChildrenCount = memberEntity.ChildrenCount,
                MemberStatus = memberEntity.MemberStatus,
                BaptizedDate = string.IsNullOrEmpty(memberEntity.BaptizedDate)?(DateTime?)null:DateTime.Parse(memberEntity.BaptizedDate),
                BaptizedPlace = memberEntity.BaptizedPlace,
                BaptizedMinister = memberEntity.BaptizedMinister,
                BelongsToGroups = memberEntity.BelongsToGroups,
                Positions = memberEntity.Positions
            };

            context.Members.InsertOnSubmit(member);
            context.SubmitChanges();
            return memberEntity.Id;
        }

        public List<Model.Member> List()
        {
            var context = new CMS_DataContext();
            return context.Members.Select( x =>
                new Model.Member()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Age = x.Age.Value,
                    Gender = x.Gender,
                    Birthday = x.Birthday.HasValue ? x.Birthday.Value.ToShortDateString():null,
                    MobilePhone = x.MobilePhone,
                    LandLine = x.LandLine,
                    Address = x.Address,
                    MaritalStatus = x.MaritalStatus,
                    NameOfSpouse = x.NameOfSpouse,
                    SpouseContact = x.SpouseContact,
                    ChildrenCount = x.ChildrenCount.HasValue ? x.ChildrenCount.Value : (int?)null,
                    MemberStatus = x.MemberStatus,
                    BaptizedDate = x.BaptizedDate.HasValue ? x.BaptizedDate.Value.ToShortDateString() : null,
                    BaptizedPlace = x.BaptizedPlace,
                    BaptizedMinister = x.BaptizedMinister,
                    BelongsToGroups = x.BelongsToGroups,
                    Positions = x.Positions
                }
                ).ToList();
        }

        public Member Get(Guid id)
        {
            var context = new CMS_DataContext();
            return context.Members.Where(x => x.Id == id).FirstOrDefault();
        }

        public Guid Update(Model.Member memberEntity)
        {
            var context = new CMS_DataContext();
            var member = context.Members.Where(x => x.Id == memberEntity.Id).FirstOrDefault();
            
            member.FirstName = memberEntity.FirstName;
            member.MiddleName = memberEntity.MiddleName;
            member.LastName = memberEntity.LastName;
            member.Age = memberEntity.Age;
            member.Gender = memberEntity.Gender;
            member.Birthday = string.IsNullOrEmpty(memberEntity.Birthday)? (DateTime?)null: DateTime.Parse(memberEntity.Birthday);
            member.MobilePhone = memberEntity.MobilePhone;
            member.LandLine = memberEntity.LandLine;
            member.Address = memberEntity.Address;
            member.MaritalStatus = memberEntity.MaritalStatus;
            member.NameOfSpouse = memberEntity.NameOfSpouse;
            member.SpouseContact = memberEntity.SpouseContact;
            member.ChildrenCount = memberEntity.ChildrenCount;
            member.MemberStatus = memberEntity.MemberStatus;
            member.BaptizedDate = string.IsNullOrEmpty(memberEntity.BaptizedDate) ? (DateTime?)null : DateTime.Parse(memberEntity.BaptizedDate);
            member.BaptizedPlace = memberEntity.BaptizedPlace;
            member.BaptizedMinister = memberEntity.BaptizedMinister;
            member.BelongsToGroups = memberEntity.BelongsToGroups;
            member.Positions = memberEntity.Positions;
            
            context.SubmitChanges();
            return memberEntity.Id;
        }
    }
}