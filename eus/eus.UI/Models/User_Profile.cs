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
    
    public partial class User_Profile
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostalCode { get; set; }
        public string Email2 { get; set; }
        public string Phone2 { get; set; }
        public Nullable<bool> Phone2Confirmed { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public Nullable<int> BirthYear { get; set; }
        public string ImageURL { get; set; }
        public string ThumbURL { get; set; }
        public string Ethnicity1 { get; set; }
        public string Ethnicity2 { get; set; }
        public string Language1 { get; set; }
        public string Language2 { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public Nullable<bool> IsApproved { get; set; }
    }
}
