using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    public class loginBindingModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string UserId { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string Password { get; set; }
    }
}