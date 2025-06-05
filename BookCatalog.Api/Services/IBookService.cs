using BookCatalog.Api.Models;
using Npgsql;
using BookCatalog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Api.Services;

// Create a DTO(Data Transfer Object) to pass between the client and API
public class IBookService
{
    private readonly BookCatalogDbContext _dbContext;

    public IBookService(BookCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // Get all books
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _dbContext.Books.ToListAsync();
    }

    // Get specific book based on id
    public async Task<Book?> GetBooksByIdAsync(int id) // Important to not use Task<List<Book>> because we only want 1 
    {
        return await _dbContext.Books.FindAsync(id);
    }

    // Add a new book
    public async Task<Book> AddBookAsync(Book book)
    {
        // Add book
        await _dbContext.Books.AddAsync(book);
        // Save
        await _dbContext.SaveChangesAsync();
        // Return the book added
        return book;
    }

    // Modify an existing book
    public async Task<bool>UpdateBookAsync(Book updatedBook)
    {
        // Create variable to avoid modifying directly
        var existingBook = await _dbContext.Books.FindAsync(updatedBook.Id);
        
        // Check if the book exists
        if (existingBook == null)
        {
            return false; // Book not found
        }
        
        // Update properties via 1 line
        _dbContext.Entry(existingBook).CurrentValues.SetValues(updatedBook);
        
        // Wait for the changes to be saved and complete the op
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Delete a book from the library(DB)
    public async Task<bool> DeleteBookAsync(int id)
    {
        // Find if the book exists
        var existingBook = await _dbContext.Books.FindAsync(id);
        if (existingBook == null)
        {
            return false;
        }
        
        // Remove the book
        _dbContext.Books.Remove(existingBook);
        // Save changes
        await _dbContext.SaveChangesAsync();
        return true;
    }
}