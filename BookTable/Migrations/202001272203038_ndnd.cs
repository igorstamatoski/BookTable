namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ndnd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Restaurants", "Category", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Restaurants", "Category", c => c.String(nullable: false));
        }
    }
}
