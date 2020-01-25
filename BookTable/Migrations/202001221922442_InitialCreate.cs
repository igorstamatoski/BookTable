namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        RestaurantId_RestaurantId = c.Int(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId_RestaurantId)
                .Index(t => t.RestaurantId_RestaurantId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        OwnerId = c.String(),
                        Approved = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        Category = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        User_Email = c.String(nullable: false),
                        User_Password = c.String(nullable: false),
                        User_RememberMe = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Event_EventId = c.Int(),
                        Table_TableId = c.Int(),
                    })
                .PrimaryKey(t => t.ReservationId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.Tables", t => t.Table_TableId)
                .Index(t => t.Event_EventId)
                .Index(t => t.Table_TableId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        Seats = c.Int(nullable: false),
                        Avaliable = c.Boolean(nullable: false),
                        Restaurant_RestaurantId = c.Int(),
                    })
                .PrimaryKey(t => t.TableId)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantId)
                .Index(t => t.Restaurant_RestaurantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Table_TableId", "dbo.Tables");
            DropForeignKey("dbo.Tables", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Reservations", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "RestaurantId_RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Tables", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.Reservations", new[] { "Table_TableId" });
            DropIndex("dbo.Reservations", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "RestaurantId_RestaurantId" });
            DropTable("dbo.Tables");
            DropTable("dbo.Reservations");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Events");
        }
    }
}
