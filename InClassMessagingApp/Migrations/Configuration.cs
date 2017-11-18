namespace InClassMessagingApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InClassMessagingApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InClassMessagingApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.SurveyQuestions.AddOrUpdate(
                new Models.SurveyQuestion { SurveyQuestionId=1, QuestionText= "Please indicate your symptom. " ,Options= "(1)Headache, (2)Dizziness, (3)Nausea, (4)Fatigue, (5)Sadness, (0)None" },
                new Models.SurveyQuestion { SurveyQuestionId = 2, QuestionText = "On a scale from 0 (none) to 4 (severe), how would you rate your {0} in the last 24 hours? " }
                );
        }
    }
}
