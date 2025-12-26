using Trabalho_ISI.Data;

namespace Trabalho_ISI.Services.Interfaces
{
    /// <summary>
    /// Service interface for recommending movies and books based on titles or preferences.
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Gets a list of movie recommendations based on a book's title.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <returns>Recommendation result containing matching movies.</returns>
        Task<RecommendationResult> GetMovieRecommendationsForBookAsync(string bookId);

        /// <summary>
        /// Gets a list of book recommendations based on a movie's title.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>Recommendation result containing matching books.</returns>
        Task<RecommendationResult> GetBookRecommendationsForMovieAsync(int movieId);

        /// <summary>
        /// Gets personalized recommendations of books and movies based on user preferences.
        /// </summary>
        /// <param name="preferences">User preferences, such as genre or keywords.</param>
        /// <returns>Personalized recommendations containing movies and books.</returns>
        Task<PersonalizedRecommendations> GetPersonalizedRecommendationsAsync(PreferencesDto preferences);
    }

    /// <summary>
    /// Represents a recommendation result for a source item.
    /// </summary>
    public class RecommendationResult
    {
        /// <summary>
        /// The source item (book or movie) for which recommendations are generated.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// List of recommended items (books or movies).
        /// </summary>
        public List<object> Recommendations { get; set; }

        /// <summary>
        /// Total number of recommendations returned.
        /// </summary>
        public int TotalRecommendations { get; set; }
    }

    /// <summary>
    /// Represents personalized recommendations for a user.
    /// </summary>
    public class PersonalizedRecommendations
    {
        public List<Movie> Movies { get; set; }
        public List<Book> Books { get; set; }
    }

    /// <summary>
    /// Represents a mapping of a book to matching movies.
    /// </summary>
    public class BookMovieMatch
    {
        public object Book { get; set; }
        public List<object> Movies { get; set; }
    }

    /// <summary>
    /// User preferences used for generating personalized recommendations.
    /// </summary>
    public class PreferencesDto
    {
        /// <summary>
        /// Optional genre preference.
        /// </summary>
        public string? Genre { get; set; }

        /// <summary>
        /// Optional list of keywords for recommendation.
        /// </summary>
        public List<string>? Keywords { get; set; }
    }
}
