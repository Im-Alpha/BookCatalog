namespace BookCatalog.Api.Models;

public class Book
{
    int Id { get; init; }
    string Title { get; init; }
    string Author { get; init; }
    string ISBN { get; init; }
    string PublicationYear { get; init; }
    string Genre { get; init; }
}