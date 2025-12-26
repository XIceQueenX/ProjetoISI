    using global::Trabalho_ISI.Data;

    namespace Trabalho_ISI.Services.Interfaces
    {
        public interface IBookService
        {
            Task<PaginatedResult<Book>> GetAllBooksAsync(int page, int pageSize, string? search);
            Task<Book?> GetBookByIdAsync(string id);
            Task<Book> CreateBookAsync(BookCreateDto dto);
            Task<Book?> UpdateBookAsync(string id, BookUpdateDto dto);
            Task<bool> DeleteBookAsync(string id);
            Task<List<Book>> GetBooksByAuthorAsync(string author);
            Task<int> GetTotalBooksCountAsync();
        }

        public class PaginatedResult<T>
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public List<T> Data { get; set; }
        }
    }
