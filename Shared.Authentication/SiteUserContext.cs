using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Persistency;

namespace Shared.Authentication
{
    public class SiteUserContext : IdentityDbContext<SiteUser>
    { 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(DatabaseConfiguration.GetConnectionString(),
                   options => options.EnableRetryOnFailure());
          
         } 
    }
}