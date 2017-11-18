namespace InClassMessagingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Iteration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyResponses", "SurveyIteration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SurveyResponses", "SurveyIteration");
        }
    }
}
