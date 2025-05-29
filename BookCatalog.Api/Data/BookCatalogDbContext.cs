using BookCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Api.Data
{
    // Inherits from db context
    public class BookCatalogDbContext  : DbContext
    {
        public BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options) : base(options)
        {
        }
        DbSet<Book> Books { get; set; }
    }  
}

