using Domain;
using eus.UI.Common;
using eus.UI.Controllers;
using eus.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using eus.UI.Areas.Vote.Models;

namespace eus.UI.Areas.Vote.Controllers
{
    [RouteArea("Vote")]
    [RoutePrefix("Topic")]
    [Route("{action}/{id}")]
    public class TopicController : Controller
    {
        private eusCommonEntities entCommon = new eusCommonEntities();
        private eusVoteEntities entVote = new eusVoteEntities();

        // GET: Vote/Topics
        //public ActionResult Index()
        //{
        //    var topics = entVote.Topics;
        //    return View(topics.ToList());
        //}

        public ActionResult TopicAnswers(int id)
        {
            // Get topic title
            var topicTitle = (from t in entVote.Topics
                              where t.TopicID == id
                              select t.Topic1).FirstOrDefault();

            ViewBag.TopicTitle = topicTitle;
            ViewBag.TopicId = id;

            // Get base URL (e.g., http://www.eusVille.com/)
            ViewBag.BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

            // If user is logged in, get UserId and set it to ViewBag.
            var userId = 0;

            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;                
                userId = (int)Identity.GetUserID(userName);
            }

            ViewBag.UserId = userId.ToString();

            var answers = (from a in entVote.Answers
                           where a.TopicID == id
                           select a).ToList();

            //var answers = TopicGet("AnswersGet", id.ToString());
            //var answers = entVote.GetTopicAnswers(id).ToList();
            
            //var x = entVote.
            return View(answers);
        }
               

        // GET: Vote/Topics/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = entVote.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Vote/Topics/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(entVote.CategoryLists, "CategoryID", "Category");
            return View();
        }

        // POST: Vote/Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TopicID,Topic1,Description,TopicURL,Search,Tags,CategoryID,UserID,TimeStamp")] Topic topic)
        public ActionResult Create([Bind(Include = "Topic,Description,Tags,CategoryID")] TopicCreate tc)
        {
            if (ModelState.IsValid)
            {
                Topic topicAdd = new Models.Topic();
                topicAdd.CategoryID = tc.CategoryID;
                topicAdd.Description = tc.Description;
                topicAdd.Tags = tc.Tags;
                topicAdd.TimeStamp = DateTime.Now;
                topicAdd.Topic1 = tc.Topic;
                //topicAdd.TopicID = topic.TopicID;
                                               

                // If user is logged in, get UserID. To get to this point, user must be logged in.
                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.Identity.Name;             
                    topicAdd.UserID = Identity.GetUserID(userName);
                }
                else
                {
                    topicAdd.UserID = 0;
                }
                                
                topicAdd.TopicTitleURL = SearchTools.CreateSearchURL(tc.Topic);               
                
                entVote.Topics.Add(topicAdd);
                entVote.SaveChanges();
                return RedirectToAction("Index");   // todo: redirect to home page
            }
            else
            {
                // log out invalid state
            }

            ViewBag.CategoryID = new SelectList(entVote.CategoryLists, "CategoryID", "Category");
            return View();
        }

        // GET: Vote/Topics/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = entVote.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Vote/Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,Topic1,Description,TopicURL,Search,Tags,CategoryID,UserID,TimeStamp")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                entVote.Entry(topic).State = EntityState.Modified;
                entVote.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Vote/Topics/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = entVote.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Vote/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Topic topic = entVote.Topics.Find(id);
            entVote.Topics.Remove(topic);
            entVote.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entVote.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> ////////////////////////////////////////////////////////////////////////
        /// GETS    GETS    GETS    GETS    GETS    GETS    GETS    GETS     
        /// </summary> //////////////////////////////////////////////////////////////////////// 
        [HttpGet]
        public string TopicGet(string Filter, string FilterData)
        {
            string jsonReturn = "";

            try
            {
                switch (Filter)
                {
                    case "AnswersGet":
                        {
                            int topicID = Convert.ToInt32(FilterData);

                            // Get topic title for id.
                            var topicTitle = (from t in entVote.Topics
                                              where t.TopicID == topicID
                                              select t.Topic1).FirstOrDefault();

                            ViewBag.TopicTitle = topicTitle;
                            ViewBag.TopicID = topicID;

                            // Get top 8 answers for topic based on TopicID.
                            //var answers = (from o in entVote.Answers
                            //               where o.TopicID == topicID
                            //               orderby o.RatingScore descending
                            //               select o).Take(8);

                            // If user is logged in, get UserID and set it to ViewBag to pass to the View.
                            var userID = 0;

                            if (User.Identity.IsAuthenticated)
                            {
                                var userName = User.Identity.Name;
                                //Identity ident = new Identity();
                                userID = (int)Identity.GetUserID(userName);
                                ViewBag.UserID = userID.ToString();
                            }
                            else
                            {
                                ViewBag.UserID = "0";
                            }

                            var answers = (from a in entVote.Answers
                                           where a.TopicID == topicID
                                           select a).ToList();
                                
                                //entVote.GetTopicAnswers(topicID).ToList();

                            ViewBag.Count = answers.Count();                                                      

                            jsonReturn = JsonConvert.SerializeObject(answers, Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                });
                            break;
                        }
                    //case "CommentsGet":
                    //    {
                    //        int answerID = Convert.ToInt32(FilterData);

                    //        using (eusVoteEntities entVote = new eusVoteEntities())
                    //        {
                    //            var comments = (from c in entVote.Comments
                    //                            where c.AnswerID == answerID
                    //                            select c).Take(5).ToArray();

                    //            jsonReturn = JsonConvert.SerializeObject(comments, Formatting.Indented,
                    //                            new JsonSerializerSettings
                    //                            {
                    //                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //                            });
                    //        }
                    //        break;
                    //    }
                    default:
                        {
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                var seriesContent = new Dictionary<string, string>
                {
                    {"user" , "test"}, {"detail", ex.ToString()}, {"action", Filter},
                    {"errorLocation", "eusVote > TopicController > Topic > Get > " + Filter} , {"level", "high"}
                };

                jsonReturn = JsonConvert.SerializeObject(seriesContent);

                // Log error
                CommonController cont = new CommonController();
                cont.Common("ErrorHandle", jsonReturn);

                return jsonReturn;
            }

            return jsonReturn;
        }


        /// <summary> ////////////////////////////////////////////////////////////////////////
        /// POSTS   POSTS   POSTS   POSTS   POSTS   POSTS   POSTS   POSTS   
        /// </summary> ////////////////////////////////////////////////////////////////////////       

        [HttpPost]
        public string Topic(string Filter, string FilterData)
        {
            string jsonReturn = "";
            string userName = "";

            // If user is logged in, get UserID. To get to this point, user must be logged in.
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;                
            }
            else
            {
                throw new Exception("Must be logged in to post.");
            }

            //FilterData = null;   // to test errorHandle
            try
            {
                switch (Filter)
                {
                    case "SetRating":
                        {
                            // Parse out JSON data
                            JObject o = JObject.Parse(FilterData);

                            int userID = (int)o.SelectToken("UserID");                            
                            int answerID = (int)o.SelectToken("AnswerID");
                            int rating = (int)o.SelectToken("Rating");
                            string comment = (string)o.SelectToken("Comment");
                            
                            using (eusVoteEntities entVote = new eusVoteEntities())
                            {
                                // Save comment only if not empty.                               
                                if (!string.IsNullOrWhiteSpace(comment))
                                {
                                    Comment com = new Comment();
                                    com.AnswerID = answerID;
                                    com.UserID = userID;
                                    com.Comment1 = comment;
                                    com.TimeStamp = DateTime.Now;

                                    entVote.Comments.Add(com);
                                    entVote.SaveChanges();

                                    // update CommentCount in Comments table
                                    var commentTotal = (from ct in entVote.Comments
                                                        where ct.AnswerID == answerID
                                                        select ct).Count();

                                    var ans = (from an in entVote.Answers
                                               where an.AnswerID == answerID
                                               select an).FirstOrDefault();

                                    ans.CommentCount = commentTotal;
                                    entVote.SaveChanges();
                                }

                                // Logic to set rating
                                var record = (from e in entVote.Ratings
                                              where e.UserID == userID
                                                 && e.AnswerID == answerID
                                              select e).FirstOrDefault();

                                // Set the rating for an answer per this user
                                if (record != null)  // If exist, then Update rating
                                {
                                    record.Rating1 = rating;
                                    entVote.SaveChanges();
                                }
                                else   // If not exist, then Insert new rating per this user
                                {
                                    Rating r = new Rating();
                                    r.UserID = userID;
                                    r.AnswerID = answerID;
                                    r.Rating1 = rating;

                                    entVote.Ratings.Add(r);
                                    entVote.SaveChanges();
                                }

                                //////////////////////////////////////////////////////////////////////////
                                //////// Update the RatingScore for the Answer/Answer in the Rating table
                                //////////////////////////////////////////////////////////////////////////

                                // Get all the rating values for the answer (will be summed up in RatingScore calc below)
                                var totalRatings = (from t in entVote.Ratings
                                                    where t.AnswerID == answerID
                                                    select t.Rating1);

                                // Get the answer info for the answer that is being updated
                                var answer = (from op in entVote.Answers
                                              where op.AnswerID == answerID
                                              select op).FirstOrDefault();

                                // If record is null, then Insert was done so increment count.
                                if (record == null)
                                {
                                    if (answer.RatingCount == null)
                                    {
                                        answer.RatingCount = 1;
                                    }
                                    else
                                    {
                                        answer.RatingCount++;
                                    }
                                }                               

                                // Calcuate new avg rating based on latest numbers
                                answer.RatingScore = Math.Round(((decimal)totalRatings.Sum() / (decimal)totalRatings.Count()), 2);
                                entVote.SaveChanges();

                                // Create dictionary of values to return
                                var seriesContent = new Dictionary<string, string>
                                {
                                    {"error", "false"} , {"count", answer.RatingCount.ToString()}, {"score", answer.RatingScore.ToString()}
                                };

                                // Serialize the dictionary to return as json
                                jsonReturn = JsonConvert.SerializeObject(seriesContent);
                                break;
                            }
                        }

                    //case "CommentPost":
                    //    {
                    //        // Parse out JSON data
                    //        JObject o = JObject.Parse(FilterData);

                    //        int userID = (int)o.SelectToken("UserID");
                    //        int answerID = (int)o.SelectToken("AnswerID");
                    //        string comment = (string)o.SelectToken("Comment");
                    //        DateTime stamp = DateTime.Now;

                    //        using (eusVoteEntities entVote = new eusVoteEntities())
                    //        {
                    //            Comment com = new Comment();
                    //            com.AnswerID = answerID;
                    //            com.UserID = userID;
                    //            com.Comment1 = comment;
                    //            com.TimeStamp = stamp;

                    //            entVote.Comments.Add(com);
                    //            entVote.SaveChanges();
                    //        }
                    //        break;
                    //    }

                    default:
                        {
                            var seriesContent = new Dictionary<string, string>
                                    {
                                        {"error", "true"} , {"message", "Controller: eusVote > TopicController > Topic [POST] doesn't have matching filter for filter = " + Filter }
                                    };

                            jsonReturn = JsonConvert.SerializeObject(seriesContent);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                // Create error data to serialize to JSON.
                var seriesContent = new Dictionary<string, string>
                {
                    {"user" , userName}, {"detail", ex.Message}, {"action", Filter},
                    {"errorLocation", "eusVote > TopicController > Topic [POST] " + Filter} , {"level", "high"}
                };

                // Serialize error data.
                jsonReturn = JsonConvert.SerializeObject(seriesContent);

                // Log error
                CommonController cont = new CommonController();
                cont.Common("ErrorHandle", jsonReturn);

                return jsonReturn;
            }

            return jsonReturn;
        }           
    }
}
