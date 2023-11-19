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
        builder.Services.AddScoped<UnitOfWork>();
        builder.Services.AddScoped<DiscordService>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        builder.Services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Verbose,
            MessageCacheSize = 1000
        }));
        
        builder.Services.AddDbContext<DataContext>(options => {
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connStr);
        });
    }
    
    public static void AddActionFilters(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ValidateClanOwner>();
        builder.Services.AddScoped<ValidateClanMember>();
        builder.Services.AddScoped<ValidateUser>();
        builder.Services.AddScoped<ValidateRaffle>();
    }
}