using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Helpers;
using RaffleApi.Services;

namespace RaffleApi.Extensions;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationsServices(this WebApplicationBuilder builder)
    {
        builder.AddDataContext();
        builder.Services.AddScoped<UnitOfWork>();
        builder.Services.AddScoped<DiscordService>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        builder.Services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Verbose,
            MessageCacheSize = 1000
        }));
    }
    
    public static void AddActionFilters(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ValidateClanOwner>();
        builder.Services.AddScoped<ValidateClanMember>();
        builder.Services.AddScoped<ValidateUser>();
        builder.Services.AddScoped<ValidateRaffleExists>();
    }
    
    private static void AddDataContext(this WebApplicationBuilder builder)
    {
        var dbProvider = builder.Configuration.GetValue<DatabaseProvider>("DatabaseProvider");
        var providerName = dbProvider switch
        {
            DatabaseProvider.AzureSQL => "DefaultConnection",
            DatabaseProvider.MySQL => "MySQLConnection",
            _ => throw new Exception("Invalid database provider")
        };
        var connStr = builder.Configuration.GetConnectionString(providerName);
        if (string.IsNullOrEmpty(connStr)) throw new Exception("Invalid connection string");
        
        builder.Services.AddDbContext<DataContext>(options => {
            switch (dbProvider)
            {
                case DatabaseProvider.AzureSQL:
                    options.UseSqlServer(connStr);
                    break;
                case DatabaseProvider.MySQL:
                    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
}