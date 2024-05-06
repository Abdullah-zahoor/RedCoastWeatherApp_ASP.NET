using SimpleWeatherApp.Models;
using System.Threading.Tasks;

namespace SimpleWeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetCurrentWeatherAsync(string city);
        Task<ForecastData> GetForecastAsync(string city);  
    }
}
