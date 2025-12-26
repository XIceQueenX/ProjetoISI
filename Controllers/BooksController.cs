using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trabalho_ISI.Services;
using Trabalho_ISI.Services.Interfaces;

namespace Trabalho_ISI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Obter todos os livros (PÚBLICO)
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
        /// Obter um livro específico (PÚBLICO)
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBook(string id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound(new { message = "Livro não encontrado" });

            return Ok(book);
        }

        /// <summary>
        /// Criar novo livro (APENAS ADMIN) 🔒
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateBook(BookCreateDto dto)
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Atualizar livro (APENAS ADMIN) 🔒
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBook(string id, BookUpdateDto dto)
        {
            var book = await _bookService.UpdateBookAsync(id, dto);

            if (book == null)
                return NotFound(new { message = "Livro não encontrado" });

            return Ok(book);
        }

        /// <summary>
        /// Eliminar livro (APENAS ADMIN) 🔒
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(string id)
        {
            var success = await _bookService.DeleteBookAsync(id);

            if (!success)
                return NotFound(new { message = "Livro não encontrado" });

            return Ok(new { message = "Livro eliminado com sucesso" });
        }

        /// <summary>
        /// Pesquisar livros por autor (PÚBLICO)
        /// </summary>
        [HttpGet("by-author/{author}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBooksByAuthor(string author)
        {
            var books = await _bookService.GetBooksByAuthorAsync(author);
            return Ok(books);
        }

        /// <summary>
        /// Estatísticas de livros (PÚBLICO)
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