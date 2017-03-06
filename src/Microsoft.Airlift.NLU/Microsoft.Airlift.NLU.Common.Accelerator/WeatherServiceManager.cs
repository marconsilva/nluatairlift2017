using Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class WeatherServiceManager
    {
        private readonly string weatherServiceBaseUri = "http://api.openweathermap.org/";
        private readonly string requestUrlCurrentWeatherFormat = "http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

        private string apiKey;
        public WeatherServiceManager(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<ParsedWeatherForecast> GetCurrentWeather()
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = false };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(weatherServiceBaseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "AirliftDemo");
                try
                {
                    var response = await client.GetAsync(requestUrlCurrentWeatherFormat.Replace("{apiKey}", apiKey).Replace("{city}", "Lisbon"));
                    return new ParsedWeatherForecast(
                        JsonConvert.DeserializeObject<CurrentWeatherResponse>(
                            await response.Content.ReadAsStringAsync()));
                    
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}


