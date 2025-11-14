using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

/// <summary>
/// Account controller.
/// </summary>
public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    /// <summary>
    /// Creates an User.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A newly created account.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST api/account/register
    ///     {
    ///        "displayName": "displayName",
    ///        "email": "test@test.com",
    ///        "password": "password"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
    {
        if (await EmailExists(request.Email)) return BadRequest("Email is already in use");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    /// <summary>
    /// Login as an User.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Valid user token.</returns>
    /// <response code="200">Returns the valid user token</response>
    /// <response code="401">If the username or password are incorrect</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user == null) return Unauthorized("Invalid email or password");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid email or password");
        }

        return user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
