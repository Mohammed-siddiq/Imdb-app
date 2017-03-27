namespace Imdb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sex = c.String(nullable: false),
                        Dob = c.DateTime(nullable: false),
                        Bio = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
