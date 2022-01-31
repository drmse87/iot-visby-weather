using System;
using System.ComponentModel.DataAnnotations;

namespace iot_visby_weather.Models
{
    public class LightGetDTO
    {
        public bool Light { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
