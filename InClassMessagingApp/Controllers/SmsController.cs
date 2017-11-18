using InClassMessagingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace InClassMessagingApp.Controllers
{
    public class SmsController : TwilioController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        readonly String AccountSid = "AC562f53f4ccf57ddf77944dc675e543d2";
        readonly String AuthToken = "d3892e5c907e4ed8829e1b2f3921c79f";
        Dictionary<string, string> symtomsList = new Dictionary<string, string> { { "1","Headache" }, { "2", "Dizziness" }, { "3", "Nausea" }, { "4", "Fatigue" }, { "5", "Sadness" }, };

        public ActionResult SendSms()
        {
            // var accountSid = "AC562f53f4ccf57ddf77944dc675e543d2";
            // var authToken = "";
            var questions = db.SurveyQuestions;
            Console.Write(questions);
            TwilioClient.Init(AccountSid, AuthToken);
            var to = new PhoneNumber("+19805980003");
            var from = new PhoneNumber("+19808192819");
            var message = MessageResource.Create(to: to, from: from, body: "This is the test message");

            string s = "";
            foreach (var q in questions)
            {
                s += q;
            }
            return Content(message.Sid + s);

        }

        public ActionResult ReceiveSms()
        {
            string requestBody = Request.Form["Body"];
            string userPhone = Request.Form["From"];

            if (userPhone == String.Empty)
                return null;

            var user = db.SurveyUsers.Where(u => u.SurveyUserId.Equals(userPhone)).FirstOrDefault();

            var response = new MessagingResponse();

            if (user != null)
                HandleRegisterUser(user, requestBody);
            else
               RegisterUser(userPhone, requestBody);
        
            return null;

          
        }

        private void RegisterUser(string userPhone, string requestBody) {

            var response = new MessagingResponse();

            if (requestBody.ToLower().Equals("start"))
            {
                try
                {
                    db.SurveyUsers.Add(new SurveyUser { SurveyUserId = userPhone, isCompletedSurvey = false, CurrentQuestion = 1, CurrentIteration = 1 });
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }


                //send Wellcome message
                SendMessage(userPhone, "Welcome to the study");

                var question = db.SurveyQuestions.Where(q => q.SurveyQuestionId ==1).FirstOrDefault();

                if (question != null)
                    SendMessage(userPhone, question.QuestionText + question.Options);

            }
            else {

                SendMessage(userPhone, "You are not currently enrolled to our survey. Reply <START> to get enrolled.");
            }



        }

        private void HandleRegisterUser(SurveyUser user, string response) {
            if (user.isCompletedSurvey)
                return;
            else {

                switch (user.CurrentQuestion) {
                    case 1:
                        HandleQuestion1(user, response);
                        break;
                    case 2:
                        HandleQuestion2(user, response);
                        break;
                    default:
                        break;
                }

            }
        }
        private void HandleQuestion1(SurveyUser user, string response) {

            saveUserResponse(user, response);

            switch (response)
            {
                case "0":
                    SendMessage(user.SurveyUserId, "Thank you and we will check with you later.");
                    user.isCompletedSurvey = true;
                    updateUser(user);
                    break;
                case "1":
                    SendMessage(user.SurveyUserId, "On a scale from 0 (none) to 4 (severe), how would you rate your Headache in the last 24 hours?");
                    user.CurrentQuestion = 2;
                    updateUser(user);
                    break;
                case "2":
                    SendMessage(user.SurveyUserId, "On a scale from 0 (none) to 4 (severe), how would you rate your Dizziness in the last 24 hours?");
                    user.CurrentQuestion = 2;
                    updateUser(user);
                    break;
                case "3":
                    SendMessage(user.SurveyUserId, "On a scale from 0 (none) to 4 (severe), how would you rate your Nausea in the last 24 hours?");
                    user.CurrentQuestion = 2;
                    updateUser(user);
                    break;
                case "4":
                    SendMessage(user.SurveyUserId, "On a scale from 0 (none) to 4 (severe), how would you rate your Fatigue in the last 24 hours?");
                    user.CurrentQuestion = 2;
                    updateUser(user);
                    break;
                case "5":
                    SendMessage(user.SurveyUserId, "On a scale from 0 (none) to 4 (severe), how would you rate your Sadness in the last 24 hours?");
                    user.CurrentQuestion = 2;
                    updateUser(user);
                    break;
                default:
                    SendMessage(user.SurveyUserId, "Please enter a number from 0 to 5");
                    break;
            }

        }

        private void HandleQuestion2(SurveyUser user, string response)
        {

            if (isValidResponse(response))
            {
                saveUserResponse(user, response);

                var userResponse = db.SurveyResponses.Where(r => r.SurveyUserId.Equals(user.SurveyUserId) && r.SurveyQuestionId == 1 && r.SurveyIteration == user.CurrentIteration).FirstOrDefault();
                var symptom = symtomsList[userResponse.ResponseText];

                switch (response)
                {
                    case "0":
                        SendMessage(user.SurveyUserId, "You do not have a " + symptom);
                        HandleIteration(user);
                        break;
                    case "1":
                    case "2":
                        SendMessage(user.SurveyUserId, "You have a mild " + symptom);
                        HandleIteration(user);
                        break;
                    case "3":
                        SendMessage(user.SurveyUserId, "You have a moderate " + symptom);
                        HandleIteration(user);
                        break;
                    case "4":
                        SendMessage(user.SurveyUserId, "You have a severe " + symptom);
                        HandleIteration(user);
                        break;
                    default:
                        SendMessage(user.SurveyUserId, "Please enter a number from 0 to 4");
                        break;
                }
            }
            else {

                SendMessage(user.SurveyUserId, "Please enter a number from 0 to 4");
            }

           

        }

        private void HandleIteration(SurveyUser user) {
            if (user.CurrentIteration < 3)
            {
                SendMessage(user.SurveyUserId, "Please indicate your symptom (1)Headache, (2)Dizziness, (3)Nausea, (4)Fatigue, (5)Sadness, (0)None");
                user.CurrentIteration++;
                user.CurrentQuestion = 1;
                updateUser(user);
            }
            else if (user.CurrentIteration == 3) {
                SendMessage(user.SurveyUserId, "Thank you and see you soon");
                user.isCompletedSurvey = true;
                updateUser(user);
            }
        }

        private void updateUser(SurveyUser user) {
            try
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool isValidResponse(string reponse) {

            List<string> question2Responses = new List<string> { "0", "1", "2", "3", "4" };

            return question2Responses.Contains(reponse);

        }

        private void saveUserResponse(SurveyUser user, string response) {
            List<string> question1Responses = new List<string> { "0", "1", "2", "3", "4", "5" };
            List<string> question2Responses = new List<string> { "0", "1", "2", "3", "4" };
            if ((user.CurrentQuestion == 1  && question1Responses.Contains(response)) || (user.CurrentQuestion == 2 && question2Responses.Contains(response)))
            {
                    db.SurveyResponses.Add(new SurveyResponse {SurveyResponseId = Guid.NewGuid().ToString(), SurveyUserId = user.SurveyUserId, SurveyQuestionId = user.CurrentQuestion, SurveyIteration = user.CurrentIteration, ResponseText = response });
            }
        }

        private void SendMessage(string toPhone , string messageBody)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var to = new PhoneNumber(toPhone);
            var from = new PhoneNumber("+19808192819");
            var message = MessageResource.Create(to: to, from: from, body: messageBody);
        }

    }
}