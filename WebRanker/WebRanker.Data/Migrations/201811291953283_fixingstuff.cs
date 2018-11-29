namespace WebRanker.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingstuff : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ItemList", newName: "Collection");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Collection", newName: "ItemList");
        }
    }
}
