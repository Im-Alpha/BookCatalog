namespace BookCatalog.Api.Dto;

public class CreateBookDto
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public int PublicationYear { get; set; }
    public string Genre {get; set;} = default!;
}