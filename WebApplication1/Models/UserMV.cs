using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserMV
    {
        public UserMV() 
        {
            Company = new CompanyMV();
        }
        public int UserID { get; set; }
        public int UserTypeID { get; set; }

        [Required(ErrorMessage ="Username Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Email Address required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage ="Contact no is required")]
        public string ContactNo { get; set; }
        public bool AreYouProvider {  get; set; }

        public CompanyMV Company { get; set; }
    }
}