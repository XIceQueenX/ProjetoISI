using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;

namespace Trabalho_ISI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter todos os filmes (com paginação)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var query = _context.Movies.AsQueryable();

            // Filtrar por título se fornecido
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
        /// Obter um filme específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound(new { message = "Filme não encontrado" });
            }

            return Ok(movie);
        }

        /// <summary>
        /// Criar um novo filme
        /// </summary>
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
        /// Atualizar um filme existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(int id, MovieUpdateDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound(new { message = "Filme não encontrado" });
            }

            // Atualizar campos
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
        /// Eliminar um filme
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound(new { message = "Filme não encontrado" });
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Filme eliminado com sucesso" });
        }

        /// <summary>
        /// Pesquisar filmes por ano
        /// </summary>
        [HttpGet("by-year/{year}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByYear(string year)
        {
            var movies = await _context.Movies
                .Where(m => m.ReleaseDate.StartsWith(year))
                .ToListAsync();

            return Ok(movies);
        }

        /// <summary>
        /// Obter filmes mais recentes
        /// </summary>
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
        /// Atualizar parcialmente um filme (PATCH)
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchMovie(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound(new { message = "Filme não encontrado" });

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

    // DTOs para Create e Update
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
    }

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