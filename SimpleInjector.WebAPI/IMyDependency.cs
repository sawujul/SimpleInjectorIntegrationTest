using System.Collections.Generic;

namespace SimpleInjector.WebAPI
{
    public interface IMyDependency
    {
        IEnumerable<WeatherForecast> GetWeatherForecasts();
    }
}