using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAdminApi.Models
{
    public class IndexViewModel
    {
       
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Service> Services { get; set; }
     
        public PageViewModel PageViewModel { get; set; }
    }
}
