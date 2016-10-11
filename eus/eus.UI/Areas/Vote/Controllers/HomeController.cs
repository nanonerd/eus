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
    public class HomeController : Controller
    {
        private eusVoteEntities entVote = new eusVoteEntities();

        private int getUserID()
        {
            var userID = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                userID = Identity.GetUserID(userName);
            }

            return userID;
        }

        // GET: Vote/Home
        public ActionResult Index()
        {           
            ViewBag.UserID = getUserID();
            return View(entVote.Topics.ToList());
        }

        // GET: Vote/Home/Details/5
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

        // GET: Vote/Home/Create
        public ActionResult Create()
        {
            // Generate and pass in category list and ids for Dropdown list
            ViewBag.CategoryID = new SelectList(entVote.CategoryLists, "CategoryID", "Category");  

            return View();
        }

        // POST: Vote/Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TopicID,Topic1,Description,TopicURL,Search,Tags,CategoryID,UserID,TimeStamp")] Topic topic)
        public ActionResult Create([Bind(Include = "Topic,Description,Tags,CategoryID,TimeStamp")] TopicCreate tc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var topicAdd = new Topic();                    
                    var timeStamp = DateTime.Now;

                    topicAdd.Topic1 = tc.Topic;
                    topicAdd.Description = tc.Description;
                    topicAdd.Tags = tc.Tags;
                    topicAdd.CategoryID = tc.CategoryID;
                    topicAdd.TopicTitleURL = SearchTools.CreateSearchURL(tc.Topic);
                    topicAdd.TimeStamp = timeStamp;
                    topicAdd.UserID = getUserID();                    

                    entVote.Topics.Add(topicAdd);
                    entVote.SaveChanges();

                    var topicJustInsertedID = topicAdd.TopicID;

                    // get topicID inserted
                    // go to Answer screen and ask user to add answer


                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }                       

            return View(tc);
            
        }

        // GET: Vote/Home/Edit/5
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

        // POST: Vote/Home/Edit/5
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

        // GET: Vote/Home/Delete/5
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

        // POST: Vote/Home/Delete/5
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
    }
}
