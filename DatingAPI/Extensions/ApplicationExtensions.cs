using DatingAPI.Data;
using DatingAPI.Interfaces;
using DatingAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAPI.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddScoped<ITokerService, TokenService>();
            services.AddDbContext<DataContext>
                (options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                });
            return services;
        }
    }
}
