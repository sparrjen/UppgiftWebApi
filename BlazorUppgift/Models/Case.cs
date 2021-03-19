using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUppgift.Models
{
    public class Case
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AdministratorId { get; set; }
        public DateTime CaseDate { get; set; }
        public DateTime? UpdateCaseDate { get; set; }
        public string CaseDescription { get; set; }
        public string CaseStatus { get; set; }
        public virtual Administrator Administrator { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
