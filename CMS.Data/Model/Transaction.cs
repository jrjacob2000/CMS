using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public Account CreditAccount { get; set; }
        public Account DebitAccount { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
    }
}