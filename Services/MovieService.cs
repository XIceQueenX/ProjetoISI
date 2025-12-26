using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;
using Trabalho_ISI.Services.Interfaces.Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Movie>> GetAllMoviesAsync(int page, int pageSize, string? search)
        {
            var query = _context.Movies.AsQueryable();

            // Aplicar filtro de pesquisa
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

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> CreateMovieAsync(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                OriginalTitle = dto.OriginalTitle ?? dto.Title,
                Overview = dto.Overview,
                ReleaseDate = dto.ReleaseDate,
                PosterPath = dto.PosterPath,
                BackdropPath = dto.BackdropPath
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie?> UpdateMovieAsync(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return null;

            movie.Title = dto.Title ?? movie.Title;
            movie.OriginalTitle = dto.OriginalTitle ?? movie.OriginalTitle;
            movie.Overview = dto.Overview ?? movie.Overview;
            movie.ReleaseDate = dto.ReleaseDate ?? movie.ReleaseDate;
            movie.PosterPath = dto.PosterPath ?? movie.PosterPath;
            movie.BackdropPath = dto.BackdropPath ?? movie.BackdropPath;

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Movie>> GetRecentMoviesAsync(int count)
        {
            return await _context.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetMoviesByYearAsync(string year)
        {
            return await _context.Movies
                .Where(m => m.ReleaseDate.StartsWith(year))
                .ToListAsync();
        }

        public async Task<int> GetTotalMoviesCountAsync()
        {
            return await _context.Movies.CountAsync();
        }
    }

    // DTOs
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