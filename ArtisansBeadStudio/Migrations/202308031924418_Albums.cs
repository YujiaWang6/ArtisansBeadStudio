namespace ArtisansBeadStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Albums : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumID = c.Int(nullable: false, identity: true),
                        AlbumName = c.String(),
                    })
                .PrimaryKey(t => t.AlbumID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Albums");
        }
    }
}
