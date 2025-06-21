using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RaffleApi.Data;

namespace API.Migrations.MySQL;

public class MySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("MySQLConnection");

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseMySql(
            connectionString ?? throw new InvalidOperationException("MySQL connection string not found."),
            ServerVersion.AutoDetect(connectionString),
            mysql => mysql.MigrationsAssembly("API.Migrations.MySQL")
        );

        return new DataContext(optionsBuilder.Options);
    }
}
