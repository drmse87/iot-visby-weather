using System;
using System.ComponentModel.DataAnnotations;

namespace simple_iot_weather_station.Models
{
    public class WeatherReportGetDTO
    {
        public string ReportId { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public int Uv { get; set; }
        public bool Raining { get; set; }
        public bool Light { get; set; }
        public DateTime ReportDate { get; set; }

        public static Models.WeatherReportGetDTO FromModel (Models.WeatherReport reportToConvert)
        {
            return new Models.WeatherReportGetDTO {
                    ReportId = reportToConvert.ReportId,
                    Temperature =  reportToConvert.Temperature,
                    Humidity = reportToConvert.Humidity,
                    Uv = reportToConvert.Uv,
                    Raining = reportToConvert.Raining == 1 ? true : false,
                    Light = reportToConvert.Light == 1 ? true : false,
                    ReportDate = reportToConvert.ReportDate
            };
        }
    }
}
