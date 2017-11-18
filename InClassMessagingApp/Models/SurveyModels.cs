using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InClassMessagingApp.Models
{
    public class SurveyResponse
    {
        public string SurveyResponseId { get; set; }
        public string SurveyUserId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyIteration { get; set; }
        public string ResponseText { get; set; }
    }

    public class SurveyQuestion
    {
        public int SurveyQuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Options { get; set; }
    }

    public class SurveyUser
    {
        public string SurveyUserId { get; set; }
        public bool isCompletedSurvey { get; set; }
        public int CurrentQuestion { get; set; }
        public int CurrentIteration { get; set; }
    }


    
}