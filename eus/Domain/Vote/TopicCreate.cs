using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain
{
    public class TopicCreate
    {
        //public long TopicID { get; set; }
        
        [StringLength(200)]
        [Required(ErrorMessage = "Enter a topic")]        
        public string Topic { get; set; }
        
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description (optional)")]        
        public string Description { get; set; }        
        
        [StringLength(200)]
        [Required(ErrorMessage = "Type some tags")]        
        public string Tags { get; set; }

        //[StringLength(200)]
        //[DisplayName("Answer Title")]
        //public string AnswerTitle { get; set; }
        //[StringLength(8000)]
        //[DisplayName("Answer Detail")]
        //[DataType(DataType.MultilineText)]
        //public string AnswerDetail { get; set; }
        //[DisplayName("Rate (1-100)")]
        //[Range(1,100)]
        //public int? Rating { get; set; }

        [DisplayName("Category")]
        public int CategoryID { get; set; }        

        //[Required]
        //public DateTime TimeStamp { get; set; }
        //public int UserID { get; set; }

        //public virtual ICollection<Answer> Answers { get; set; }
        //public virtual CategoryList CategoryList { get; set; }
    }
}
