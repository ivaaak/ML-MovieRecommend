using Microsoft.AspNetCore.Mvc;
using MovieRecommend.API.Data.Structures;
using MovieRecommend.API.ML;

namespace MovieRecommend.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MLController : ControllerBase
{
    private readonly ILogger<MLController> _logger;

    private readonly IMLService _mlService;

    public MLController(
        ILogger<MLController> logger,
        IMLService mlService)
    {
        _logger = logger;
        _mlService = mlService;
    }

    [HttpGet("calculate")]
    public APIResultDTO Calculate()
    {
        return _mlService.RunModel();
    }

    [HttpGet("parametrized")]
    public APIResultDTO Parametrized(float movieID, float userID)
    {
        var result = new APIResultDTO();

        var data = _mlService.RunModelWithParams(movieID, userID);

        return data;
    }
}
