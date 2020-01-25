namespace BookTable.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Idto", c => c.String());
            DropColumn("dbo.Reservations", "User_Email");
            DropColumn("dbo.Reservations", "User_Password");
            DropColumn("dbo.Reservations", "User_RememberMe");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "User_RememberMe", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reservations", "User_Password", c => c.String(nullable: false));
            AddColumn("dbo.Reservations", "User_Email", c => c.String(nullable: false));
            DropColumn("dbo.Reservations", "Idto");
        }
    }
}
