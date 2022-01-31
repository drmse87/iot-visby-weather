using System;
using System.ComponentModel.DataAnnotations;

namespace simple_iot_weather_station.Models
{
    public class LightGetDTO
    {
        public bool Light { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
