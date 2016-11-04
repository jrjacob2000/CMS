using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.DataAccess.Model
{
    
    public class JsonTransactionResponse
    {
        public int Count { get; set; }
        public object Items { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class JsonBankEntryResponse
    {
        public int Count { get; set; }
        public List<CMS.DataAccess.Model.BankEntry> Items { get; set; }
        public decimal TotalBalance { get; set; }
    }
}