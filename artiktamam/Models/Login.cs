using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace artiktamam.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Username Bos Birakılamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Bos Birakılamaz")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
    }
}