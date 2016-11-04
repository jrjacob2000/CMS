using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace CMS.DataAccess
{
    public class AccountServices
    {
        

        public List<Model.Fund> GetFundList(Guid? parent)
        {
            //List<Model.Fund> result = null;
            //CMS_DataContext context = new CMS_DataContext();
            //if (parent == null)
            //{
            //    result = (from f in context.Funds
            //              join ac in context.Accounts on f.AccountId equals ac.Id
            //              where f.Parent == null
            //              select new Model.Fund
            //              {
            //                  Id = f.AccountId.Value,
            //                  Description = ac.Name,
            //                  Type = ac.Type

            //              }).ToList();
            //}
            //else
            //{
            //    result = (from f in context.Funds
            //              join ac in context.Accounts on f.AccountId equals ac.Id
            //              where f.Parent == parent
            //              select new Model.Fund
            //              {
            //                  Id = f.AccountId.Value,
            //                  Description = ac.Name,
            //                  Type = ac.Type
            //              }).ToList();
            //}

            //foreach(Model.Fund f in result)
            //{
            //    f.Childrens = this.GetFundList(f.Id);
            //};

            //return result;
            return null;
        }        

        public List<Model.Fund> GetFundAccount(string fundType)
        {
            CMS_DataContext context = new CMS_DataContext();
            var items = (from fu in context.Funds
                         join ac in context.Accounts on fu.AccountId equals ac.Id
                         join parent in context.Accounts on fu.Parent equals parent.Id

                         where ac.Type.ToUpper() == fundType.ToUpper() || fu.FundType == "RESTRICTED"
                         select new Model.Fund
                         {
                             Id = fu.ID,
                             Account = new Model.Account() { Id = ac.Id, Name = fu.FundType == "RESTRICTED" ? "RES-" + ac.Name : ac.Name, Type = ac.Type, Category = ac.Category },
                             FundType = fu.FundType,
                             Parent = new Model.Account() { Id = parent.Id, Name = parent.Name, Type = parent.Type, Category = parent.Category },
                         }).OrderBy(o => o.Parent.Name).ThenBy(n => n.FundType).ToList();
            return items;
        }

        public List<Model.Account> GetBankAccount()
        {
            CMS_DataContext context = new CMS_DataContext();
            var items = context.Accounts.Where(x => x.Type == "ASSET" && x.Code == "BANK")
                         .Select(s => new Model.Account()
                         {
                             Id = s.Id,
                             Name = s.Name,
                             Type = s.Type,
                             Category = s.Category
                         }
                         ).ToList();
            return items;
        }

        public List<Model.BalanceFund> GetBalanceFundList()
        {
            try
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Account>(t => t.Funds);


                List<Model.BalanceFund> bf = null;
                CMS_DataContext context = new CMS_DataContext();
                context.LoadOptions = dlo;

                bf = context.Accounts
                    .Where(x => x.Category.ToUpper() == "BALANCE" && x.Type.ToUpper() == "FUND")
                    .Select(s => new Model.BalanceFund()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Unrestricted = (from f in context.FundActivities
                                        where f.FundType == "UNRESTRICTED" && f.Parent == s.Id
                                        select new Model.Fund()
                                        {
                                            Id = f.Id,
                                            Account = new Model.Account() { Id = f.AccountId, Name = f.Name, Type = f.Type },
                                            FundType = f.FundType,
                                            Amount = f.Amount.HasValue ? f.Amount.Value : 0
                                        }).ToList(),
                        Restricted = (from f in context.FundActivities
                                      where f.FundType == "RESTRICTED" && f.Parent == s.Id
                                      select new Model.Fund()
                                      {
                                          Id = f.Id,
                                          Account = new Model.Account() { Id = f.AccountId, Name = f.Name, Type = f.Type },
                                          FundType = f.FundType,
                                          Amount = f.Amount.HasValue ? f.Amount.Value : 0
                                      }).ToList(),
                        //Unrestricted = (from f in context.Funds
                        //               join a in context.Accounts on f.AccountId equals a.Id
                        //               join t in context.Transactions on a.Id equals t.CreditAccountId into trans
                        //               from x in trans.DefaultIfEmpty()
                        //               where  f.FundType == "UNRESTRICTED" && f.Parent == s.Id
                        //               select new Model.Fund(){
                        //                                Id = f.ID, 
                        //                                Account = new Model.Account(){Id = a.Id, Name = a.Name, Type = a.Type, Category = a.Category},
                        //                                FundType = f.FundType,
                        //                                Amount = 222
                        //                            }).ToList(),
                        //Restricted = (from f in context.Funds
                        //              join a in context.Accounts on f.AccountId equals a.Id
                        //              where f.FundType == "RESTRICTED" && f.Parent == s.Id
                        //              select new Model.Fund()
                        //              {
                        //                  Id = f.ID,
                        //                  Account = new Model.Account() { Id = a.Id, Name = a.Name, Type = a.Type, Category = a.Category },
                        //                  FundType = f.FundType,
                        //                  Amount = 222
                        //              }).ToList(),
                    }).ToList();



                return bf;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateFundAccount(DataAccess.Model.Fund fund)
        {
            CMS_DataContext context = new CMS_DataContext();

            var acct = new DataAccess.Account()
            {
                Id = Guid.NewGuid(),
                Name = fund.Account.Name,
                Category = fund.Account.Category,
                Type = fund.Account.Type,
                Code = null
            };

            var fnd = new DataAccess.Fund()
            {
                ID = Guid.NewGuid(),
                Parent = fund.Parent.Id,
                AccountId = acct.Id,
                FundType = fund.FundType
            };

            context.Accounts.InsertOnSubmit(acct);
            if (acct.Category.ToUpper() == "FUNDACTIVITY")
                context.Funds.InsertOnSubmit(fnd);
            context.SubmitChanges();
        }

        
    }
}