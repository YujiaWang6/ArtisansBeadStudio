namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KeychainUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Keychains", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Keychains", "UserID");
            AddForeignKey("dbo.Keychains", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Keychains", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Keychains", new[] { "UserID" });
            DropColumn("dbo.Keychains", "UserID");
        }
    }
}
