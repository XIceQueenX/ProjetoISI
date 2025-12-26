using Microsoft.EntityFrameworkCore;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Book>> GetAllBooksAsync(int page, int pageSize, string? search)
        {
            var query = _context.Books.AsQueryable();

            // Aplicar filtro de pesquisa se fornecido
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

        public async Task<Book?> GetBookByIdAsync(string id)
        {
            return await _context.Books.FindAsync(id);
        }

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

        public async Task<Book?> UpdateBookAsync(string id, BookUpdateDto dto)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return null;

            // Atualizar apenas os campos fornecidos
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

        public async Task<bool> DeleteBookAsync(string id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _context.Books
                .Where(b => b.Authors.Contains(author))
                .ToListAsync();
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            return await _context.Books.CountAsync();
        }
    }

    // DTOs
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string Authors { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public string? Description { get; set; }
    }

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