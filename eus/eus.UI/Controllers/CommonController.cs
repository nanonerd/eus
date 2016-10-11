using eus.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eus.UI.Controllers
{
    public class CommonController : Controller
    {        
        public Int64 GetUserID()
        {
            Int64 userID = 0;
            string userName;

            if (User != null && User.Identity.IsAuthenticated)
            {
                var common = new CommonController();
                userName = common.GetUserName();

                try
                {
                    using (eusCommonEntities ent = new eusCommonEntities())
                    {
                        // If userID = null, it will simply return 0;
                        userID = (from u in ent.AspNetUsers
                                  where u.UserName == userName
                                  select u.UserID).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userID;
        }

        public string GetUserName()
        {
            var userName = "NotLoggedIn";

            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            return userName;
        }
                

        // POST
        public void Common(string Filter, string FilterData)
        {
            string jsonReturn = "";

            try
            {
                switch (Filter)
                {
                    case "ErrorHandle":  // log error into eusCommon.dbo.Admin_ErrorLog
                        {
                            // Parse out JSON data
                            JObject o = JObject.Parse(FilterData);

                            string user = o.SelectToken("user").ToString();
                            string detail = o.SelectToken("detail").ToString();
                            string action = o.SelectToken("action").ToString();
                            string errorLocation = o.SelectToken("errorLocation").ToString();
                            string level = o.SelectToken("level").ToString();

                            if (level == "high")
                            {
                                // email admin to notify !!
                            }


                            using (eusCommonEntities ent = new eusCommonEntities())
                            {
                                Admin_ErrorLog log = new Admin_ErrorLog();

                                log.User = user;
                                log.Action = action;
                                log.Details = detail;
                                log.ErrorLocation = errorLocation;
                                log.Level = level;
                                log.Timestamp = DateTime.Now;
                                log.IsOpen = true;

                                ent.Admin_ErrorLog.Add(log);
                                ent.SaveChanges();

                                break;
                            }
                        }

                    case "Email":
                        {
                            // Send email !
                            break;
                        }

                    default:
                        {
                            //var seriesContent = new Dictionary<string, string>
                            //        {
                            //            {"error", "true"} , {"message", "Controller: eusVote_Topic.Topic doesn't have matching filter for filter = " + Filter }
                            //        };

                            //jsonReturn = JsonConvert.SerializeObject(seriesContent);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                var seriesContent = new Dictionary<string, string>
                {
                    {"user", "inside catch"}, {"detail", ex.ToString()}, {"action", "Failed to log error."},
                    {"errorLocation", "CommonController > ErrorHandle "}
                };

                jsonReturn = JsonConvert.SerializeObject(seriesContent);

                // Log error
                CommonController cont = new CommonController();
                cont.Common("ErrorHandle", jsonReturn);
            }

            //return jsonReturn;    
        }
    }
}
