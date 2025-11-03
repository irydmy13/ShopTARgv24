using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _cfg;

        public OpenWeatherService(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _cfg = cfg;
        }

        public async Task<OpenWeatherDto?> GetCurrentAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;

            var key = _cfg["OpenWeather:ApiKey"] ?? "";
            var units = _cfg["OpenWeather:Units"] ?? "metric";
            var lang = _cfg["OpenWeather:Lang"] ?? "en";

            var url = $"weather?q={Uri.EscapeDataString(city)}&appid={key}&units={units}&lang={lang}";

            using var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<Root>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (root == null) return null;

            return new OpenWeatherDto
            {
                City = root.name,
                Country = root.sys?.country,
                Temp = root.main?.temp ?? 0,
                FeelsLike = root.main?.feels_like ?? 0,
                Humidity = root.main?.humidity ?? 0,
                WindSpeed = root.wind?.speed ?? 0,
                Description = root.weather != null && root.weather.Length > 0 ? root.weather[0].description : null,
                Icon = root.weather != null && root.weather.Length > 0 ? root.weather[0].icon : null
            };
        }

        #region minimal API models
        private class Root
        {
            public string name { get; set; }
            public Sys sys { get; set; }
            public Main main { get; set; }
            public Wind wind { get; set; }
            public Weather[] weather { get; set; }
        }
        private class Sys { public string country { get; set; } }
        private class Main { public double temp { get; set; } public double feels_like { get; set; } public int humidity { get; set; } }
        private class Wind { public double speed { get; set; } }
        private class Weather { public string description { get; set; } public string icon { get; set; } }
        #endregion
    }
}
