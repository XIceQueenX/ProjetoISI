using global::Trabalho_ISI.Data;

namespace Trabalho_ISI.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing movies.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Retrieves a paginated list of movies, optionally filtered by a search string.
        /// </summary>
        Task<PaginatedResult<Movie>> GetAllMoviesAsync(int page, int pageSize, string? search);

        /// <summary>
        /// Retrieves a single movie by its ID.
        /// Returns null if the movie does not exist.
        /// </summary>
        Task<Movie?> GetMovieByIdAsync(int id);

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        Task<Movie> CreateMovieAsync(MovieCreateDto dto);

        /// <summary>
        /// Updates an existing movie by ID.
        /// Returns null if the movie does not exist.
        /// </summary>
        Task<Movie?> UpdateMovieAsync(int id, MovieUpdateDto dto);

        /// <summary>
        /// Deletes a movie by ID.
        /// Returns true if deleted successfully, false otherwise.
        /// </summary>
        Task<bool> DeleteMovieAsync(int id);

        /// <summary>
        /// Retrieves a list of the most recent movies.
        /// </summary>
        Task<List<Movie>> GetRecentMoviesAsync(int count);

        /// <summary>
        /// Retrieves movies released in a specific year.
        /// </summary>
        Task<List<Movie>> GetMoviesByYearAsync(string year);

        /// <summary>
        /// Returns the total number of movies.
        /// </summary>
        Task<int> GetTotalMoviesCountAsync();
    }
}
