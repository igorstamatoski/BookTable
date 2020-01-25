namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restaurantmodelchanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Restaurants", "OwnerId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Restaurants", "OwnerId", c => c.Int(nullable: false));
        }
    }
}
