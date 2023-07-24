namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bead : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beads",
                c => new
                    {
                        BeadId = c.Int(nullable: false, identity: true),
                        BeadName = c.String(),
                        BeadDescription = c.String(),
                        BeadPicture = c.String(),
                        ColourId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeadId)
                .ForeignKey("dbo.BeadColours", t => t.ColourId, cascadeDelete: true)
                .Index(t => t.ColourId);
            
            CreateTable(
                "dbo.BeadColours",
                c => new
                    {
                        ColourId = c.Int(nullable: false, identity: true),
                        ColourName = c.String(),
                        ColourProperty = c.String(),
                    })
                .PrimaryKey(t => t.ColourId);
            
            CreateTable(
                "dbo.Keychains",
                c => new
                    {
                        KeychainId = c.Int(nullable: false, identity: true),
                        KeychainName = c.String(),
                    })
                .PrimaryKey(t => t.KeychainId);
            
            CreateTable(
                "dbo.KeychainBeads",
                c => new
                    {
                        Keychain_KeychainId = c.Int(nullable: false),
                        Bead_BeadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Keychain_KeychainId, t.Bead_BeadId })
                .ForeignKey("dbo.Keychains", t => t.Keychain_KeychainId, cascadeDelete: true)
                .ForeignKey("dbo.Beads", t => t.Bead_BeadId, cascadeDelete: true)
                .Index(t => t.Keychain_KeychainId)
                .Index(t => t.Bead_BeadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeychainBeads", "Bead_BeadId", "dbo.Beads");
            DropForeignKey("dbo.KeychainBeads", "Keychain_KeychainId", "dbo.Keychains");
            DropForeignKey("dbo.Beads", "ColourId", "dbo.BeadColours");
            DropIndex("dbo.KeychainBeads", new[] { "Bead_BeadId" });
            DropIndex("dbo.KeychainBeads", new[] { "Keychain_KeychainId" });
            DropIndex("dbo.Beads", new[] { "ColourId" });
            DropTable("dbo.KeychainBeads");
            DropTable("dbo.Keychains");
            DropTable("dbo.BeadColours");
            DropTable("dbo.Beads");
        }
    }
}
