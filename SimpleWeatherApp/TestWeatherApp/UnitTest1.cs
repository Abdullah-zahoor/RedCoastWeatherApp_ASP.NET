using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SimpleWeatherApp.Services;


namespace SimpleWeatherApp.Tests
{
    public class WeatherServiceTests
    {
        // Test case for successful retrieval of current weather data
        [Fact]
        public async Task GetCurrentWeatherAsync_Success()
        {
            
            var httpClient = new Mock<HttpClient>();
            var expectedCity = "New York";
            var responseContent = @"
            {
                ""data"": [
                    {
                        ""temp"": 25,
                        ""app_temp"": 26,
                        ""weather"": {
                            ""description"": ""Clear sky""
                        },
                        ""rh"": 50,
                        ""wind_spd"": 10
                    }
                ]
            }";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };
            httpClient.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(httpResponseMessage);
            var weatherService = new WeatherService(httpClient.Object);

          
            var weatherData = await weatherService.GetCurrentWeatherAsync(expectedCity);

            
            Assert.NotNull(weatherData);
            Assert.Equal(expectedCity, weatherData.City);
            Assert.Equal(25, weatherData.Temperature);
            Assert.Equal(26, weatherData.high_temp);
            Assert.Equal(25, weatherData.LowTemp);
            Assert.Equal("Clear sky", weatherData.Description);
            Assert.Equal(50, weatherData.Humidity);
            Assert.Equal(10, weatherData.WindSpeed);
        }

        // Test case for handling invalid city name
        [Fact]
        public async Task GetCurrentWeatherAsync_InvalidCity()
        {
            
            var httpClient = new Mock<HttpClient>();
            var invalidCity = "InvalidCity";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            httpClient.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(httpResponseMessage);
            var weatherService = new WeatherService(httpClient.Object);

            
            var weatherData = await weatherService.GetCurrentWeatherAsync(invalidCity);

          
            Assert.Null(weatherData);
        }

        // Test case for handling exception during HTTP request
        [Fact]
        public async Task GetCurrentWeatherAsync_HttpException()
        {
            
            var httpClient = new Mock<HttpClient>();
            var city = "New York";
            var exceptionMessage = "Internal Server Error";
            httpClient.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ThrowsAsync(new HttpRequestException(exceptionMessage));
            var weatherService = new WeatherService(httpClient.Object);

           
            await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetCurrentWeatherAsync(city));
        }

        
       

        
    }
}
