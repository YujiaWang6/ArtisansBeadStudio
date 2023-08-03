namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageStyle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "StyleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Images", "StyleID");
            AddForeignKey("dbo.Images", "StyleID", "dbo.Styles", "StyleID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "StyleID", "dbo.Styles");
            DropIndex("dbo.Images", new[] { "StyleID" });
            DropColumn("dbo.Images", "StyleID");
        }
    }
}
