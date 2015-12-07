using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal? BudgetedAmount { get; set; }
        public decimal? BudgetUsed { get; set; }
        public int? HouseholdId { get; set; }

        public virtual Household Household { get; set; }
        
    }
}