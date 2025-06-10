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
using AutoMapper;

namespace BookCatalog.Api.Controllers
{
    [ApiController] // Indicates this is an API controller
    [Route("api/[controller]")] // Sets the base route to /api/Books
    public class BooksController : ControllerBase
    {
        // Use service layer to connect to DB
        private readonly IBookService _bookService;
        // Set up Dto to abstract book data
        private readonly IMapper _mapper;

        // Set up Connection variable for DB
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        // Set up Mapper variable
        public BooksController(IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<Book>> GetBooksById(Guid id) // Parameter comes from the route
        {
            var book = await _bookService.GetBooksByIdAsync(id); // start the search for the book by index

            // Check if book index exists
            if (book == null)
            {
                return NotFound(); // HTTP 404
            }
            
            // Set up dto to map values and return Dto object
            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto); // HTTP 200
        }

        
        // --- POST Endpoint ---
        // POST api/books
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] CreateBookDto createDto) // Parameter comes from the request BODY
        {
            // Creat mapping
            var book = _mapper.Map<Book>(createDto);
            // Wait for data
            await _bookService.AddBookAsync(book);
            
            // returns 201 Created
            // Location: /api/books/{id}
            
            // Get result 
            var resDto = _mapper.Map<BookDto>(book);
            // Return result
            return CreatedAtAction(nameof(AddBook), new { Guid = book.Id }, resDto);
        }


        // --- PUT Endpoint (UPDATE Book)---
        // PUT api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookUpdateDto updateDto)
        {
            // Search for book
            var book = await _bookService.GetBooksByIdAsync(id);
            // Check if book exists
            if (book == null)
            {
                return NotFound();
            }
            // Create Dto mapping
            _mapper.Map(updateDto, book);
            // Create a variable to hold the result
            await _bookService.SaveChangesAsync();

            // Return Success
            return NoContent(); // HTTP 204 
        }
        
        
        // -- PATCH Endpoint for updating without all of the information --
        // PATCH api/books/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBook(Guid id, [FromBody] BookPatchDto patchDto)
        {
            var book = await _bookService.GetBooksByIdAsync(id);
            if (book == null) return NotFound();

            // Apply only provided fields
            // if (patch.Title != null) book.Title = patch.Title;
            // if (patch.Author != null) book.Author = patch.Author;
            // if (patch.PublicationYear.HasValue) book.PublicationYear = patch.PublicationYear.Value;
            // if (patch.Genre != null) book.Genre = patch.Genre;
            
            // Can remove all lines above with Dto AutoMapper and consolidate into single line
            _mapper.Map(patchDto, book);

            await _bookService.SaveChangesAsync(); // or _dbContext.SaveChangesAsync()

            return NoContent();
        }



        // --- DELETE Endpoint ---
        // NOTE: For a different API a soft delete might be better for non-sensitive data and audit logs
        // DELETE api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent(); // HTTP 204
        }
        
        // Add IsDeleted field to DB
        // public async Task<bool> SoftDeleteBookAsync(Guid id)
        // {
        //     var book = await _dbContext.Books.FindAsync(id);
        //     if (book == null || book.IsDeleted) return false;
        //
        //     book.IsDeleted = true;
        //     await _dbContext.SaveChangesAsync();
        //     return true;
        // }


        // Helper method to check if a book exists
       // private bool BookExists(Guid id)
        //{
         //   return _bookService.Books.Any(e => e.Id == id);
        //}
    }
}