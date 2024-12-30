namespace Library.Application.DTOs.Authors
{
    public record AuthorDto(
        int Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string Country
    );
}
