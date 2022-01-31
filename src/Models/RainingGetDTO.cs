using System;
using System.ComponentModel.DataAnnotations;

namespace simple_iot_weather_station.Models
{
    public class RainingGetDTO
    {
        public bool Raining { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
