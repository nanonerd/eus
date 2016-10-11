using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vote
{
    public class CommentViewModel
    {
        public List<CommentModel> CommentModelList { get; set; }

        public long? TopicID { get; set; }
        public string TopicTitle { get; set; }
        public string TopicTitleURL { get; set; }
        public int AnswerID { get; set; }        
        public string AnswerTitle { get; set; }
        public string AnswerTitleURL { get; set; }        
        public string BaseURL { get; set; }
        
        
        //public DateTime TimeStamp { get; set; }
        //public string returnURL { get; set; }
    }

    public class CommentModel
    {
        public string Author { get; set; }
        public string CommentText { get; set; }
        public int UserID { get; set; }
        public int? ReplyTo { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LongDate { get; set; }
        public string ShortTime { get; set; }
    }
}
