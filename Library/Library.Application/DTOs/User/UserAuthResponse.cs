namespace Library.Application.DTOs.User
{
    public record UserAuthResponse(
        string AccessToken,
        string RefreshToken
    );
}
