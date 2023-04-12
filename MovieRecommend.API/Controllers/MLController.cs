using Microsoft.AspNetCore.Mvc;
using MovieRecommend.API.ML;

namespace MovieRecommend.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MLController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IMLService _mlService;

    public MLController(
        ILogger<WeatherForecastController> logger,
        IMLService mlService)
    {
        _logger = logger;
        _mlService = mlService;
    }

    [HttpGet(Name = "Calculate")]
    public string Calculate()
    {
        
        return _mlService.CalculateThings();
    }
}
