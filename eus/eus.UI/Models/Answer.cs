//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eus.UI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        public int AnswerID { get; set; }
        public Nullable<int> TopicID { get; set; }
        public string Answer1 { get; set; }
        public string AnswerTitleURL { get; set; }
        public string Detail { get; set; }
        public Nullable<decimal> RatingScore { get; set; }
        public Nullable<int> RatingCount { get; set; }
        public Nullable<int> CommentCount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
    }
}
