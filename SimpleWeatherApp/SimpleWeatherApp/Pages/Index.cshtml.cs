using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleWeatherApp.Models;
using SimpleWeatherApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWeatherApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWeatherService _weatherService;

        public IndexModel(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [BindProperty]
        public string City { get; set; }  // To capture user input for the city name.

        public WeatherData Weather { get; set; }  // To hold the current weather data.
        public ForecastData Forecast { get; set; }  // To hold the 5-day forecast data.

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(City))
            {
                // Attempt to fetch the current weather.
                Weather = await _weatherService.GetCurrentWeatherAsync(City);

                // Attempt to fetch the 5-day weather forecast.
                Forecast = await _weatherService.GetForecastAsync(City);

               
                if (Weather == null || Forecast == null || Forecast.DailyForecasts.Count == 0)
                {
                    ModelState.AddModelError("", "No weather or forecast data found for the specified city. Please ensure the city name is correct and try again.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please enter a city name to get the weather information.");
            }
            return Page();
        }
    }
}
