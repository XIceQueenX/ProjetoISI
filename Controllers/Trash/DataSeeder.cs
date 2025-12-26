using Microsoft.AspNetCore.Mvc;

namespace Trabalho_ISI.Controllers.Trash
{
    using Microsoft.EntityFrameworkCore;
    using System.Net.Http;
    using System.Text.Json;
    /*
        public class DataSeeder
        {
            private readonly AppDbContext _context;
            private readonly HttpClient _httpClient;

            public DataSeeder(AppDbContext context, HttpClient httpClient)
            {
                _context = context;
                _httpClient = httpClient;
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyTestApp/1.0");
            }
            public async Task SeedAsync()
            {
                Console.WriteLine("Fetching TMDB movies...");
                var moviesData = await FetchTMDBMovies(50);
                var movies = moviesData.Select(m => new Movie
                {
                    Id = m.GetProperty("id").GetInt32(),
                    Title = m.GetProperty("title").GetString(),
                    OriginalTitle = m.GetProperty("original_title").GetString(),
                    Overview = m.GetProperty("overview").GetString(),
                    ReleaseDate = m.GetProperty("release_date").GetString(),
                    PosterPath = m.GetProperty("poster_path").GetString(),
                    BackdropPath = m.GetProperty("backdrop_path").GetString()
                }).ToList();

                foreach (var movie in movies)
                {
                    if (!_context.Movies.Any(x => x.Id == movie.Id))
                        _context.Movies.Add(movie);
                }

                Console.WriteLine("Fetching Google Books...");
                var booksData = await FetchGoogleBooks(50);
                var books = booksData.Select(b => {
                    var volumeInfo = b.GetProperty("volumeInfo");
                    var authors = volumeInfo.TryGetProperty("authors", out var a)
                        ? string.Join(", ", a.EnumerateArray().Select(x => x.GetString()))
                        : "";
                    return new Book
                    {
                        Id = b.GetProperty("id").GetString(),
                        Title = volumeInfo.GetProperty("title").GetString(),
                        Subtitle = volumeInfo.TryGetProperty("subtitle", out var s) ? s.GetString() : "",
                        Authors = authors,
                        Publisher = volumeInfo.TryGetProperty("publisher", out var p) ? p.GetString() : "",
                        PublishedDate = volumeInfo.TryGetProperty("publishedDate", out var pd) ? pd.GetString() : "",
                        Description = volumeInfo.TryGetProperty("description", out var d) ? d.GetString() : ""
                    };
                }).ToList();

                foreach (var book in books)
                {
                    if (!_context.Books.Any(x => x.Id == book.Id))
                        _context.Books.Add(book);
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Saved 50 movies and 50 books into the database!");
            }

            private async Task<JsonElement[]> FetchTMDBMovies(int total = 50)
            {

                var urlBase = "https://api.themoviedb.org/3/movie/popular?api_key=a2a1d2e30fd76a6543814e665c3a80dc&language=pt-BR&page=";
                var movies = new List<JsonElement>();
                int page = 1;

                while (movies.Count < total)
                {
                    var url = urlBase + page;
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    var results = json.RootElement.GetProperty("results").EnumerateArray();
                    foreach (var movie in results)
                    {
                        movies.Add(movie);
                        if (movies.Count == total) break;
                    }
                    page++;
                }

                return movies.ToArray();
            }

            private async Task<JsonElement[]> FetchGoogleBooks(int total = 50)
            {
                var baseUrl = "https://www.googleapis.com/books/v1/volumes?q=subject:fiction&key=AIzaSyCFyRHSadMrLQIB5fZHuFSQMEOOTDTLpME";
                var books = new List<JsonElement>();
                int fetched = 0;

                while (books.Count < total)
                {
                    int maxResults = Math.Min(40, total - books.Count);
                    int startIndex = fetched;
                    var url = $"{baseUrl}&startIndex={startIndex}&maxResults={maxResults}";

                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                    if (!json.RootElement.TryGetProperty("items", out var items)) break;

                    foreach (var book in items.EnumerateArray())
                    {
                        books.Add(book);
                        if (books.Count == total) break;
                    }

                    fetched += maxResults;
                }

                return books.ToArray();
            }
        }
        */
}
