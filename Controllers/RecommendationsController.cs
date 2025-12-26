using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        /// <summary>
        /// Recomendar filmes baseados num livro
        /// </summary>
        [HttpGet("movies-for-book/{bookId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetMovieRecommendationsForBook(string bookId)
        {
            try
            {
                var result = await _recommendationService.GetMovieRecommendationsForBookAsync(bookId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Recomendar livros baseados num filme
        /// </summary>
        [HttpGet("books-for-movie/{movieId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBookRecommendationsForMovie(int movieId)
        {
            try
            {
                var result = await _recommendationService.GetBookRecommendationsForMovieAsync(movieId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Recomendações personalizadas
        /// </summary>
        [HttpPost("personalized")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPersonalizedRecommendations(PreferencesDto preferences)
        {
            var result = await _recommendationService.GetPersonalizedRecommendationsAsync(preferences);
            return Ok(result);
        }

        /// <summary>
        /// Encontrar correspondências entre livros e filmes
        /// </summary>
        [HttpGet("matches")]
        [AllowAnonymous]
        public async Task<ActionResult> FindBookMovieMatches()
        {
            var matches = await _recommendationService.FindBookMovieMatchesAsync();
            return Ok(new
            {
                totalMatches = matches.Count,
                matches = matches.Take(20)
            });
        }
    }
}