namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "AlbumID", c => c.Int(nullable: false));
            CreateIndex("dbo.Images", "AlbumID");
            AddForeignKey("dbo.Images", "AlbumID", "dbo.Albums", "AlbumID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "AlbumID", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "AlbumID" });
            DropColumn("dbo.Images", "AlbumID");
        }
    }
}
