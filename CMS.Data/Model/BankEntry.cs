using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess.Model
{
    public class BankEntry
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Date { get; set; }
        public Guid TransactionId { get; set; }
    }
}