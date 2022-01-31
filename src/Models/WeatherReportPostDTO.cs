using System;

namespace iot_visby_weather.Models
{
    public class WeatherReportPostDTO
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public int Uv { get; set; }
        public int Raining { get; set; }
        public int Light { get; set; }
    }
}
