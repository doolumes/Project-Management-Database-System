using System;
using System.Collections.Generic;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group6Application.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginView
    {
        public List<Account> Acount = new List<Account>();
    }
}
