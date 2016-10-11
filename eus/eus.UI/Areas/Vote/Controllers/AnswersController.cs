using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eus.UI.Models;
using eus.UI.Common;
using Domain;

namespace eus.UI.Areas.Vote.Controllers
{
    public class AnswersController : Controller
    {
        private eusVoteEntities entVote = new eusVoteEntities();
      

        // GET: Vote/Answers
        public ActionResult Index()
        {
            if (true)
            {
                // 
            }
            return View(entVote.Answers.ToList());
        }

        // GET: Vote/Answers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = entVote.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Vote/Answers/Create
        public ActionResult Create(int id)
        {
            var answer = new Answer();
            var userId = 0;            

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.Identity.Name;
                    userId = (int)Identity.GetUserID(userName);
                }

                // Get topic record
                var topic = (from t in entVote.Topics
                             where t.TopicID == id
                             select t).FirstOrDefault();

                ViewBag.TopicTitle = topic.Topic1;
                ViewBag.TopicId = topic.TopicID;
                ViewBag.TopicURL = topic.TopicTitleURL;                              
                
                answer.TopicID = id;
                answer.UserID = userId;
                answer.RatingCount = 1;
                answer.TimeStamp = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return View(answer);
        }

        // POST: Vote/Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Create([Bind(Include = "AnswerID,TopicID,Answer1,Detail,RatingScore,RatingCount,UserID,TimeStamp")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert new answer into Answer table
                    answer.AnswerTitleURL = SearchTools.CreateSearchURL(answer.Answer1);

                    entVote.Answers.Add(answer);
                    entVote.SaveChanges();

                    var justCreatedAnswerID = (from a in entVote.Answers
                                               where a.AnswerTitleURL == answer.AnswerTitleURL
                                               select a.AnswerID).FirstOrDefault();

                    // Insert new rating into Rating table
                    var rating = new Rating();
                    rating.UserID = answer.UserID;
                    rating.AnswerID = justCreatedAnswerID;
                    rating.Rating1 = (int)answer.RatingScore;

                    // Update AnswerCount in Topic Table 
                    var topic = (from t in entVote.Topics
                                 where t.TopicID == answer.TopicID
                                 select t).FirstOrDefault();

                    var answerCount = (from ac in entVote.Answers
                                       where ac.TopicID == answer.TopicID
                                       select ac).Count();
                                        
                    topic.AnswerCount = answerCount;

                    entVote.Ratings.Add(rating);
                    //entVote.Topics.Add(topic);
                    

                    entVote.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Vote/Answers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = entVote.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Vote/Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerID,TopicID,Answer1,Detail,RatingScore,RatingCount,UserID,TimeStamp")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                entVote.Entry(answer).State = EntityState.Modified;
                entVote.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer);
        }

        // GET: Vote/Answers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = entVote.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Vote/Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Answer answer = entVote.Answers.Find(id);
            entVote.Answers.Remove(answer);
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
    }
}
