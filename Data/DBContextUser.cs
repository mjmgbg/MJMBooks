using Microsoft.AspNet.Identity.EntityFramework;

namespace Data
{
    public class DbContextUser : IdentityDbContext<ApplicationUser>
    {
        public DbContextUser()
            : base("name=MJMBooks")
        {
        }

        public static DbContextUser Create()
        {
            return new DbContextUser();
        }
    }
}