using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    /// <summary>
    /// Service responsible for generating recommendations between books and movies.
    /// </summary>
    public class RecommendationService : IRecommendationService
    {
        // Database context used to access Books and Movies tables
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor with dependency injection of the database context.
        /// </summary>
        public RecommendationService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recommends movies whose title matches the given book's title.
        /// </summary>
        /// <param name="bookId">ID of the book</param>
        /// <returns>A list of movie recommendations</returns>
        public async Task<RecommendationResult> GetMovieRecommendationsForBookAsync(string bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                throw new Exception("Livro não encontrado");

            var movies = await _context.Movies
    .Where(m => m.Title.ToLower() == book.Title.ToLower())
    .Select(m => new
    {
        m.Id,
        m.Title,
        m.Overview,
        m.ReleaseDate,
        m.PosterPath,
        MatchReason = "Mesma ideia do livro: " + book.Title
    })
    .ToListAsync();


            return new RecommendationResult
            {
                Source = new { book.Id, book.Title, book.Authors },
                Recommendations = movies.Cast<object>().ToList(),
                TotalRecommendations = movies.Count
            };
        }

        /// <summary>
        /// Recommends books whose title matches the given movie's title.
        /// </summary>
        /// <param name="movieId">ID of the movie</param>
        /// <returns>A list of book recommendations</returns>
        public async Task<RecommendationResult> GetBookRecommendationsForMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);

            if (movie == null)
                throw new Exception("Filme não encontrado");

            var books = await _context.Books
    .Where(b => b.Title.ToLower() == movie.Title.ToLower())
    .Select(b => new
    {
        b.Id,
        b.Title,
        b.Authors,
        b.Description,
        b.PublishedDate,
        MatchReason = "Mesma ideia do filme: " + movie.Title
    })
    .ToListAsync();


            return new RecommendationResult
            {
                Source = new { movie.Id, movie.Title, movie.ReleaseDate },
                Recommendations = books.Cast<object>().ToList(),
                TotalRecommendations = books.Count
            };
        }
    }
}
