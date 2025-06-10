using AutoMapper;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Dto;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();
        
        // Mapping for patch and checks for non-null properties in BookPatchDto
        CreateMap<Book, BookPatchDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}