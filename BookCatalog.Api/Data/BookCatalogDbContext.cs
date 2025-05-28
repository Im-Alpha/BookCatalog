using BookCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Api.Data;

// Inherits from db context
public class BookCatalogDbContext  : DbContext
{
    DbSet<Book> Books { get; set; }
}