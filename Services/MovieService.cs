using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    /// <summary>
    /// Service responsible for CRUD operations and queries related to movies.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all movies with optional search and pagination.
        /// </summary>
        public async Task<PaginatedResult<Movie>> GetAllMoviesAsync(int page, int pageSize, string? search)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.Title.Contains(search) ||
                    m.OriginalTitle.Contains(search) ||
                    m.Overview.Contains(search));
            }

            var total = await query.CountAsync();
            var movies = await query
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Movie>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Data = movies
            };
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// Returns null if not found.
        /// </summary>
        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        /// <summary>
        /// Creates a new movie record.
        /// </summary>
        public async Task<Movie> CreateMovieAsync(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                OriginalTitle = dto.OriginalTitle ?? dto.Title,
                Overview = dto.Overview,
                ReleaseDate = dto.ReleaseDate,
                PosterPath = PrependImageUrl(dto.PosterPath),
                BackdropPath = PrependImageUrl(dto.BackdropPath)
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        /// <summary>
        /// Updates an existing movie by ID.
        /// Returns null if movie not found.
        /// </summary>
        public async Task<Movie?> UpdateMovieAsync(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return null;

            movie.Title = dto.Title ?? movie.Title;
            movie.OriginalTitle = dto.OriginalTitle ?? movie.OriginalTitle;
            movie.Overview = dto.Overview ?? movie.Overview;
            movie.ReleaseDate = dto.ReleaseDate ?? movie.ReleaseDate;
            if (dto.PosterPath != null) movie.PosterPath = PrependImageUrl(dto.PosterPath);
            if (dto.BackdropPath != null) movie.BackdropPath = PrependImageUrl(dto.BackdropPath);

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return movie;
        }

        /// <summary>
        /// Deletes a movie by ID.
        /// Returns true if deleted successfully.
        /// </summary>
        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves the most recent movies.
        /// </summary>
        public async Task<List<Movie>> GetRecentMoviesAsync(int count)
        {
            return await _context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves movies filtered by release year.
        /// </summary>
        public async Task<List<Movie>> GetMoviesByYearAsync(string year)
        {
            return await _context.Movies
                .Where(m => m.ReleaseDate.StartsWith(year))
                .ToListAsync();
        }

        /// <summary>
        /// Returns the total number of movies.
        /// </summary>
        public async Task<int> GetTotalMoviesCountAsync()
        {
            return await _context.Movies.CountAsync();
        }

        /// <summary>
        /// Prepends the TMDB image base URL if the path is relative.
        /// </summary>
        string? PrependImageUrl(string? path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (path.StartsWith("http")) return path; // already a full URL

            return $"https://image.tmdb.org/t/p/w500{path}";
        }
    }

    /// <summary>
    /// DTO for creating a movie.
    /// </summary>
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
    }

    /// <summary>
    /// DTO for updating a movie.
    /// All fields are optional.
    /// </summary>
    public class MovieUpdateDto
    {
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Overview { get; set; }
        public string? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
    }
}
