namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDataannotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Restaurants", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Restaurants", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Restaurants", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Restaurants", "Address", c => c.String());
            AlterColumn("dbo.Restaurants", "Description", c => c.String());
            AlterColumn("dbo.Restaurants", "Category", c => c.String());
            AlterColumn("dbo.Events", "Description", c => c.String());
        }
    }
}
