using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using API.Data;
using API.Interfaces;
using API.Middlewares;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;

namespace API;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks();
        AddServiceDefaults(builder);

        builder.Services.AddControllers()
            .AddMvcOptions(options =>
            {
                // Add the filter we could have
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddMemoryCache();

        AddDbContext(builder);
        AddScopedServices(builder);

        builder.Services.AddOpenApiDocument(options =>
        {
            options.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dating API",
                    Description = "An ASP.NET Core Web API for managing Dating items",
                    TermsOfService = "https://example.com/terms",
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = "https://example.com/contact"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = "https://example.com/license"
                    }
                };
            };
        });

        WebApplication app = builder.Build();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            Task.Run(() => Seed.SeedUsers(context));
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger>();
            logger.LogError(ex, "Migration process failed!");
        }

        // Configure the HTTP request pipeline.
        app.UseMiddleware<ExceptionMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(x => x.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            ));

            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi();

            app.UseReDoc(options =>
            {
                options.Path = "/redoc";
            });
        }
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void AddServiceDefaults(WebApplicationBuilder builder)
    {
        builder.Services.AddCors();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenKey = builder.Configuration["TokenKey"]
                    ?? throw new ArgumentNullException("Cannot get the token key - Program.cs");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
        });
    }

    private static void AddScopedServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IMembersRepository, MembersRepository>();
    }
}
