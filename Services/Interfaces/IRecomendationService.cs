using Trabalho_ISI.Data;

namespace Trabalho_ISI.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<RecommendationResult> GetMovieRecommendationsForBookAsync(string bookId);
        Task<RecommendationResult> GetBookRecommendationsForMovieAsync(int movieId);
        Task<PersonalizedRecommendations> GetPersonalizedRecommendationsAsync(PreferencesDto preferences);
        Task<List<BookMovieMatch>> FindBookMovieMatchesAsync();
    }

    public class RecommendationResult
    {
        public object Source { get; set; }
        public List<object> Recommendations { get; set; }
        public int TotalRecommendations { get; set; }
    }

    public class PersonalizedRecommendations
    {
        public List<Movie> Movies { get; set; }
        public List<Book> Books { get; set; }
    }

    public class BookMovieMatch
    {
        public object Book { get; set; }
        public List<object> Movies { get; set; }
    }

    public class PreferencesDto
    {
        public string? Genre { get; set; }
        public List<string>? Keywords { get; set; }
    }
}