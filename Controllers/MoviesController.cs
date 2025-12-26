using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;

namespace Trabalho_ISI.Controllers
{
    /// <summary>
    /// Controller responsible for managing movie-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // Database context for accessing movie data
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor with dependency injection of the database context.
        /// </summary>
        /// <param name="context">Application database context</param>
        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all movies with pagination and optional search (PUBLIC).
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="search">Optional search string (title or original title)</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.Title.Contains(search) ||
                    m.OriginalTitle.Contains(search));
            }

            var total = await query.CountAsync();

            var movies = await query
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize),
                data = movies
            });
        }

        /// <summary>
        /// Retrieves a specific movie by its ID (PUBLIC).
        /// </summary>
        /// <param name="id">Movie identifier</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            return Ok(movie);
        }

        /// <summary>
        /// Creates a new movie (ADMIN ONLY).
        /// </summary>
        /// <param name="movieDto">Movie creation data</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Movie>> CreateMovie(MovieCreateDto movieDto)
        {
            var movie = new Movie
            {
                Title = movieDto.Title,
                OriginalTitle = movieDto.OriginalTitle ?? movieDto.Title,
                Overview = movieDto.Overview,
                ReleaseDate = movieDto.ReleaseDate,
                PosterPath = movieDto.PosterPath,
                BackdropPath = movieDto.BackdropPath
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Updates an existing movie completely (ADMIN ONLY).
        /// </summary>
        /// <param name="id">Movie identifier</param>
        /// <param name="movieDto">Updated movie data</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            movie.Title = movieDto.Title ?? movie.Title;
            movie.OriginalTitle = movieDto.OriginalTitle ?? movie.OriginalTitle;
            movie.Overview = movieDto.Overview ?? movie.Overview;
            movie.ReleaseDate = movieDto.ReleaseDate ?? movie.ReleaseDate;
            movie.PosterPath = movieDto.PosterPath ?? movie.PosterPath;
            movie.BackdropPath = movieDto.BackdropPath ?? movie.BackdropPath;

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        /// <summary>
        /// Deletes a movie by its ID (ADMIN ONLY).
        /// </summary>
        /// <param name="id">Movie identifier</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Movie deleted successfully" });
        }

        /// <summary>
        /// Retrieves movies released in a specific year (PUBLIC).
        /// </summary>
        /// <param name="year">Release year</param>
        [HttpGet("by-year/{year}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByYear(string year)
        {
            var movies = await _context.Movies
                .Where(m => m.ReleaseDate.StartsWith(year))
                .ToListAsync();

            return Ok(movies);
        }

        /// <summary>
        /// Retrieves the most recently released movies (PUBLIC).
        /// </summary>
        /// <param name="count">Number of movies to return</param>
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetRecentMovies([FromQuery] int count = 10)
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(count)
                .ToListAsync();

            return Ok(movies);
        }

        /// <summary>
        /// Partially updates a movie using PATCH (ADMIN ONLY).
        /// </summary>
        /// <param name="id">Movie identifier</param>
        /// <param name="dto">Partial movie update data</param>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            // Apply only provided fields
            if (dto.Title != null)
                movie.Title = dto.Title;

            if (dto.Overview != null)
                movie.Overview = dto.Overview;

            if (dto.ReleaseDate != null)
                movie.ReleaseDate = dto.ReleaseDate;

            if (dto.PosterPath != null)
                movie.PosterPath = dto.PosterPath;

            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }

    /// <summary>
    /// DTO used when creating a new movie.
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
    /// DTO used when updating an existing movie.
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
