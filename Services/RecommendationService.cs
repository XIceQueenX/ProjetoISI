using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly AppDbContext _context;

        public RecommendationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RecommendationResult> GetMovieRecommendationsForBookAsync(string bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                throw new Exception("Livro não encontrado");

            // Extrair palavras-chave do título e descrição
            var keywords = ExtractKeywords($"{book.Title} {book.Description}");

            if (!keywords.Any())
                return new RecommendationResult
                {
                    Source = new { book.Id, book.Title, book.Authors },
                    Recommendations = new List<object>(),
                    TotalRecommendations = 0
                };

            // Procurar filmes que contenham pelo menos uma palavra-chave
            var movies = await _context.Movies
                .Where(m => keywords.Any(k =>
                    m.Title.Contains(k, StringComparison.OrdinalIgnoreCase) ||
                    m.OriginalTitle.Contains(k, StringComparison.OrdinalIgnoreCase) ||
                    m.Overview.Contains(k, StringComparison.OrdinalIgnoreCase)))
                .Take(10)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Overview,
                    m.ReleaseDate,
                    m.PosterPath,
                    MatchReason = "Baseado no livro: " + book.Title
                })
                .ToListAsync();

            return new RecommendationResult
            {
                Source = new { book.Id, book.Title, book.Authors },
                Recommendations = movies.Cast<object>().ToList(),
                TotalRecommendations = movies.Count
            };
        }

        public async Task<RecommendationResult> GetBookRecommendationsForMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);

            if (movie == null)
                throw new Exception("Filme não encontrado");

            var keywords = ExtractKeywords($"{movie.Title} {movie.Overview}");

            var books = await _context.Books
                .Where(b => keywords.Any(k =>
                    b.Title.Contains(k) ||
                    b.Description.Contains(k)))
                .Take(10)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Authors,
                    b.Description,
                    b.PublishedDate,
                    MatchReason = "Baseado no filme: " + movie.Title
                })
                .ToListAsync();

            return new RecommendationResult
            {
                Source = new { movie.Id, movie.Title, movie.ReleaseDate },
                Recommendations = books.Cast<object>().ToList(),
                TotalRecommendations = books.Count
            };
        }

        public async Task<PersonalizedRecommendations> GetPersonalizedRecommendationsAsync(
            PreferencesDto preferences)
        {
            var movieRecommendations = new List<Movie>();
            var bookRecommendations = new List<Book>();

            // Pesquisar por gênero
            if (!string.IsNullOrEmpty(preferences.Genre))
            {
                movieRecommendations.AddRange(await _context.Movies
                    .Where(m => m.Overview.Contains(preferences.Genre))
                    .Take(5)
                    .ToListAsync());

                bookRecommendations.AddRange(await _context.Books
                    .Where(b => b.Description.Contains(preferences.Genre))
                    .Take(5)
                    .ToListAsync());
            }

            // Pesquisar por palavras-chave
            if (preferences.Keywords != null && preferences.Keywords.Any())
            {
                foreach (var keyword in preferences.Keywords)
                {
                    var movies = await _context.Movies
                        .Where(m => m.Title.Contains(keyword) || m.Overview.Contains(keyword))
                        .Take(3)
                        .ToListAsync();
                    movieRecommendations.AddRange(movies);

                    var books = await _context.Books
                        .Where(b => b.Title.Contains(keyword) || b.Description.Contains(keyword))
                        .Take(3)
                        .ToListAsync();
                    bookRecommendations.AddRange(books);
                }
            }

            return new PersonalizedRecommendations
            {
                Movies = movieRecommendations.Distinct().Take(10).ToList(),
                Books = bookRecommendations.Distinct().Take(10).ToList()
            };
        }

        public async Task<List<BookMovieMatch>> FindBookMovieMatchesAsync()
        {
            var books = await _context.Books.Take(50).ToListAsync();
            var matches = new List<BookMovieMatch>();

            foreach (var book in books)
            {
                var keywords = ExtractKeywords(book.Title);

                var matchingMovies = await _context.Movies
                    .Where(m => keywords.Any(k => m.Title.Contains(k)))
                    .Take(3)
                    .Select(m => new { m.Id, m.Title, m.ReleaseDate })
                    .ToListAsync();

                if (matchingMovies.Any())
                {
                    matches.Add(new BookMovieMatch
                    {
                        Book = new { book.Id, book.Title, book.Authors },
                        Movies = matchingMovies.Cast<object>().ToList()
                    });
                }
            }

            return matches;
        }

        // Método auxiliar para extrair palavras-chave
        private List<string> ExtractKeywords(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            var stopWords = new[] { "the", "a", "an", "and", "or", "but", "de", "da", "do", "o", "a", "em" };

            var words = text.ToLower()
                .Split(new[] { ' ', ',', '.', '!', '?', ':', ';', '-' },
                       StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 3 && !stopWords.Contains(w))
                .Distinct()
                .ToList();

            return words;
        }
    }
}