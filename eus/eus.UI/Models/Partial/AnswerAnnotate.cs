using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Namespace needs to be the same as the partial class to be annotated.
namespace eus.UI.Models
{
    // http://stackoverflow.com/questions/16736494/add-data-annotations-to-a-class-generated-by-entity-framework
    // This is created in order to Annotate classes that EF auto generates. Cannot modify the EF classes so need to do this.
    // Below is example of way to annotate the Answer partial class that EF auto generated.

    [MetadataType(typeof(AnswerAnnotate))]
    public partial class Answer
    {

    }

    public class AnswerAnnotate
    {        
        public long AnswerID { get; set; }
        public Nullable<long> TopicID { get; set; }

        [DisplayName("Answer")]
        [Display(Prompt="short title")]
        [Required(ErrorMessage = "Enter answer title")]
        public string Answer1 { get; set; }
        
        [Required(ErrorMessage = "Enter answer details")]
        public string Detail { get; set; }

        [DisplayName("Rating (0 - 100)")]
        [Display(Prompt=("0 - 100"))]
        [Required(ErrorMessage = "Enter 0-100 rating")]
        [Range(0, 100, ErrorMessage="Enter 0-100 rating")]
        public Nullable<decimal> RatingScore { get; set; }

        public Nullable<long> RatingCount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
    }
}