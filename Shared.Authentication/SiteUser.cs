using System;
using Microsoft.AspNetCore.Identity;


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


namespace Shared.Authentication
{


    public class SiteUser : IdentityUser
    {
        public string Name { get; set; } 
         
        public string IpAddress { get; set; }

        public Guid CustomerId { get; set; }
    }
}
