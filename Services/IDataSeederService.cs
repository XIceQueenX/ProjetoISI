using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using Trabalho_ISI.Data;

namespace Trabalho_ISI.Services
{
    public interface IDataSeederService
    {
        Task<SeedResult> SeedAsync(int movieCount = 50, int bookCount = 50);
    }

    public class SeedResult
    {
        public int MoviesAdded { get; set; }
        public int BooksAdded { get; set; }
    }

    public class DataSeederService : IDataSeederService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        private const string TMDBImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        public DataSeederService(AppDbContext context, AppSettings settings, HttpClient httpClient)
        {
            _context = context;
            _settings = settings;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyTestApp/1.0");
        }

        public async Task<SeedResult> SeedAsync(int movieCount = 50, int bookCount = 50)
        {
            int moviesAdded = await SeedMoviesAsync(movieCount);
            int booksAdded = await SeedBooksAsync(bookCount);

            await _context.SaveChangesAsync();

            return new SeedResult
            {
                MoviesAdded = moviesAdded,
                BooksAdded = booksAdded
            };
        }

        private async Task<int> SeedMoviesAsync(int total)
        {
            var apiKey = _settings.TMDB.ApiKey;
            var baseUrl = _settings.TMDB.BaseUrl;

            int added = 0;
            int page = 1;

            while (added < total)
            {
                var url = $"{baseUrl}/movie/popular?api_key={apiKey}&language=pt-BR&page={page}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var results = json.RootElement.GetProperty("results").EnumerateArray();

                foreach (var m in results)
                {
                    int movieId = m.GetProperty("id").GetInt32();

                    if (await _context.Movies.AnyAsync(x => x.Id == movieId))
                        continue;

                    var movie = new Movie
                    {
                        Title = m.GetProperty("title").GetString(),
                        OriginalTitle = m.GetProperty("original_title").GetString(),
                        Overview = m.GetProperty("overview").GetString(),
                        ReleaseDate = m.GetProperty("release_date").GetString(),
                        PosterPath = m.GetProperty("poster_path").GetString() is string p ? TMDBImageBaseUrl + p : null,
                        BackdropPath = m.GetProperty("backdrop_path").GetString() is string b ? TMDBImageBaseUrl + b : null
                    };

                    _context.Movies.Add(movie);
                    added++;

                    if (added >= total) break;
                }

                if (!results.Any()) break;

                page++;
            }

            return added;
        }

        private async Task<int> SeedBooksAsync(int total)
        {
            var apiKey = _settings.GoogleBooks.ApiKey;
            var baseUrl = _settings.GoogleBooks.BaseUrl;
            var urlBase = $"{baseUrl}/volumes?q=subject:fiction&key={apiKey}";

            int added = 0;
            int fetched = 0;

            while (added < total)
            {
                int maxResults = Math.Min(40, total - added);
                var url = $"{urlBase}&startIndex={fetched}&maxResults={maxResults}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                if (!json.RootElement.TryGetProperty("items", out var items)) break;

                foreach (var b in items.EnumerateArray())
                {
                    var volumeInfo = b.GetProperty("volumeInfo");
                    var bookId = b.GetProperty("id").GetString();

                    if (await _context.Books.AnyAsync(x => x.Id == bookId))
                        continue;

                    var authors = volumeInfo.TryGetProperty("authors", out var a)
                        ? string.Join(", ", a.EnumerateArray().Select(x => x.GetString()))
                        : "";

                    var book = new Book
                    {
                        Id = bookId,
                        Title = volumeInfo.GetProperty("title").GetString(),
                        Subtitle = volumeInfo.TryGetProperty("subtitle", out var s) ? s.GetString() : "",
                        Authors = authors,
                        Publisher = volumeInfo.TryGetProperty("publisher", out var p) ? p.GetString() : "",
                        PublishedDate = volumeInfo.TryGetProperty("publishedDate", out var pd) ? pd.GetString() : "",
                        Description = volumeInfo.TryGetProperty("description", out var d) ? d.GetString() : ""
                    };

                    _context.Books.Add(book);
                    added++;

                    if (added >= total) break;
                }

                fetched += maxResults;
            }

            return added;
        }
    }
}
