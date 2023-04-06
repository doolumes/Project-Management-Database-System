using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group6Application.Model;

namespace Group6Application.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
