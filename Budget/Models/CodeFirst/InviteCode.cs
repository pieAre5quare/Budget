using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class InviteCode
    {
 
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public Guid Code { get; set; }

        public virtual Household Household { get; set; }
    }
}