using System;
using System.ComponentModel.DataAnnotations;

namespace iot_visby_weather.Models
{
    public class LatestWeatherReportAndRecordsGetDTO
    {
        public WeatherReportGetDTO Report { get; set; }
        public WeatherRecords Records { get; set; }
    }
}