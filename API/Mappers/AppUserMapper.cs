using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Mappers;

public static class AppUserMapper
{
    public static UserResponse ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserResponse
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            ImageUrl = user.ImageUrl,
            Token = tokenService.CreateToken(user)
        };
    }
}
