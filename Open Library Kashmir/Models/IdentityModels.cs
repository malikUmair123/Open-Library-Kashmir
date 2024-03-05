using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.BuilderProperties;
using Open_Library_Kashmir.Migrations;

namespace Open_Library_Kashmir.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public virtual Address Address { get; set; }

        public string AadharImageUrl { get; set; }

        public string Remarks { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //database initializer in the context class to MigrateDatabaseToLatestVersion automatically 
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<DonorBook> DonorBooks { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<RecipientBook> RecipientBooks { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Instead of Writing the Schema in Each ToTable Method we can configure the same
            //Configure default schema as dbo
            modelBuilder.HasDefaultSchema("dbo");

            //Now Comment the below Statements
            modelBuilder.Entity<ApplicationUser>().ToTable("User", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("Role", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Claim", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Login", "dbo");

            //Use the below Statements
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Claim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Login");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }
}