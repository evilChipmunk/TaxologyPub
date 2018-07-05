using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Taxology.Site.Models
{
    public class SiteUserModel : IdentityUser
    {
        public string IPAddress { get; set; }
    }
}
