using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UppgiftWebApi.Models
{
    public class RequestAdmin
    {
        public int AdminId { get; set; }
        public string AccessToken { get; set; }
    }
}
