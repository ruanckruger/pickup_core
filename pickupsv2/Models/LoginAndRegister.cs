using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using pickupsv2.Areas.Identity.Pages.Account;

namespace pickupsv2.Models
{
    public class LoginAndRegister
    { 
        public LoginModel Login { get; set; }
        public RegisterModel Register { get; set; }
    }
        
}