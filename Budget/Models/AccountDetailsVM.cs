using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class AccountDetailsVM
    {
        public BankAccount BankAccount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}