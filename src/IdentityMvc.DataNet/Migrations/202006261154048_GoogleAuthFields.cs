namespace IdentityMvc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoogleAuthFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "IsGoogleAuthenticatorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserAccounts", "GoogleAuthenticatorSecretKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "GoogleAuthenticatorSecretKey");
            DropColumn("dbo.UserAccounts", "IsGoogleAuthenticatorEnabled");
        }
    }
}
