using System;

namespace BookCatalog.Api.Models;

public class Book
{
    // Using 'Id' so that it works with ef core
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public string Genre { get; set; }
}