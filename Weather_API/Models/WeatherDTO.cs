using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCoreApi.Models
{

    public class WeatherDTO
    {
        public decimal? Temperature { get; set; }
        public decimal? TemperatureMax { get; set; }
        public decimal? TemperatureMin { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? Pressure { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
