using System;

namespace BookCatalog.Api.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; } // Client needs the ID to refer to this book
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
    }
}