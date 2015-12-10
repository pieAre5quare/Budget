using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public bool IsDeposit { get; set; }
        public bool IsReconciled { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTimeOffset Date { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual Category Category { get; set; }


    }
}