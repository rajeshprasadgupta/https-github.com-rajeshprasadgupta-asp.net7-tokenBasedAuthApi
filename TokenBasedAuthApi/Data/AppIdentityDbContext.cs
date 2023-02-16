using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyServiceApi.Data
{
    public class AppIdentityDbContext :
IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppIdentityDbContext
           (DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
