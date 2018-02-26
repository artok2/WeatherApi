using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AzureCoreApi.Models
{
    
    [Produces("application/json")]
    [Route("api/Weathers")]
    public class WeathersController : Controller
    {
        private readonly AzureCoreApiContext _context;

        public WeathersController(AzureCoreApiContext context)
        {
            _context = context;
        }
        // Typed lambda expression for Select() method. 
        private static readonly Expression<Func<Weather, WeatherDTO>> AsWeatherDto =
         x => new WeatherDTO
         {
             Temperature = x.Temperature,
             TemperatureMax = x.TemperatureMax,
             TemperatureMin = x.TemperatureMin,
             Humidity = x.Humidity,
             Pressure = x.Pressure,
             CreateDate = x.CreateDate
         }; 

        // GET: api/Weathers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var weatherList = await _context.Weather.ToListAsync();
            return Ok(weatherList);
        }
        [Route("~/api/GetByDateTime/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByDateTime([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weahter = await _context.Weather.Where(b =>
                       b.CreateDate.Value.Year == id.Year
                       && b.CreateDate.Value.Month == id.Month
                       && b.CreateDate.Value.Day == id.Day
                       && b.CreateDate.Value.Hour == id.Hour
                            ).Select(AsWeatherDto).SingleOrDefaultAsync();
         
            if (weahter == null)
            {
                return NotFound();
            }
            return Ok(weahter);
        }
        
        // PUT: api/Weathers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeather([FromRoute] int id, [FromBody] Weather weather)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weather.WeatherID)
            {
                return BadRequest();
            }

            _context.Entry(weather).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/Weathers
        [HttpPost]
        public async Task<IActionResult> PostWeather([FromBody] Weather weather)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Weather.Add(weather);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeather", new { id = weather.WeatherID }, weather);
        }

        // DELETE: api/Weathers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeather([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var weather = await _context.Weather.SingleOrDefaultAsync(m => m.WeatherID == id);
            if (weather == null)
            {
                return NotFound();
            }

            _context.Weather.Remove(weather);
            await _context.SaveChangesAsync();

            return Ok(weather);
        }
       
        private bool WeatherExists(int id)
        {
            return _context.Weather.Any(e => e.WeatherID == id);
        }
    }
}