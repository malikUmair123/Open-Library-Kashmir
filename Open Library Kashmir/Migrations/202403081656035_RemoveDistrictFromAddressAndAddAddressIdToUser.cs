namespace Open_Library_Kashmir.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDistrictFromAddressAndAddAddressIdToUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Address_AddressId", "dbo.Address");
            DropIndex("dbo.User", new[] { "Address_AddressId" });
            RenameColumn(table: "dbo.User", name: "Address_AddressId", newName: "AddressId");
            AlterColumn("dbo.User", "AddressId", c => c.Int(nullable: false));
            CreateIndex("dbo.User", "AddressId");
            AddForeignKey("dbo.User", "AddressId", "dbo.Address", "AddressId", cascadeDelete: true);
            DropColumn("dbo.Address", "District");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Address", "District", c => c.String(maxLength: 100));
            DropForeignKey("dbo.User", "AddressId", "dbo.Address");
            DropIndex("dbo.User", new[] { "AddressId" });
            AlterColumn("dbo.User", "AddressId", c => c.Int());
            RenameColumn(table: "dbo.User", name: "AddressId", newName: "Address_AddressId");
            CreateIndex("dbo.User", "Address_AddressId");
            AddForeignKey("dbo.User", "Address_AddressId", "dbo.Address", "AddressId");
        }
    }
}
