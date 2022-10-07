using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApp.Infrastructure.Entities;

namespace UserApp.Infrastructure.DatabaseContexts
{
    public class ApplicationDbContext :
        IdentityDbContext<User>
    {
        public ApplicationDbContext(
             DbContextOptions<ApplicationDbContext> dbContextOptions
        ) : base( dbContextOptions )
        {

        }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            base.OnModelCreating( builder );
        }

        //public DbSet<User> Users { get; set; }

    }
}
