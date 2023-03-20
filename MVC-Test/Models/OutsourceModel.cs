using System;
using System.Collections.Generic;
using Group6Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Group6Application.Model
{
    public class OutsourceTemplate
    {
        public string? Name { get; set; }
        public int? Outsource_ID { get; set; }
        public string? Email { get; set; }
        public long? Phone_number { get; set; }
        public string? Job { get; set; }
        public int? Task_ID { get; set; }
    }

    public class OutsourceView
    {
        public List<OutsourceTemplate> Outsource = new List<OutsourceTemplate>();
    }
}
