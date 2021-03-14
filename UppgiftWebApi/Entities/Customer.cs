using System;
using System.Collections.Generic;

#nullable disable

namespace UppgiftWebApi.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
