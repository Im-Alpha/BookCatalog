using BookCatalog.Api.Data; // For DbContext (if not using a service layer yet)
using BookCatalog.Api.Models; // For Book entity
using BookCatalog.Api.Dto; // For DTOs
using BookCatalog.Api.Services; // Services
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For ToListAsync, FindAsync etc.
using System;
using System.Collections.Generic;
using System.Linq; // For LINQ operations
using System.Threading.Tasks;

namespace BookCatalog.Api.Controllers
{
    [ApiController] // Indicates this is an API controller
    [Route("api/[controller]")] // Sets the base route to /api/Books
    public class BooksController : ControllerBase
    {
        // Use service layer to connect to DB
        private readonly IBookService _bookService;

        // Set up Connection variable for DB
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // --- GET Endpoints ---

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }
        

        // GET api/books/{id}
        [HttpGet("{id}")] // {id} is a route parameter
        public async Task<ActionResult<Book>> GetBooksById(int id) // Parameter comes from the route
        {
            var book = await _bookService.GetBooksByIdAsync(id); // start the search for the book by index

            // Check if book index exists
            if (book == null)
            {
                return NotFound(); // HTTP 404
            }

            // Map the entity to the DTO before returning
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Genre = book.Genre
            };

            return Ok(book); // HTTP 200
        }

        // --- POST Endpoint ---
        // POST api/books
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book book) // Parameter comes from the request BODY
        {
            var createdBook = await _bookService.AddBookAsync(book);
            // returns 201 Created
            // Location: /api/books/{id}
            return CreatedAtAction(nameof(AddBook), new { id = createdBook.Id }, createdBook);
        }

        // --- PUT Endpoint (UPDATE Book)---
        // PUT api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }
            
            // Create a variable to hold the result
            var res = await _bookService.UpdateBookAsync(updatedBook);

            if (!res)
            {
                return NotFound();
            }

            return NoContent(); // HTTP 204 - success
        }

        // --- DELETE Endpoint ---
        // DELETE api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent(); // HTTP 204
        }

        // Helper method to check if a book exists
       // private bool BookExists(Guid id)
        //{
         //   return _bookService.Books.Any(e => e.Id == id);
        //}
    }
}