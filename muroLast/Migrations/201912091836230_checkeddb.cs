namespace muroLast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkeddb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inventars", "Checked_CheckedID", "dbo.Checkeds");
            DropIndex("dbo.Inventars", new[] { "Checked_CheckedID" });
            AddColumn("dbo.Checkeds", "Inventar_InventarID", c => c.Int());
            CreateIndex("dbo.Checkeds", "Inventar_InventarID");
            AddForeignKey("dbo.Checkeds", "Inventar_InventarID", "dbo.Inventars", "InventarID");
            DropColumn("dbo.Inventars", "Checked_CheckedID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inventars", "Checked_CheckedID", c => c.Int());
            DropForeignKey("dbo.Checkeds", "Inventar_InventarID", "dbo.Inventars");
            DropIndex("dbo.Checkeds", new[] { "Inventar_InventarID" });
            DropColumn("dbo.Checkeds", "Inventar_InventarID");
            CreateIndex("dbo.Inventars", "Checked_CheckedID");
            AddForeignKey("dbo.Inventars", "Checked_CheckedID", "dbo.Checkeds", "CheckedID");
        }
    }
}
