using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLibrary.Common;
using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryService _library;
        private readonly ILogger _logger;

        public BooksController(ILibraryService library, ILogger<BooksController> logger)
        {
            _library = library ?? throw new ArgumentNullException(nameof(library));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Book>>> Get([FromQuery]int limit = 20)
        {
            if (limit < 1)
            {
                return BadRequest();
            }

            var books = await _library.FindAllAsync(limit);
            _logger.LogInformation(
                ApplicationEvents.LibraryQueried,
                "Retrieved {Count} books.",
                books.Count);

            return books;
        }

        // GET api/books/5
        [HttpGet("{id:objectid}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _library.FindAsync(id);
            if (book is null)
            {
                _logger.LogInformation(
                    ApplicationEvents.BookNotFound,
                    "Failed to retrieve book '{Id}'.",
                    id);

                return NotFound();
            }

            _logger.LogInformation(
                ApplicationEvents.BookQueried,
                "Retrieved book '{Id}'.",
                book.Id);

            return book;
        }

        // POST api/books
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            var newBook = await _library.AddAsync(book);
            _logger.LogInformation(
                ApplicationEvents.BookCreated,
                "Added book with id '{Id}'.",
                newBook.Id);

            return CreatedAtRoute("GetById", new { id = newBook.Id }, newBook);
        }

        // PUT api/books/5
        [HttpPut("{id:objectid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> Put(string id, [FromBody]Book book)
        {
            var updatedBook = await _library.UpdateAsync(id, book);
            if (updatedBook is null)
            {
                _logger.LogInformation(
                    ApplicationEvents.BookNotFound,
                    "Failed to update book '{Id}'.",
                    id);

                return NotFound();
            }

            _logger.LogInformation(
                ApplicationEvents.BookUpdated,
                "Updated book with id '{Id}'.",
                id);

            return updatedBook;
        }

        // DELETE api/books/5
        [HttpDelete("{id:objectid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            var removedBook = await _library.RemoveAsync(id);
            if (removedBook is null)
            {
                _logger.LogInformation(
                    ApplicationEvents.BookNotFound,
                    "Failed to delete book '{Id}'.",
                    id);

                return NotFound();
            }

            _logger.LogInformation(
                ApplicationEvents.BookDeleted,
                "Removed book with id '{Id}'.",
                id);

            return NoContent();
        }
    }
}
