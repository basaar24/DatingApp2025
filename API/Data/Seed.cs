using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var seedUsersData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var seedUsers = JsonSerializer.Deserialize<List<SeedUserDto>>(seedUsersData);

        if (seedUsers == null)
        {
            Console.WriteLine("No seed data available");
            return;
        }

        foreach (var seedUser in seedUsers)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Id = seedUser.Id,
                Email = seedUser.Email,
                DisplayName = seedUser.DisplayName,
                ImageUrl = seedUser.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = seedUser.Id,
                    DisplayName = seedUser.DisplayName,
                    Gender = seedUser.Gender,
                    City = seedUser.City,
                    Country = seedUser.Country,
                    Description = seedUser.Description,
                    BirthDay = seedUser.BirthDay,
                    ImageUrl = seedUser.ImageUrl,
                    LastActive = seedUser.LastActive,
                    Created = seedUser.Created
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = seedUser.ImageUrl!,
                MemberId = seedUser.Id
            });

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
