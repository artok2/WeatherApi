using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Weatherfunc
{


    public class Weather
    {
        private const string Temp = "temp";
        private const string PressureSelector = "pressure";
        private const string HumiditySelector = "humidity";
        private const string TempMin = "temp_min";
        private const string TempMax = "temp_max";

        public double Temperature { get; set; }
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }

        public Weather(JToken WeatherData)
        {
            Temperature = double.Parse(WeatherData.SelectToken(Temp).ToString());
            TemperatureMin = double.Parse(WeatherData.SelectToken(TempMin).ToString());
            TemperatureMax = double.Parse(WeatherData.SelectToken(TempMax).ToString());
            Pressure = double.Parse(WeatherData.SelectToken(PressureSelector).ToString());
            Humidity = double.Parse(WeatherData.SelectToken(HumiditySelector).ToString());
        }
    }
    
    public static class TimerGetWeather2
    {
        [FunctionName("TimerGetWeather2")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            string city = "Turku";
            string apid =   ConfigurationManager.AppSettings["API_ID"].ToString();
            string ValidCod = "200";
            string COD = "cod";
            string MainSelector = "main";
            bool ValidRequest = true;
            string sBaseAdress = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={apid}";

            JObject jsonData = JObject.Parse(new System.Net.WebClient().DownloadString(sBaseAdress));

            if (jsonData.SelectToken(COD).ToString() == ValidCod)
            {                              
                Weather pWeather = new Weather(jsonData.SelectToken(MainSelector));
                DBinsert(pWeather, log);
            }
            else
            {
                ValidRequest = false;
            }
            log.Info($"C# Timer trigger function executed at: {DateTime.Now} with result: {ValidRequest}");
        }
         

        public static string SQLins = "INSERT INTO [Weather] ([Temperature],[TemperatureMax],[TemperatureMin],[Pressure],[Humidity],[CreateDate]) " +
                                            "VALUES (@Temperature,@TemperatureMax,@TemperatureMin,@Pressure,@Humidity, GETDATE())";

        public static void DBinsert(Weather entry, TraceWriter log)
        {
            log.Info($"DBinsert starts");
            try
            {               
                var connstr = ConfigurationManager.ConnectionStrings["con_weather2"].ConnectionString;     
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    //log.Info($"Temp: {entry.Temperature}");

                    using (SqlCommand cmd = new SqlCommand(SQLins, conn))
                    {
                        cmd.Parameters.AddWithValue("@Temperature", entry.Temperature);
                        cmd.Parameters.AddWithValue("@TemperatureMax", entry.TemperatureMax);
                        cmd.Parameters.AddWithValue("@TemperatureMin", entry.TemperatureMin);
                        cmd.Parameters.AddWithValue("@Pressure", entry.Pressure);
                        cmd.Parameters.AddWithValue("@Humidity", entry.Humidity);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            log.Verbose($"Row Inserted: {rows}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Verbose($"Error inserting row: {ex.Message} {ex.InnerException}");
            }

            
        }

    }

}