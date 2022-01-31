using System;
using System.ComponentModel.DataAnnotations;

namespace iot_visby_weather.Models
{
    public class RainingGetDTO
    {
        public bool Raining { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
