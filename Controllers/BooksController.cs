using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trabalho_ISI.Services;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Controllers
{
    /// <summary>
    /// Controller responsible for managing book-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Service that contains the business logic for books
        private readonly IBookService _bookService;

        /// <summary>
        /// Constructor with dependency injection of the book service.
        /// </summary>
        /// <param name="bookService">Book service</param>
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Retrieves a paginated list of all books (PUBLIC).
        /// Supports optional search by title or description.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetBooks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _bookService.GetAllBooksAsync(page, pageSize, search);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific book by its ID (PUBLIC).
        /// </summary>
        /// <param name="id">Book identifier</param>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBook(string id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        /// <summary>
        /// Creates a new book (ADMIN ONLY).
        /// </summary>
        /// <param name="dto">Book creation data</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateBook(BookCreateDto dto)
        {
            var book = await _bookService.CreateBookAsync(dto);

            // Returns HTTP 201 with the location of the created resource
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Updates an existing book (ADMIN ONLY).
        /// </summary>
        /// <param name="id">Book identifier</param>
        /// <param name="dto">Updated book data</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBook(string id, BookUpdateDto dto)
        {
            var book = await _bookService.UpdateBookAsync(id, dto);

            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        /// <summary>
        /// Deletes a book by its ID (ADMIN ONLY).
        /// </summary>
        /// <param name="id">Book identifier</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(string id)
        {
            var success = await _bookService.DeleteBookAsync(id);

            if (!success)
                return NotFound(new { message = "Book not found" });

            return Ok(new { message = "Book deleted successfully" });
        }

        /// <summary>
        /// Retrieves books written by a specific author (PUBLIC).
        /// </summary>
        /// <param name="author">Author name</param>
        [HttpGet("by-author/{author}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBooksByAuthor(string author)
        {
            var books = await _bookService.GetBooksByAuthorAsync(author);
            return Ok(books);
        }

        /// <summary>
        /// Retrieves basic book statistics (PUBLIC).
        /// </summary>
        [HttpGet("stats")]
        [AllowAnonymous]
        public async Task<ActionResult> GetStats()
        {
            var total = await _bookService.GetTotalBooksCountAsync();
            return Ok(new { totalBooks = total });
        }
    }
}
