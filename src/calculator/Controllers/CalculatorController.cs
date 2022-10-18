using Microsoft.AspNetCore.Mvc;

namespace calculator.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    [HttpGet("add")]
    public IActionResult Add(int a, int b)
    {
        return Ok(a + b);
    }

    [HttpGet("subtract")]
    public IActionResult Subtract(int a, int b)
    {
        return Ok(a - b);
    }
}
