namespace ArrangeGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Int(nullable: false),
                        Player = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Ip = c.String(),
                        Moves = c.Int(),
                        GameId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GameHistories");
        }
    }
}
