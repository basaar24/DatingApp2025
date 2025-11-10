using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.UnitTests;

[SetUpFixture]
public class GlobalTestSetup
{
    public static AppDbContext AppDbContext { get; private set; }

    [OneTimeSetUp]
    public async Task Setup()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data source=dating.db")
            .Options;

        AppDbContext = new AppDbContext(options);
        await AppDbContext.Database.MigrateAsync();
        await Seed.SeedUsers(AppDbContext);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await AppDbContext.DisposeAsync();
    }
}
