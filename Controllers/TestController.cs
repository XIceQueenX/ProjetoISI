using Microsoft.AspNetCore.Mvc;
using Trabalho_ISI.Services;

[ApiController]
[Route("[controller]")]
public class SeedController : ControllerBase
{
    private readonly IDataSeederService _seeder;

    public SeedController(IDataSeederService seeder)
    {
        _seeder = seeder;
    }

    [HttpPost]
    public async Task<IActionResult> Seed(int movies = 10, int books = 10)
    {
        var result = await _seeder.SeedAsync(movies, books);
        return Ok(result);
    }
}
