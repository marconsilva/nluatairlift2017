using Newtonsoft.Json;
using System;

namespace Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels
{
    public class WeatherSysData
    {
        public int type { get; set; }
        public int id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
        [JsonIgnore]
        public DateTime sunriseDateTime { get { return DateTime.FromBinary(sunrise); } }
        [JsonIgnore]
        public DateTime sunsetDateTime { get { return DateTime.FromBinary(sunset); } }

    }

}