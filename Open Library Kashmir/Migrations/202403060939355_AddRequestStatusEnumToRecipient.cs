namespace Open_Library_Kashmir.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestStatusEnumToRecipient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipient", "RequestStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipient", "RequestStatus");
        }
    }
}
