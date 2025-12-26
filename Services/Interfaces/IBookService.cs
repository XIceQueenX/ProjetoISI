using global::Trabalho_ISI.Data;

namespace Trabalho_ISI.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing books.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves a paginated list of books, optionally filtered by a search string.
        /// </summary>
        Task<PaginatedResult<Book>> GetAllBooksAsync(int page, int pageSize, string? search);

        /// <summary>
        /// Retrieves a single book by its ID.
        /// </summary>
        Task<Book?> GetBookByIdAsync(string id);

        /// <summary>
        /// Creates a new book.
        /// </summary>
        Task<Book> CreateBookAsync(BookCreateDto dto);

        /// <summary>
        /// Updates an existing book by ID.
        /// Returns null if the book does not exist.
        /// </summary>
        Task<Book?> UpdateBookAsync(string id, BookUpdateDto dto);

        /// <summary>
        /// Deletes a book by ID.
        /// Returns true if deleted successfully, false otherwise.
        /// </summary>
        Task<bool> DeleteBookAsync(string id);

        /// <summary>
        /// Retrieves all books by a specific author.
        /// </summary>
        Task<List<Book>> GetBooksByAuthorAsync(string author);

        /// <summary>
        /// Returns the total number of books.
        /// </summary>
        Task<int> GetTotalBooksCountAsync();
    }

    /// <summary>
    /// Generic paginated result for returning lists of data.
    /// </summary>
    /// <typeparam name="T">Type of items in the result.</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Total number of items across all pages.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// List of items for the current page.
        /// </summary>
        public List<T> Data { get; set; }
    }
}
