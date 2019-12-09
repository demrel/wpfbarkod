namespace muroLast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StarPack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventars",
                c => new
                    {
                        InventarID = c.Int(nullable: false, identity: true),
                        SN = c.String(),
                        Barcode = c.String(),
                        Count = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Person = c.String(),
                        Note = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Item_ItemID = c.Int(),
                        Room_RoomID = c.Int(),
                    })
                .PrimaryKey(t => t.InventarID)
                .ForeignKey("dbo.Items", t => t.Item_ItemID)
                .ForeignKey("dbo.Rooms", t => t.Room_RoomID)
                .Index(t => t.Item_ItemID)
                .Index(t => t.Room_RoomID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ItemID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoomID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventars", "Room_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Inventars", "Item_ItemID", "dbo.Items");
            DropIndex("dbo.Inventars", new[] { "Room_RoomID" });
            DropIndex("dbo.Inventars", new[] { "Item_ItemID" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Items");
            DropTable("dbo.Inventars");
        }
    }
}
