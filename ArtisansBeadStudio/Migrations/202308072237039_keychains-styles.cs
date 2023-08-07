namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keychainsstyles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StyleKeychains",
                c => new
                    {
                        Style_StyleID = c.Int(nullable: false),
                        Keychain_KeychainId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Style_StyleID, t.Keychain_KeychainId })
                .ForeignKey("dbo.Styles", t => t.Style_StyleID, cascadeDelete: true)
                .ForeignKey("dbo.Keychains", t => t.Keychain_KeychainId, cascadeDelete: true)
                .Index(t => t.Style_StyleID)
                .Index(t => t.Keychain_KeychainId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StyleKeychains", "Keychain_KeychainId", "dbo.Keychains");
            DropForeignKey("dbo.StyleKeychains", "Style_StyleID", "dbo.Styles");
            DropIndex("dbo.StyleKeychains", new[] { "Keychain_KeychainId" });
            DropIndex("dbo.StyleKeychains", new[] { "Style_StyleID" });
            DropTable("dbo.StyleKeychains");
        }
    }
}
