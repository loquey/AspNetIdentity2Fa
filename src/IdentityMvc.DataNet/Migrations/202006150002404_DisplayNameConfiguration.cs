namespace IdentityMvc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayNameConfiguration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "UserAccounts");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.UserAccounts", "UserNameIndex");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            AddColumn("dbo.AspNetUserRoles", "UserAccount_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserClaims", "UserAccount_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserLogins", "UserAccount_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserAccounts", "DisplayName", c => c.String(maxLength: 100));
            AlterColumn("dbo.UserAccounts", "Email", c => c.String());
            AlterColumn("dbo.UserAccounts", "UserName", c => c.String());
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String());
            CreateIndex("dbo.AspNetUserRoles", "UserAccount_Id");
            CreateIndex("dbo.AspNetUserClaims", "UserAccount_Id");
            CreateIndex("dbo.AspNetUserLogins", "UserAccount_Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserAccount_Id", "dbo.UserAccounts", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserAccount_Id", "dbo.UserAccounts", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserAccount_Id", "dbo.UserAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserAccount_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.AspNetUserLogins", "UserAccount_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.AspNetUserClaims", "UserAccount_Id", "dbo.UserAccounts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserAccount_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserAccount_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserAccount_Id" });
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserAccounts", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserAccounts", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.UserAccounts", "DisplayName", c => c.String());
            DropColumn("dbo.AspNetUserLogins", "UserAccount_Id");
            DropColumn("dbo.AspNetUserClaims", "UserAccount_Id");
            DropColumn("dbo.AspNetUserRoles", "UserAccount_Id");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.UserAccounts", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserAccounts", newName: "AspNetUsers");
        }
    }
}
