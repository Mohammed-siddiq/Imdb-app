namespace Imdb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yearofreleaseInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "YearOfRelease", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "YearOfRelease", c => c.String(nullable: false));
        }
    }
}
