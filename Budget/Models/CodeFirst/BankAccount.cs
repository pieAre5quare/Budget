using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    [Serializable]
    public class BankAccount
    {
        public BankAccount()
        {
            this.Transactions = new HashSet<Transaction>();
            this.IsArchived = false;
        }
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public int HouseholdId { get; set; }
        public bool IsArchived { get; set; }


        public virtual Household Household { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}