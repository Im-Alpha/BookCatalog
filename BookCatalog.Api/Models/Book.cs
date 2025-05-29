using System;

namespace BookCatalog.Api.Models;

public class Book
{
    // Using 'Id' so that it works with ef core
    public Guid Id { get; set; }

    public string Title { get; init; }
    public string Author { get; init; }
    public string ISBN { get; init; }
    public string PublicationYear { get; init; }
    public string Genre { get; init; }
}