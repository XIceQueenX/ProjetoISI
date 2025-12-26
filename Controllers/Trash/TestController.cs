using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Trabalho_ISI.Data;

namespace Trabalho_ISI.Controllers.Trash
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public TestController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyTestApp/1.0");
        }


        // GET api/test/tmdb
        [HttpGet("tmdb")]
        [AllowAnonymous]
        public async Task<IActionResult> TestTMDB()
        {
            // Hardcoded TMDB URL with working API key
            var url = "https://api.themoviedb.org/3/movie/popular?api_key=a2a1d2e30fd76a6543814e665c3a80dc&language=pt-BR&page=1";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest(new
                    {
                        message = "TMDB returned error",
                        status = response.StatusCode,
                        content = errorContent
                    });
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseBody);
                var firstMovie = json.RootElement.GetProperty("results")[0];

                return Ok(new
                {
                    message = "TMDB API OK",
                    sampleMovie = firstMovie
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Erro TMDB",
                    error = ex.Message
                });
            }
        }



        [HttpGet("books")]
        public async Task<IActionResult> TestGoogleBooks()
        {
            var apiKey = _configuration["ExternalAPIs:GoogleBooks:ApiKey"];
            var baseUrl = _configuration["ExternalAPIs:GoogleBooks:BaseUrl"];
            var url = $"{baseUrl}/volumes?q=subject:fiction&maxResults=1&key={apiKey}";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var json = JsonDocument.Parse(response);
                var firstBook = json.RootElement.GetProperty("items")[0];

                return Ok(new { message = "Google Books API OK", sampleBook = firstBook });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro Google Books", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> TestAll()
        {
            var tmdbResult = await TestTMDB() as ObjectResult;
            var booksResult = await TestGoogleBooks() as ObjectResult;

            return Ok(new
            {
                TMDB = tmdbResult?.Value,
                GoogleBooks = booksResult?.Value
            });
        }
        /*[HttpGet("savebatch")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveBatch()
        {
            var seeder = new DataSeeder(_context, _httpClient);
            await seeder.SeedAsync();
            return Ok(new { message = "50 movies and 50 books saved into database" });
        }*/


    }
}
