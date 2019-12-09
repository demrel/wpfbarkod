namespace muroLast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StarPack2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Binas",
                c => new
                    {
                        BinaId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BinaId);
            
            AddColumn("dbo.Inventars", "Bina_BinaId", c => c.Int());
            CreateIndex("dbo.Inventars", "Bina_BinaId");
            AddForeignKey("dbo.Inventars", "Bina_BinaId", "dbo.Binas", "BinaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventars", "Bina_BinaId", "dbo.Binas");
            DropIndex("dbo.Inventars", new[] { "Bina_BinaId" });
            DropColumn("dbo.Inventars", "Bina_BinaId");
            DropTable("dbo.Binas");
        }
    }
}
