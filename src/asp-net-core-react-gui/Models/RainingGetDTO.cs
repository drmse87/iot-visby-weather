using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class RainingGetDTO
    {
        public bool Raining { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
