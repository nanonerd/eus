using System.Web.Mvc;

namespace eus.UI.Areas.Vote
{
    public class VoteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Vote";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AnswersController",
                "Vote/Answers/{action}",
                new { controller = "Answers", action = "Index"},  // TODO: change Index to HandleNotSpecified to direct to a "not found" page
                new[] { "eus.UI.Areas.Vote.Controllers" }
            );

            //context.MapRoute(
            //    "TopicController",
            //    "Vote/Home/{action}",
            //    new { controller = "Home", action = "Index" },  // TODO: change Index to HandleNotSpecified to direct to a "not found" page
            //    new[] { "eus.UI.Areas.Vote.Controllers" }
            //);           

           

            context.MapRoute(
               "TopicController",
               "Vote/Topic/{action}/{id}",
               new { controller = "Topic", action = "Index", id = UrlParameter.Optional },
               new[] { "eus.UI.Areas.Vote.Controllers" }
             );

            //context.MapRoute(
            //  "AnswerComments",
            //  "Vote/comment/{id}",
            //  new { controller = "Comment", action = "IndexVote" },
            //  new[] { "eus.UI.Controllers" }
            //);

            context.MapRoute(
              "AnswerComments",
              "Vote/{answerTitle}/Comment/{id}",
              new { controller = "Comment", action = "IndexVote" },
              new[] { "eus.UI.Controllers" }
            );

            context.MapRoute(
              "AnswerCommentsGet",
              "Vote/{answerTitle}/Comment/Get/{id}",
              new { controller = "Comment", action = "GetComments" },
              new[] { "eus.UI.Controllers" }
            );

            context.MapRoute(
              "AnswerCommentsPost",
              "Vote/{answerTitle}/Comment/Post/{id}",
              new { controller = "Comment", action = "PostComments" },
              new[] { "eus.UI.Controllers" }
            );

            context.MapRoute(
               "Topic",
               "Vote/{topicTitleURL}/{id}",
               new { controller = "Topic", action = "TopicAnswers" },
               new[] { "eus.UI.Areas.Vote.Controllers"}
             );

            
                                               
            context.MapRoute(
                "Vote_default",
                "Vote/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "eus.UI.Areas.Vote.Controllers"}
            );                       
        }
    }
}