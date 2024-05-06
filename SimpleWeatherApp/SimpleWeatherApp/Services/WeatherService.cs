using SimpleWeatherApp.Models;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleWeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7f1e263586da40c5b752f991e8be042f"; 

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData> GetCurrentWeatherAsync(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.weatherbit.io/v2.0/current?city={city}&key={ApiKey}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var weatherDataRoot = JsonSerializer.Deserialize<WeatherDataRoot>(content, jsonOptions);

                if (weatherDataRoot != null && weatherDataRoot.Data.Count > 0)
                {
                    var weather = weatherDataRoot.Data[0];
                    return new WeatherData
                    {
                        City = city,
                        Temperature = weather.Temp,
                        high_temp = weather.high_temp,
                        LowTemp = weather.high_temp,
                        Description = weather.Weather.Description,
                        Humidity = weather.Rh,
                        WindSpeed = weather.WindSpd
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null; 
        }


        private class WeatherDataRoot
        {
            public List<WeatherInfo> Data { get; set; }
        }

        private class WeatherInfo
        {
            public double Temp { get; set; }
            public double high_temp { get; set; }
            public string Description { get; set; }
            public double Rh { get; set; }
            public double WindSpd { get; set; }
            public WeatherDetails Weather { get; set; }
        }

        private class WeatherDetails
        {
            public string Description { get; set; }
            public double high_temp { get; set; }
        }

        public async Task<ForecastData> GetForecastAsync(string city)
        {
            var url = $"https://api.weatherbit.io/v2.0/forecast/daily?city={city}&key={ApiKey}&days=5";
            var response = await _httpClient.GetAsync(url);

            
            Console.WriteLine($"Request URL: {url}");

            
            Console.WriteLine($"Response Status Code: {response.StatusCode}");

            // Check if the response is successful before processing
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Log the API response content
                Console.WriteLine($"Response Content: {content}");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var forecastData = JsonSerializer.Deserialize<ForecastDataRoot>(content, options);

                if (forecastData != null && forecastData.Data.Count > 0)
                {
                    return new ForecastData
                    {
                        DailyForecasts = forecastData.Data.Select(d => new WeatherData
                        {
                            Temperature = d.Temp,
                            high_temp = d.app_max_temp,
                            LowTemp = d.app_min_temp,
                            Description = d.Weather?.Description,
                            Date = DateTime.TryParseExact(d.valid_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
                                            ? date
                                            : DateTime.MinValue
                        }).ToList()
                    };
                }
            }
            else
            {
                
                Console.WriteLine($"Failed to retrieve forecast data. Reason: {response.ReasonPhrase}");
            }
            return new ForecastData();
        }




        private class ForecastDataRoot
        {
            public List<ForecastDay> Data { get; set; }
        }

        private class ForecastDay
        {
            public double Temp { get; set; }
            public double app_max_temp { get; set; }
            public double app_min_temp { get; set; }
            public string valid_date { get; set; }
            public WeatherDetails Weather { get; set; }
        }



    }
}
