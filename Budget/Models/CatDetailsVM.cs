using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class CatDetailsVM
    {
        public Category Category { get; set; }
        public decimal BudgetUsed { get; set; }
    }
}