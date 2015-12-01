using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class Budget
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public int HouseholdId { get; set; }
        public int CategoryId { get; set; }

        public virtual Household Household { get; set; }
        public virtual Category Category { get; set; }
    }
}