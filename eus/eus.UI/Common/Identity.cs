using eus.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;

namespace eus.UI.Common
{
    public static class Identity 
    {
        //public static string GetUserName()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var userName = User.Identity.Name;
        //        var userID = Identity.GetUserID(userName);
        //    }
        //}

              

        public static int GetUserID(string userName)
        {
            int userID = 0;

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
                throw new Exception("" + ex);
            }

            return userID;
        }
    }
}