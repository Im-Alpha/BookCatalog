namespace BookCatalog.Api.Dto;

public class BookPatchDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? PublicationYear { get; set; }
    public string? Genre { get; set; }
}
