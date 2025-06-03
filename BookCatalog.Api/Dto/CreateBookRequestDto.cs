using System.ComponentModel.DataAnnotations; // For [Required], [StringLength]

namespace BookCatalog.Api.Dto
{
    public class CreateBookRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(50, ErrorMessage = "Author name cannot exceed 50 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, ErrorMessage = "ISBN must be 13 or 10 characters long.", MinimumLength = 10)] // Standard ISBN lengths
        public string ISBN { get; set; }

        [Range(1000, 9999, ErrorMessage = "Publication year must be a valid 4-digit year.")]
        public int PublicationYear { get; set; }

        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters.")]
        public string Genre { get; set; }
    }
}