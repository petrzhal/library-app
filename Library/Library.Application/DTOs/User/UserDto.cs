﻿namespace Library.Application.DTOs.User
{
    public record UserDto(
        int Id,
        string Username,
        string Email,
        string Role
    );
}
