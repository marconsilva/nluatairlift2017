using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels
{
    public sealed class ParsedWeatherForecast
    {
        public WeatherType Forecast { get; set; }
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }
        public DateTime Date { get; set; }

        public ParsedWeatherForecast(CurrentWeatherResponse forecast)
        {
            Date = forecast.dateTime;

            var main = forecast.weather.FirstOrDefault();
            switch (main.id)
            {
                case 500:
                    Forecast = WeatherType.LightRain;
                    break;
                case 803:
                    Forecast = WeatherType.Cloudy;
                    break;
                default:
                    Forecast = WeatherType.Unknown;
                    break;
            }

            MinTemperature = forecast.main.temp_min;
            MaxTemperature = forecast.main.temp_max;
        }
    }
}
