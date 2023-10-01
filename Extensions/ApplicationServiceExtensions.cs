using Microsoft.EntityFrameworkCore;
using RaffleApi.ActionFilters;
using RaffleApi.Data;
using RaffleApi.Helpers;

namespace RaffleApi.Extensions;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationsServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UnitOfWork>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        
        builder.Services.AddDbContext<DataContext>(options => {
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connStr);
        });

        return;
    }
    
    public static void AddActionFilters(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ValidateClanOwner>();
        builder.Services.AddScoped<ValidateClanMember>();
        builder.Services.AddScoped<ValidateUser>();
        builder.Services.AddScoped<ValidateRaffle>();
        return;
    }
}