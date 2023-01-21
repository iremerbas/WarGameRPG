namespace WarGameRPG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Health = c.Double(nullable: false),
                        Durability = c.Int(nullable: false),
                        Power = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        Level = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Characters");
        }
    }
}
