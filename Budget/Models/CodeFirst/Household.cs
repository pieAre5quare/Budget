using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class Household
    {
        public Household()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Accounts = new HashSet<BankAccount>();
            this.Budgets = new HashSet<Budget>();
            this.Codes = new HashSet<InviteCode>();
            this.Categories = new HashSet<Category>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<BankAccount> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<InviteCode> Codes { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}