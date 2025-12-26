using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    /// <summary>
    /// Service responsible for CRUD operations and queries related to books.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all books with optional search and pagination.
        /// </summary>
        public async Task<PaginatedResult<Book>> GetAllBooksAsync(int page, int pageSize, string? search)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b =>
                    b.Title.Contains(search) ||
                    b.Authors.Contains(search) ||
                    b.Description.Contains(search));
            }

            var total = await query.CountAsync();
            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Book>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Data = books
            };
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// Returns null if not found.
        /// </summary>
        public async Task<Book?> GetBookByIdAsync(string id)
        {
            return await _context.Books.FindAsync(id);
        }

        /// <summary>
        /// Creates a new book record.
        /// </summary>
        public async Task<Book> CreateBookAsync(BookCreateDto dto)
        {
            var book = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Subtitle = dto.Subtitle,
                Authors = dto.Authors,
                Publisher = dto.Publisher,
                PublishedDate = dto.PublishedDate,
                Description = dto.Description
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        /// <summary>
        /// Updates an existing book by ID.
        /// Returns null if book not found.
        /// </summary>
        public async Task<Book?> UpdateBookAsync(string id, BookUpdateDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            // Update only provided fields
            book.Title = dto.Title ?? book.Title;
            book.Subtitle = dto.Subtitle ?? book.Subtitle;
            book.Authors = dto.Authors ?? book.Authors;
            book.Publisher = dto.Publisher ?? book.Publisher;
            book.PublishedDate = dto.PublishedDate ?? book.PublishedDate;
            book.Description = dto.Description ?? book.Description;

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return book;
        }

        /// <summary>
        /// Deletes a book by ID.
        /// Returns true if deleted successfully.
        /// </summary>
        public async Task<bool> DeleteBookAsync(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves books by author name.
        /// </summary>
        public async Task<List<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _context.Books
                .Where(b => b.Authors.Contains(author))
                .ToListAsync();
        }

        /// <summary>
        /// Returns the total number of books.
        /// </summary>
        public async Task<int> GetTotalBooksCountAsync()
        {
            return await _context.Books.CountAsync();
        }
    }

    /// <summary>
    /// DTO for creating a book.
    /// </summary>
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string Authors { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for updating a book.
    /// All fields are optional.
    /// </summary>
    public class BookUpdateDto
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Authors { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public string? Description { get; set; }
    }
}
