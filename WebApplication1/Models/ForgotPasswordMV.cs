using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ForgotPasswordMV
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}