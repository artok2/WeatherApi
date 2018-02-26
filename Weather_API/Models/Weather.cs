using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCoreApi.Models
{
    public class Weather
    {
        public int WeatherID { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Pressure { get; set; }
        public Nullable<decimal> TemperatureMax { get; set; }
        public Nullable<decimal> TemperatureMin { get; set; }
        public Nullable<decimal> Humidity { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
