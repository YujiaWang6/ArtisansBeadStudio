namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Styles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Styles",
                c => new
                    {
                        StyleID = c.Int(nullable: false, identity: true),
                        StyleName = c.String(),
                    })
                .PrimaryKey(t => t.StyleID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Styles");
        }
    }
}
