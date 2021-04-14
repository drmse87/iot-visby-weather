using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class LightGetDTO
    {
        public bool Light { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
