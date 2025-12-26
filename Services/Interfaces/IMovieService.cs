namespace Trabalho_ISI.Services.Interfaces
{
    using global::Trabalho_ISI.Data;

    namespace Trabalho_ISI.Services.Interfaces
    {
        public interface IMovieService
        {
            Task<PaginatedResult<Movie>> GetAllMoviesAsync(int page, int pageSize, string? search);
            Task<Movie?> GetMovieByIdAsync(int id);
            Task<Movie> CreateMovieAsync(MovieCreateDto dto);
            Task<Movie?> UpdateMovieAsync(int id, MovieUpdateDto dto);
            Task<bool> DeleteMovieAsync(int id);
            Task<List<Movie>> GetRecentMoviesAsync(int count);
            Task<List<Movie>> GetMoviesByYearAsync(string year);
            Task<int> GetTotalMoviesCountAsync();
        }
    }
}
