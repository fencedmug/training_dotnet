using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class RedirectController : ControllerBase
{
    private readonly ILogger<RedirectController> _logger;
    private readonly HttpClient _httpClient;

    public RedirectController(ILogger<RedirectController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpGet("weather")]
    public async Task<IActionResult> GetWeatherFromExternal()
    {
        try
        {
            var result = await _httpClient.GetAsync("http://webapi/WeatherForecast/");
            _logger.LogInformation($"Webapi returned: {result.StatusCode} {(int)result.StatusCode}");
            return Ok(await result.Content.ReadAsStringAsync());
        }
        catch
        {
            return  NotFound("Failed to get weather forecast");
        }
    }
}