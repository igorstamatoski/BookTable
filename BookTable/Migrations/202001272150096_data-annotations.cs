namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataannotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Restaurants", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Restaurants", "Category", c => c.String());
            AlterColumn("dbo.Events", "Description", c => c.String());
        }
    }
}
