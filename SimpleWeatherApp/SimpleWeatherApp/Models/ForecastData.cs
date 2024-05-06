using System.Collections.Generic;

namespace SimpleWeatherApp.Models
{
    public class ForecastData
    {
        public List<WeatherData> DailyForecasts { get; set; } = new List<WeatherData>();

        
        public ForecastData()
        {
            DailyForecasts = new List<WeatherData>();
        }

    }
}
