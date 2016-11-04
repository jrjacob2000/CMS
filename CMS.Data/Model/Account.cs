using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
    }

    public class Bank
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Decimal Amount { get; set; }
    }

    public class BalanceFund
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public List<Fund> Unrestricted { get; set; }
        public List<Fund> Restricted { get; set; }
    }

    public class BankActivity
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string BankName { get; set; }
        public string TransactionType { get; set; }
        public string AccountName { get; set; } 
    }

    public class Fund
    {
        public Guid Id { get; set; }
        public string FundType { get; set; }
        public Account Account { get; set; }
        public Account Parent { get; set; }
        public Decimal Amount { get; set; }
    }
}