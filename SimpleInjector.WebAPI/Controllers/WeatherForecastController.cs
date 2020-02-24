using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SimpleInjector.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMyDependency _dependency;

        public WeatherForecastController(IMyDependency dependency)
        {
            _dependency = dependency;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _dependency.GetWeatherForecasts();
        }
    }
}
