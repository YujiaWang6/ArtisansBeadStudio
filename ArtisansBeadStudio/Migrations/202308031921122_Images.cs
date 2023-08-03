namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ImageTitle = c.String(),
                        ImageDescription = c.String(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.ImageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Images");
        }
    }
}
