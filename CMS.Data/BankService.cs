using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CMS.DataAccess.Model;

namespace CMS.DataAccess
{
    public class BankService
    {

        public List<Model.Bank> GetBankBalance()
        {
            CMS_DataContext context = new CMS_DataContext();
            var items = context.BankBalances
                         .Select(s => new Model.Bank()
                         {
                             Id = s.Id,
                             Name = s.Name,
                             Amount = s.Amount
                         }
                         ).ToList();
            return items;
        }

        public object GetBankActivity(Guid bankId, int page, DateTime dateFilter)
        {
            //decimal totalBal = 0;
            var pageSize = 10;

            CMS_DataContext context = new CMS_DataContext();
            var items = (from trans in context.Transactions
                         join bank in context.Accounts on trans.DebitAccountId equals bank.Id
                         join acct in context.Accounts on trans.CreditAccountId equals acct.Id
                         where bank.Code == "BANK" && bank.Id == bankId && (trans.Date >= dateFilter || dateFilter == null)
                         select new Model.BankActivity()
                         {
                             Id = trans.Id,
                             Reference = trans.Reference,
                             BankName = bank.Name,
                             Amount = trans.Amount,
                             Date = trans.Date,
                             TransactionType = "Deposit",
                             AccountName = acct.Name
                         }).ToList();

            items.AddRange((from trans in context.Transactions
                            join bank in context.Accounts on trans.CreditAccountId equals bank.Id
                            join acct in context.Accounts on trans.DebitAccountId equals acct.Id
                            where bank.Code == "BANK" && bank.Id == bankId && (trans.Date >= dateFilter || dateFilter == null)
                            select new Model.BankActivity()
                            {
                                Id = trans.Id,
                                Reference = trans.Reference,
                                BankName = bank.Name,
                                Amount = trans.Amount * -1,
                                Date = trans.Date,
                                TransactionType = "Withdraw",
                                AccountName = acct.Name
                            }).ToList());

            var count =  items.Count();
            //var pagedItems = items.OrderByDescending(x => x.Date).ToList();
            var pagedItems = items.OrderByDescending(x => x.Date).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var balance = GetBankBalance().Where(x => x.Id == bankId).FirstOrDefault().Amount;

            return new JsonTransactionResponse()
            {
                Count = count,
                Items = pagedItems,
                TotalBalance = GetBankBalance().Where(x => x.Id == bankId).FirstOrDefault().Amount
            };

        }

        public Guid Create(DataAccess.Model.Bank bank)
        {
            CMS_DataContext context = new CMS_DataContext();

            var acct = new DataAccess.Account()
            {
                Id = Guid.NewGuid(),
                Name = bank.Name,
                Category = "BALANCE",
                Type = "ASSET",
                Code = "BANK"
            };


            context.Accounts.InsertOnSubmit(acct);
            context.SubmitChanges();

            return acct.Id;
        }

        public void Update(DataAccess.Model.Bank bank)
        {
            CMS_DataContext context = new CMS_DataContext();

            var acct = context.Accounts.Where(x => x.Id == bank.Id).FirstOrDefault();
            acct.Name = bank.Name;
   
            context.SubmitChanges();
        }

        public void Delete(Guid id)
        {
            CMS_DataContext context = new CMS_DataContext();

            var canDelete = context.Transactions.Where(x => x.CreditAccountId == id || x.DebitAccountId == id).Count() == 0;

            if (canDelete)
            {
                var acct = context.Accounts.Where(x => x.Id == id).FirstOrDefault();

                context.Accounts.DeleteOnSubmit(acct);
                context.SubmitChanges();
            }
            else
            {
                throw new Exception("Cannot delete this bank account, its containing data.");
            }
        }
               
    }
}