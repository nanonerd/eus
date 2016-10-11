using Domain.Vote;
using eus.UI.Common;
using eus.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace eus.UI.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public string GetComments(int id)
        {
            using (eusVoteEntities entVote = new eusVoteEntities())
            {
                var comments = (from c in entVote.Comments
                                where c.AnswerID == id
                                select c).Take(5).ToArray();

                var jsonReturn = JsonConvert.SerializeObject(comments, Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

                return jsonReturn;
            }
        }

        [HttpGet]
        public ActionResult IndexVote(int id)
        {            
            var commentsVM = new CommentViewModel();
            List<CommentModel> comments;                        

            using (eusVoteEntities entVote = new eusVoteEntities())
            {
                var answer = (from o in entVote.Answers
                              where o.AnswerID == id
                              select new { o.TopicID, o.Answer1, o.AnswerTitleURL }).FirstOrDefault();

                //ViewBag.AnswerTitle = answer.Title;
                commentsVM.AnswerTitle = answer.Answer1;
                commentsVM.AnswerTitleURL = answer.AnswerTitleURL;
                commentsVM.AnswerID = id;

                var topic = (from t in entVote.Topics
                             where t.TopicID == answer.TopicID
                             select new { t.Topic1, t.TopicTitleURL }).FirstOrDefault();

                commentsVM.TopicTitle = topic.Topic1;
                commentsVM.TopicTitleURL = topic.TopicTitleURL;
                commentsVM.TopicID = answer.TopicID;

                // Get base URL (e.g., http://www.eusVille.com)
                commentsVM.BaseURL = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";                               
            }

            using (eusCommonEntities entCommon = new eusCommonEntities())
            {
                using (eusVoteEntities entVote = new eusVoteEntities())
                {
                    var listUsers = new List<AspNetUser>(entCommon.AspNetUsers);
                    var listComments = new List<Comment>(entVote.Comments);
                    

                    // Do a join to listUsers to get the UserName
                    comments = (from c in listComments
                                join u in listUsers on c.UserID equals u.UserID
                                where (c.AnswerID == id)
                                select new CommentModel
                                {
                                    Author = u.UserName,
                                    CommentText = c.Comment1,
                                    UserID = u.UserID,
                                    ReplyTo = c.ReplyTo,
                                    TimeStamp = (DateTime)c.TimeStamp
                                }).ToList();
                }
            }
                    
            // If user is logged in, get UserID. 
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                ViewBag.UserID = Identity.GetUserID(userName);
            }
            else
            {
                ViewBag.UserID = 0;
            }

            //comments = ConvertTimestamp(comments);
            commentsVM.CommentModelList = ConvertTimestamp(comments);                       

            return View(commentsVM);
        }

        private List<CommentModel> ConvertTimestamp(List<CommentModel> cm)
        {
            List<CommentModel> list = new List<CommentModel>();

            foreach (CommentModel cmod in cm)
            {
                cmod.LongDate = cmod.TimeStamp.ToLongDateString();
                cmod.ShortTime = cmod.TimeStamp.ToShortTimeString();

                list.Add(cmod);
            }
            return list;
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public void PostComments(string Filter, string FilterData)
        {
            switch (Filter)
            {
                case "CommentPost":
                    {
                        // Parse out JSON data
                        JObject o = JObject.Parse(FilterData);

                        int userID = (int)o.SelectToken("UserID");
                        int answerID = (int)o.SelectToken("AnswerID");
                        string comment = (string)o.SelectToken("Comment");
                        DateTime stamp = DateTime.Now;

                        using (eusVoteEntities entVote = new eusVoteEntities())
                        {
                            Comment com = new Comment();
                            com.AnswerID = answerID;
                            com.UserID = userID;
                            com.Comment1 = comment;
                            com.TimeStamp = stamp;

                            entVote.Comments.Add(com);
                            entVote.SaveChanges();
                        }
                        break;
                    }
                default: 
                    break;
            }
        }



        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
