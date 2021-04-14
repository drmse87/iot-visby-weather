using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class LatestWeatherReportAndRecordsGetDTO
    {
        public WeatherReportGetDTO Report { get; set; }
        public WeatherRecords Records { get; set; }
    }
}