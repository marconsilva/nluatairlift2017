using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels
{
    public class CurrentWeatherResponse
    {
        public Coordenates coord { get; set; }
        public List<WeaterData> weather { get; set; }
        [JsonProperty("base")]
        public string baseParameter { get; set; }
        public MainWeatherData main { get; set; }
        public long visibility { get; set; }
        public WindWeatherData wind { get; set; }
        public CloudsWeatherData clouds { get; set; }
        public long dt { get; set; }
        [JsonIgnore]
        public DateTime dateTime { get { return DateTime.FromBinary(dt); } }
        public WeatherSysData sys { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

    }
}
