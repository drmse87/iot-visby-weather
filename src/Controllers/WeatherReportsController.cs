using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;

namespace iot_visby_weather.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherReportsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _db;
        private readonly ILogger<WeatherReportsController> _logger;
        private static readonly string _errorMessageSavingData = "An error occurred saving data to database.";
        private static readonly string _errorMessageFetchingData = "An error occurred fetching data from database.";
        private static readonly string _apiKeyHeaderName = "X-Api-Key";
        private static readonly string _thingDescriptionFilename = @"./ThingDescription.json";
        private IConfiguration _conf { get; }
        public WeatherReportsController(ILogger<WeatherReportsController> logger, 
            Data.ApplicationDbContext db, 
            IConfiguration conf)
        {
            _conf = conf;
            _db = db;
            _logger = logger;
        }

        [Route("~/api/v1")]
        [HttpGet]
        public JsonElement GetThingDescription()
        {
            string thingDescriptionLocation = System.IO.File.ReadAllText(_thingDescriptionFilename);

            JsonElement thingDescription = JsonSerializer.Deserialize<JsonElement>(thingDescriptionLocation);

            return thingDescription;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWeatherReports()
        {
            try 
            {
                IEnumerable<Models.WeatherReport> weatherReports = await _db.WeatherReports
                    .OrderBy(w => w.ReportDate)
                    .ToListAsync();

                return Ok(weatherReports);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/latest")]
        [HttpGet]
        public async Task<IActionResult> GetLatestReportAndRecords()
        {
            try 
            {
                IEnumerable<Models.WeatherReport> weatherReports = await _db.WeatherReports.ToListAsync();

                Models.WeatherReport latestWeatherReport = weatherReports
                    .OrderByDescending(wr => wr.ReportDate)
                    .FirstOrDefault();

                if (latestWeatherReport == null)
                {
                    return NotFound();
                }

                return Ok(new Models.LatestWeatherReportAndRecordsGetDTO {
                    Report = Models.WeatherReportGetDTO.FromModel(latestWeatherReport),
                    Records = new Models.WeatherRecords {
                        FirstLightToday = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date && w.Light == 1)
                            .OrderBy(w => w.ReportDate)
                            .FirstOrDefault()?.ReportDate.ToString("HH:mm") ?? "",
                        LastLightToday = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date && w.Light == 1)
                            .OrderBy(w => w.ReportDate)
                            .LastOrDefault()?.ReportDate.ToString("HH:mm") ?? "", 
                        LastTimeRaining = weatherReports
                            .Where(w => w.Raining == 1)
                            .OrderBy(w => w.ReportDate)
                            .LastOrDefault()?.ReportDate.ToString() ?? "", 
                        TemperatureMax = weatherReports
                            .OrderByDescending(w => w.Temperature)
                            .FirstOrDefault().Temperature,
                        TemperatureMaxDate = weatherReports
                            .OrderByDescending(w => w.Temperature)
                            .FirstOrDefault().ReportDate,
                        TemperatureMin = weatherReports
                            .OrderBy(w => w.Temperature)
                            .FirstOrDefault().Temperature,
                        TemperatureMinDate = weatherReports
                            .OrderBy(w => w.Temperature)
                            .FirstOrDefault().ReportDate,
                        HumidityMax = weatherReports
                            .OrderByDescending(w => w.Humidity)
                            .FirstOrDefault().Humidity,
                        HumidityMaxDate = weatherReports
                            .OrderByDescending(w => w.Humidity)
                            .FirstOrDefault().ReportDate,
                        HumidityMin = weatherReports
                            .OrderBy(w => w.Humidity)
                            .FirstOrDefault().Humidity,
                        HumidityMinDate = weatherReports
                            .OrderBy(w => w.Humidity)
                            .FirstOrDefault().ReportDate,
                        UvMax = weatherReports
                            .OrderByDescending(w => w.Uv)
                            .FirstOrDefault().Uv,
                        UvMaxDate = weatherReports
                            .OrderByDescending(w => w.Uv)
                            .FirstOrDefault().ReportDate,
                        UvMin = weatherReports
                            .OrderBy(w => w.Uv)
                            .FirstOrDefault().Uv,
                        UvMinDate = weatherReports
                            .OrderBy(w => w.Uv)
                            .FirstOrDefault().ReportDate
                    }
                });
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/temperature")]
        [HttpGet]
        public async Task<IActionResult> GetTemperature(string display)
        {
            try 
            {
                IEnumerable<dynamic> weatherReports = await _db.WeatherReports
                    .ToListAsync();

                switch(display)
                {
                   case "hourly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date &&
                                w.ReportDate.Hour == DateTime.Now.Hour)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH:mm"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Temperature = g.FirstOrDefault()?.Temperature
                            });
                        break;
                    case "daily":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH"))
                            .Select(g => new {
                                ReportDate = g.Key + ":00",
                                Temperature = g.FirstOrDefault()?.Temperature
                            });
                        break;
                    case "monthly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year &&
                                w.ReportDate.Month == DateTime.Now.Month)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("yyyy-MM-dd"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Temperature = Math.Round(g.Average(w => Convert.ToInt32(w.Temperature)), 1)
                            });
                        break;
                    case "yearly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("MMMM"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Temperature = Math.Round(g.Average(w => Convert.ToInt32(w.Temperature)), 1)
                            });
                        break;
                }

                return Ok(weatherReports);
            }
            catch (InvalidOperationException e)
            {
                System.Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/humidity")]
        [HttpGet]
        public async Task<IActionResult> GetHumidity(string display)
        {
            try 
            {
                IEnumerable<dynamic> weatherReports = await _db.WeatherReports
                    .ToListAsync();

                switch(display)
                {
                   case "hourly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date &&
                                w.ReportDate.Hour == DateTime.Now.Hour)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH:mm"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Humidity = g.FirstOrDefault()?.Humidity
                            });
                        break;
                    case "daily":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH"))
                            .Select(g => new {
                                ReportDate = g.Key + ":00",
                                Humidity = g.FirstOrDefault()?.Humidity
                            });
                        break;
                    case "monthly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year &&
                                w.ReportDate.Month == DateTime.Now.Month)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("yyyy-MM-dd"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Humidity = Math.Round(g.Average(w => Convert.ToInt32(w.Humidity)), 1)
                            });
                        break;
                    case "yearly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("MMMM"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Humidity = Math.Round(g.Average(w => Convert.ToInt32(w.Humidity)), 1)
                            });
                        break;
                }

                return Ok(weatherReports);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/uv")]
        [HttpGet]
        public async Task<IActionResult> GetUv(string display)
        {
            try 
            {
                IEnumerable<dynamic> weatherReports = await _db.WeatherReports
                    .ToListAsync();

                switch(display)
                {
                   case "hourly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date &&
                                w.ReportDate.Hour == DateTime.Now.Hour)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH:mm"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Uv = g.FirstOrDefault()?.Uv
                            });
                        break;
                    case "daily":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Date == DateTime.Now.Date)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("HH"))
                            .Select(g => new {
                                ReportDate = g.Key + ":00",
                                Uv = g.FirstOrDefault()?.Uv
                            });
                        break;
                    case "monthly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year &&
                                w.ReportDate.Month == DateTime.Now.Month)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("yyyy-MM-dd"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Uv = Math.Round(g.Average(w => Convert.ToInt32(w.Uv)), 1)
                            });
                        break;
                    case "yearly":
                        weatherReports = weatherReports
                            .Where(w => w.ReportDate.Year == DateTime.Now.Year)
                            .OrderBy(w => w.ReportDate)
                            .GroupBy(w => w.ReportDate.ToString("MMMM"))
                            .Select(g => new {
                                ReportDate = g.Key,
                                Uv = Math.Round(g.Average(w => Convert.ToInt32(w.Uv)), 1)
                            });
                        break;
                }

                return Ok(weatherReports);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/raining")]
        [HttpGet]
        public async Task<IActionResult> GetRaining()
        {
            try 
            {
                IEnumerable<Models.RainingGetDTO> weatherReports = await _db.WeatherReports
                    .OrderByDescending(w => w.ReportDate)
                    .Where(w => w.Raining == 1)
                    .Select(w => new Models.RainingGetDTO {
                        Raining = true,
                        ReportDate = w.ReportDate
                    })
                    .ToListAsync();

                return Ok(weatherReports);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [Route("~/api/v1/[controller]/light")]
        [HttpGet]
        public async Task<IActionResult> GetLight()
        {
            try 
            {
                IEnumerable<Models.LightGetDTO> weatherReports = await _db.WeatherReports
                    .OrderByDescending(w => w.ReportDate)
                    .Where(w => w.Light == 1)
                    .Select(w => new Models.LightGetDTO {
                        Light = true,
                        ReportDate = w.ReportDate
                    })
                    .ToListAsync();

                return Ok(weatherReports);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageFetchingData);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeatherReport(Models.WeatherReportPostDTO incomingWeatherReport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            StringValues key;
            if (!Request.Headers.TryGetValue(_apiKeyHeaderName, out key) || 
                key.FirstOrDefault() != _conf["ApiKey"])
            {
                return Unauthorized();
            }

            // Some clarification needed here: when the sensor reports 0 (LOW), it actually means something is happening. This is why it is stored as 1 (true).
            try
            {
                Models.WeatherReport newReport = new Models.WeatherReport {
                    ReportId = Guid.NewGuid().ToString(),
                    Temperature = incomingWeatherReport.Temperature,
                    Humidity = incomingWeatherReport.Humidity,
                    Uv = GetUvIndex(incomingWeatherReport.Uv),
                    Raining = incomingWeatherReport.Raining == 0 ? 1 : 0,
                    Light = incomingWeatherReport.Light == 0 ? 1 : 0,
                    ReportDate = DateTime.Now
                };

                await _db.WeatherReports.AddAsync(newReport);
                await _db.SaveChangesAsync();

                string locationUri = $"{_conf["BaseUrl"]}/api/v1/weatherreports/{newReport.ReportId}";

                return Created(locationUri, Models.WeatherReportGetDTO.FromModel(newReport));
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    _errorMessageSavingData);
            }
        }

        // To save Arduino sketch size, UV index is calculated here.
        private static int GetUvIndex (int sensorValue) 
        {
            float analogReference = 1.08f;
            float milliVolt = (float)(sensorValue * (analogReference / 1024.0)) * 1000;
            int uvIndex = 0;

            if (milliVolt < 50)
            {
                uvIndex = 0;
            }
            else if (milliVolt > 50 && milliVolt <= 227)
            {
                uvIndex = 0;
            }
            else if (milliVolt > 227 && milliVolt <= 318)
            {
                uvIndex = 1;
            }
            else if (milliVolt > 318 && milliVolt <= 408)
            {
                uvIndex = 2;
            }
            else if (milliVolt > 408 && milliVolt <= 503)
            {
                uvIndex = 3;
            }
            else if (milliVolt > 503 && milliVolt <= 606)
            {
                uvIndex = 4;
            }
            else if (milliVolt > 606 && milliVolt <= 696)
            {
                uvIndex = 5;
            }
            else if (milliVolt > 696 && milliVolt <= 795)
            {
                uvIndex = 6;
            }
            else if (milliVolt > 795 && milliVolt <= 881)
            {
                uvIndex = 7;
            }
            else if (milliVolt > 881 && milliVolt <= 976)
            {
                uvIndex = 8;
            }
            else if (milliVolt > 976 && milliVolt <= 1079)
            {
                uvIndex = 9;
            }
            else if (milliVolt > 1079 && milliVolt <= 1170)
            {
                uvIndex = 10;
            }
            else if (milliVolt > 1170)
            {
                uvIndex = 11;
            }

            return uvIndex;      
        }
    }
}
