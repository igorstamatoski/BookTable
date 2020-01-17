namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventadded : DbMigration
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
            
            AddColumn("dbo.Reservations", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservations", "Event_EventId", c => c.Int());
            AddColumn("dbo.Restaurants", "OwnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Restaurants", "Address", c => c.String());
            AlterColumn("dbo.Restaurants", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Reservations", "Event_EventId");
            AddForeignKey("dbo.Reservations", "Event_EventId", "dbo.Events", "EventId");
            DropColumn("dbo.Reservations", "Time");
            DropColumn("dbo.Restaurants", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Restaurants", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "Time", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Reservations", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "RestaurantId_RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Reservations", new[] { "Event_EventId" });
            DropIndex("dbo.Events", new[] { "RestaurantId_RestaurantId" });
            AlterColumn("dbo.Restaurants", "Name", c => c.String());
            DropColumn("dbo.Restaurants", "Address");
            DropColumn("dbo.Restaurants", "OwnerId");
            DropColumn("dbo.Reservations", "Event_EventId");
            DropColumn("dbo.Reservations", "CreatedAt");
            DropTable("dbo.Events");
        }
    }
}
