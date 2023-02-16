using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TokenBasedAuthApi.Data
{
    //Inherit from IdentiyDbContext, to use ASP.NET Core Identity tables for user and roles
    public class AppDbContext :IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        //feed in Two test users and two test roles in the database
        //user "User" has "user" has user role
        //user "admin has "admin" role
        //User with "admin" role can only update database via IdentityEndpoints
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "d1b5952a-2162-46c7-b29e-1a2a68922c14",
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    },
                    new IdentityRole
                    {
                        Id = "42458d3e-3c22-45e1-be81-6caa7ba865ef",
                        Name = "User",
                        NormalizedName = "USER"
                    }
                );
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(
                    new IdentityUser
                    {
                        Id = "fa939cb1-bd0e-4984-a8b2-18a749c843d5",
                        Email = "admin@localhost.com",
                        NormalizedEmail = "ADMIN@LOCALHOST.COM",
                        NormalizedUserName = "ADMIN",
                        UserName = "admin",
                        PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                        EmailConfirmed = true
                    },
                    new IdentityUser
                    {
                        Id = "29ab7a0d-f2c4-486d-9562-de9465ac50f7",
                        Email = "user@localhost.com",
                        NormalizedEmail = "USER@LOCALHOST.COM",
                        NormalizedUserName = "USER",
                        UserName = "user",
                        PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                        EmailConfirmed = true
                    }

                );
            builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "d1b5952a-2162-46c7-b29e-1a2a68922c14",
                        UserId = "fa939cb1-bd0e-4984-a8b2-18a749c843d5"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "42458d3e-3c22-45e1-be81-6caa7ba865ef",
                        UserId = "29ab7a0d-f2c4-486d-9562-de9465ac50f7"
                    }
                );
            //feed in some test data for testing fetching of data via FunctionalEnpoints works only when Request is sent with right token
            builder.Entity<ServiceType>().HasData(
                new ServiceType { 
                    Id= 1,
                    Name = "Service Type 1",
                    Description = "Service Type 1"
                },
                new ServiceType
                {
                    Id = 2,
                    Name = "Service Type 2",
                    Description = "Service Type 2"
                },
                new ServiceType
                {
                    Id = 3,
                    Name = "Service Type 3",
                    Description = "Service Type 3"
                },
                new ServiceType
                {
                     Id = 4,
                     Name = "Service Type 4",
                     Description = "Service Type 4"
                },
                new ServiceType
                {
                    Id = 5,
                    Name = "Service Type 5",
                    Description = "Service Type 5"
                }

            );
        }
    }
}
