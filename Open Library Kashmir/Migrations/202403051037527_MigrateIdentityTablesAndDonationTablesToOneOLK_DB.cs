namespace Open_Library_Kashmir.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateIdentityTablesAndDonationTablesToOneOLK_DB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(maxLength: 100),
                        AddressLine2 = c.String(maxLength: 100),
                        Locality = c.String(maxLength: 100),
                        City = c.String(maxLength: 100),
                        District = c.String(maxLength: 100),
                        State = c.String(maxLength: 100),
                        CountryCode = c.String(maxLength: 2),
                        PostalCode = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Author = c.String(maxLength: 255),
                        Publisher = c.String(maxLength: 255),
                        PublicationYear = c.Int(),
                        Condition = c.String(maxLength: 20),
                        Class = c.String(maxLength: 100),
                        Genre = c.String(maxLength: 100),
                        Subject = c.String(maxLength: 100),
                        ISBN = c.String(maxLength: 50),
                        ImageUrl = c.String(maxLength: 100),
                        ShortDescription = c.String(maxLength: 500),
                        Status = c.String(maxLength: 50),
                        AvailableAt = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.BookId);
            
            CreateTable(
                "dbo.DonorBook",
                c => new
                    {
                        DonorId = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        DateDonated = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => new { t.DonorId, t.BookId })
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Donor", t => t.DonorId, cascadeDelete: true)
                .Index(t => t.DonorId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Donor",
                c => new
                    {
                        DonorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.DonorId);
            
            CreateTable(
                "dbo.RecipientBook",
                c => new
                    {
                        RecipientId = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        DateRecieved = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => new { t.RecipientId, t.BookId })
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Recipient", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.RecipientId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Recipient",
                c => new
                    {
                        RecipientId = c.String(nullable: false, maxLength: 128),
                        AadharCardUrl = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RecipientId);
            
            CreateTable(
                "dbo.Wishlist",
                c => new
                    {
                        WishlistId = c.Int(nullable: false, identity: true),
                        RecipientId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.WishlistId)
                .ForeignKey("dbo.Recipient", t => t.RecipientId, cascadeDelete: true)
                .Index(t => t.RecipientId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        AadharImageUrl = c.String(),
                        Remarks = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Address_AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.Address_AddressId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Address_AddressId);
            
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Login",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WishlistBooks",
                c => new
                    {
                        Wishlist_WishlistId = c.Int(nullable: false),
                        Book_BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Wishlist_WishlistId, t.Book_BookId })
                .ForeignKey("dbo.Wishlist", t => t.Wishlist_WishlistId, cascadeDelete: true)
                .ForeignKey("dbo.Book", t => t.Book_BookId, cascadeDelete: true)
                .Index(t => t.Wishlist_WishlistId)
                .Index(t => t.Book_BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.Login", "UserId", "dbo.User");
            DropForeignKey("dbo.Claim", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "Address_AddressId", "dbo.Address");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Wishlist", "RecipientId", "dbo.Recipient");
            DropForeignKey("dbo.WishlistBooks", "Book_BookId", "dbo.Book");
            DropForeignKey("dbo.WishlistBooks", "Wishlist_WishlistId", "dbo.Wishlist");
            DropForeignKey("dbo.RecipientBook", "RecipientId", "dbo.Recipient");
            DropForeignKey("dbo.RecipientBook", "BookId", "dbo.Book");
            DropForeignKey("dbo.DonorBook", "DonorId", "dbo.Donor");
            DropForeignKey("dbo.DonorBook", "BookId", "dbo.Book");
            DropIndex("dbo.WishlistBooks", new[] { "Book_BookId" });
            DropIndex("dbo.WishlistBooks", new[] { "Wishlist_WishlistId" });
            DropIndex("dbo.Login", new[] { "UserId" });
            DropIndex("dbo.Claim", new[] { "UserId" });
            DropIndex("dbo.User", new[] { "Address_AddressId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.Wishlist", new[] { "RecipientId" });
            DropIndex("dbo.RecipientBook", new[] { "BookId" });
            DropIndex("dbo.RecipientBook", new[] { "RecipientId" });
            DropIndex("dbo.DonorBook", new[] { "BookId" });
            DropIndex("dbo.DonorBook", new[] { "DonorId" });
            DropTable("dbo.WishlistBooks");
            DropTable("dbo.Login");
            DropTable("dbo.Claim");
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.Wishlist");
            DropTable("dbo.Recipient");
            DropTable("dbo.RecipientBook");
            DropTable("dbo.Donor");
            DropTable("dbo.DonorBook");
            DropTable("dbo.Book");
            DropTable("dbo.Address");
        }
    }
}
