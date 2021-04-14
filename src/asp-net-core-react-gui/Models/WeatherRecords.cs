using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class WeatherRecords
    {
        public float TemperatureMax { get; set; }
        public DateTime TemperatureMaxDate { get; set; }
        public float TemperatureMin { get; set; }
        public DateTime TemperatureMinDate { get; set; }
        public float HumidityMax { get; set; }
        public DateTime HumidityMaxDate { get; set; }
        public float HumidityMin { get; set; }
        public DateTime HumidityMinDate { get; set; }
        public int UvMax { get; set; }
        public DateTime UvMaxDate { get; set; }
        public int UvMin { get; set; }
        public DateTime UvMinDate { get; set; }
        public string FirstLightToday { get; set; }
        public string LastLightToday { get; set; }
        public string LastTimeRaining { get; set; }
    }
}
