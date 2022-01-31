using System;
using System.ComponentModel.DataAnnotations;

namespace simple_iot_weather_station.Models
{
    public class LatestWeatherReportAndRecordsGetDTO
    {
        public WeatherReportGetDTO Report { get; set; }
        public WeatherRecords Records { get; set; }
    }
}