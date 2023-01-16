using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace StartmenuRecoveryWS.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<StartmenuRecoveryWS.Data.Shortcut> Shortcuts { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string ADMIN_ID ="9ef639b9-2157-4f08-a07d-349b0f399999";
            string ROLE_ID = "9ef639b9-2157-4f08-a07d-349b0f38833b";


            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "Administrators",
                NormalizedName = "ADMINS"
            });

            //create user
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "Admin",
                NormalizedUserName = "admin@app.local",
                Email = "admin@app.local",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123$"),
                SecurityStamp = string.Empty
            });

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = ADMIN_ID,
                RoleId = ROLE_ID
            });
        }
    }
}