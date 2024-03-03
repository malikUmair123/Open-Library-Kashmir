using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Open_Library_Kashmir.Models
{
    public partial class BookDonationDataModels : DbContext
    {
        public BookDonationDataModels()
            : base("name=BookDonationDBContext")
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Distribution> Distributions { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Publisher)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Condition)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Class)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Genre)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.ISBN)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Image_Path)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Short_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Available_At)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Distributions)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Donations)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Wishlists)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("WishlistBook").MapLeftKey("BookId").MapRightKey("WishlistId"));

            modelBuilder.Entity<Distribution>()
                .Property(e => e.Recipient_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Donation>()
                .Property(e => e.Donor_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Donor_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.First_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Last_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Aadhar_Card_Path)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<Donor>()
                .HasMany(e => e.Donations)
                .WithRequired(e => e.Donor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Recipient_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.First_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Last_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Aadhar_Card_Path)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<Recipient>()
                .HasMany(e => e.Distributions)
                .WithRequired(e => e.Recipient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Recipient>()
                .HasMany(e => e.Wishlists)
                .WithRequired(e => e.Recipient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wishlist>()
                .Property(e => e.Recipient_ID)
                .IsUnicode(false);
        }
    }
}
