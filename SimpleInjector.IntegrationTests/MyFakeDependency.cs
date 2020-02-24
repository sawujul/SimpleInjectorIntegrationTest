using SimpleInjector.WebAPI;
using System;
using System.Collections.Generic;

namespace SimpleInjector.IntegrationTests
{
    public class MyFakeDependency : IMyDependency
    {
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return new[] { new WeatherForecast { Date = DateTime.MinValue, Summary = "This is a Fake Summary", TemperatureC = 9001 } };
        }
    }
}
