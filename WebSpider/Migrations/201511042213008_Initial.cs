namespace WebSpider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Pages", "PageTitle", t => t.String());                        
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.Pages", "PageTitle");
        }
    }
}
