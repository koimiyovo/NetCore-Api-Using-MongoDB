using BooksAPI.Models;
using BooksAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET /api/books
        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get() => await _bookService.Get();

        // GET /api/books/XX
        [HttpGet("{id:length(24)}", Name = nameof(GetBook))]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            var book = await _bookService.Get(id);

            if (book == null) return NotFound();

            return book;
        }

        // POST /api/books
        [HttpPost]
        public async Task<ActionResult<Book>> Create([FromBody] Book book)
        {
            await _bookService.Create(book);

            return CreatedAtRoute(nameof(GetBook), new { id = book.Id.ToString() }, book);
        }

        // PUT /api/books
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Book bookIn)
        {
            var book = await _bookService.Get(id);

            if (book == null) return NotFound();

            await _bookService.Update(id, bookIn);

            return NoContent();
        }

        // DELETE /api/books/XX
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.Get(id);

            if (book == null) return NotFound();

            await _bookService.Remove(book);

            return NoContent();
        }
    }
}
