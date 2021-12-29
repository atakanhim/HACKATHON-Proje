using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace artiktamam.Models
{
    public class Register:Login
    {
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("PassWord")]
        public string PassWordAgain { get; set; }

        [Required(ErrorMessage = "Email Bos Birakılamaz")]
        public string Email { get; set; }
    }
}