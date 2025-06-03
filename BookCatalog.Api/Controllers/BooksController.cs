using BookCatalog.Api.Data; // For DbContext (if not using a service layer yet)
using BookCatalog.Api.Models; // For Book entity
using BookCatalog.Api.Dto; // For DTOs
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
        private readonly BookCatalogDbContext _context; // Use a service layer here later!

        // Constructor for Dependency Injection
        public BooksController(BookCatalogDbContext context)
        {
            _context = context;
        }

        // --- GET Endpoints ---
        // GET api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            // In a real app, you'd map these to BookDto using AutoMapper or manually
            // For now, a simple select to BookDto
            // Select is part of a LINQ(Language Integrated Query)
            // 
            var bookDtos = books.Select(b
                => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                PublicationYear = b.PublicationYear,
                Genre = b.Genre
            }).ToList();

            return Ok(bookDtos);
        }

        // GET api/books/{id}
        [HttpGet("{id}")] // {id} is a route parameter
        public async Task<ActionResult<BookDto>> GetBook(Guid id) // Parameter comes from the route
        {
            var book = await _context.Books.FindAsync(id); // start the search for the book by index

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

            return Ok(bookDto); // HTTP 200
        }

        // --- POST Endpoint ---
        // POST api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookRequestDto request) // Parameter comes from the request BODY
        {
            // IMPORTANT: [FromBody] tells ASP.NET Core to deserialize the JSON request body
            // into an instance of CreateBookRequestDto.

            // Validate the incoming request DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns HTTP 400 with validation errors
            }

            // Map the DTO to your actual Book entity
            var book = new Book
            {
                Id = Guid.NewGuid(), // Generate a new ID here or let the DB do it
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                PublicationYear = request.PublicationYear,
                Genre = request.Genre
            };

            _context.Books.Add(book); // Add the entity to the DbContext
            await _context.SaveChangesAsync(); // Commit changes to the database

            // Map the created entity back to a BookDto for the response
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Genre = book.Genre
            };

            // Return 201 Created and the location of the new resource
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        // --- PUT Endpoint ---
        // PUT api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookDto bookDto) // Using BookDto for simplicity here
        {
            if (id != bookDto.Id)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Update entity properties from DTO
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.ISBN = bookDto.ISBN;
            book.PublicationYear = bookDto.PublicationYear;
            book.Genre = bookDto.Genre;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Re-throw if it's not a NotFound issue
                }
            }

            return NoContent(); // HTTP 204
        }

        // --- DELETE Endpoint ---
        // DELETE api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent(); // HTTP 204
        }

        // Helper method to check if a book exists
        private bool BookExists(Guid id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}