namespace SimpleWeatherApp.Models
{
    public class WeatherData
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public double high_temp { get; set; }
        public double LowTemp { get; set; }
        public string Description { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Date { get; set; }
    }
}
