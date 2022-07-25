using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Api.Infrastructure.Persistence.Context;
using BlazorDictionary.Api.Infrastructure.Persistence.Repositories;
using BlazorDictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Api.Infrastructure.Persistence.Extensions;
public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlazorDictionaryContext>(conf =>
        {
            var connStr = configuration["BlazorSozlukDbConnectionString"].ToString();
            conf.UseSqlServer(connStr, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        });

        //var seedData = new SeedData();

        //seedData.SeedAsync(configuration).GetAwaiter().GetResult();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();


        return services;
    }
}