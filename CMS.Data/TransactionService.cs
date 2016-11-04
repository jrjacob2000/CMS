using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CMS.DataAccess.Model;

namespace CMS.DataAccess
{
    public class TransactionService
    {
        public Guid Create(Model.Transaction transEntity)
        {
            var context = new CMS_DataContext();
            var trans = new DataAccess.Transaction() { 
                Id = transEntity.Id,
                Reference = transEntity.Reference,
                Notes = transEntity.Notes,
                CreditAccountId = transEntity.CreditAccount.Id,
                DebitAccountId = transEntity.DebitAccount.Id,
                Amount = transEntity.Amount,
                Date = DateTime.Parse(transEntity.Date)
            };
                       
          
            try
            {
                context.Transactions.InsertOnSubmit(trans);
                context.SubmitChanges();
            }
            catch (Exception ex)
            { throw ex; }
            return transEntity.Id;
        }

        public object GetContributions(DateTime? dateFilter, int page)
        {
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Transaction>(t => t.CreditAccount);

            decimal totalBal = 0;
            var pageSize = 10;
            var context = new CMS_DataContext();
            context.LoadOptions = dlo;


            var allItems = context.Transactions;
            var items = context.Transactions.Where(x => x.CreditAccount.Category.ToUpper() == "FUNDACTIVITY" && x.CreditAccount.Type.ToUpper() == "INCOME" && (x.Date == dateFilter || dateFilter == null));
            var count = items.Count();
            if (count > 0)
                totalBal = allItems.Sum(x => x.Amount);

            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new Model.Transaction
                {
                    Id = x.Id,
                    Reference = x.Reference,
                    Notes = x.Notes,
                    CreditAccount = new Model.Account() { Id = x.CreditAccount.Id, Name = x.CreditAccount.Name },
                    DebitAccount = new Model.Account() { Id = x.DebitAccount.Id, Name = x.DebitAccount.Name },
                    Amount = x.Amount,
                    Date = x.Date.ToShortDateString()
                }).ToList();

            return new JsonTransactionResponse()
            {
                Count = count,
                Items = pagedItems,
                TotalBalance = totalBal
            };
        }

        public object GetExpenses(DateTime? dateFilter, int page)
        {
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Transaction>(t => t.CreditAccount);

            decimal totalBal = 0;
            var pageSize = 10;
            var context = new CMS_DataContext();
            context.LoadOptions = dlo;


            var allItems = context.Transactions;
            var items = context.Transactions.Where(x => (x.DebitAccount.Category.ToUpper() == "FUNDACTIVITY" && x.DebitAccount.Type.ToUpper() == "EXPENSE" && (x.Date == dateFilter || dateFilter == null)) || x.DebitAccount.Fund.FundType == "RESTRICTED");
            var count = items.Count();
            if (count > 0)
                totalBal = allItems.Sum(x => x.Amount);

            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new Model.Transaction
                {
                    Id = x.Id,
                    Reference = x.Reference,
                    Notes = x.Notes,
                    CreditAccount = new Model.Account() { Id = x.CreditAccount.Id, Name = x.CreditAccount.Name },
                    DebitAccount = new Model.Account() { Id = x.DebitAccount.Id, Name = x.DebitAccount.Name },
                    Amount = x.Amount,
                    Date = x.Date.ToShortDateString()
                }).ToList();

            return new JsonTransactionResponse()
            {
                Count = count,
                Items = pagedItems,
                TotalBalance = totalBal
            };
        }

        //public object GetByTransactionType(string type,DateTime? dateFilter, int page)
        //{
        //    DataLoadOptions dlo = new DataLoadOptions();
        //    dlo.LoadWith<Transaction>(t => t.Account);

        //    decimal totalBal = 0;
        //    var pageSize = 10;
        //    var context = new CMS_DataContext();
        //    context.LoadOptions = dlo;
        //    var allItems = context.Transactions;
        //    var items = context.Transactions.Where(x => x.TransactionType == type && (x.Date == dateFilter || dateFilter == null));
        //    var count = items.Count();
        //    if(count > 0)
        //        totalBal = allItems.Sum(x => x.Amount);

        //    var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize)
        //        .Select(x => new Model.Transaction { 
        //            Id = x.Id,
        //            Reference = x.Reference,
        //            Notes = x.Notes,
        //            TransactionType = x.TransactionType,
        //            Account = new Model.Account(){ Id= x.Account.Id, Description = x.Account.Description },
        //            Amount = x.Amount,
        //            Date = x.Date.ToShortDateString()
        //        }).ToList();

        //    return new JsonTransactionResponse()
        //    {
        //        Count = count,
        //        Items = pagedItems,
        //        TotalBalance = totalBal
        //    };
        //}

        public Transaction Get(Guid id)
        {
            var context = new CMS_DataContext();
            return context.Transactions.Where(x => x.Id == id).FirstOrDefault();
        }

        public Guid Update(Model.Transaction tranEntity)
        {
            var context = new CMS_DataContext();
            var trans = context.Transactions.Where(x => x.Id == tranEntity.Id).FirstOrDefault();

            trans.Reference = tranEntity.Reference;
            trans.Amount = tranEntity.Amount;
            trans.Notes = tranEntity.Notes;
            trans.CreditAccountId = tranEntity.CreditAccount.Id;
            trans.DebitAccountId = tranEntity.DebitAccount.Id;
            trans.Date = DateTime.Parse(tranEntity.Date);

            context.SubmitChanges();
            return tranEntity.Id;
        }

        public void Delete(Guid id)
        {
            try
            {
                var context = new CMS_DataContext();
                var trans = context.Transactions.Where(x => x.Id == id).FirstOrDefault();

                context.Transactions.DeleteOnSubmit(trans);
                context.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}