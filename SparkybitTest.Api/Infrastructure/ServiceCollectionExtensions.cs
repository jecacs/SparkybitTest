using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SparkybitTest.Adapters.MongoDb;
using SparkybitTest.Adapters.MongoDb.Repositories;
using SparkybitTest.Api.Infrastructure.Settings;
using SparkybitTest.Api.Services;
using SparkybitTest.Domain.Repositories;
using SparkybitTest.Domain.Services;
using SparkybitTest.RabbitMq.Configs;
using SparkybitTest.RabbitMq.Services;

namespace SparkybitTest.Api.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(_ =>
            new RabbitMqConfiguration
            {
                EasyNetQConnectionString = builder.Configuration["RabbitMqConfiguration:EasyNetQConnectionString"]
            });

        builder.Services.AddSingleton(_ =>
            new JwtSettings
            {
                Audience = builder.Configuration["Jwt:Audience"],
                Key = builder.Configuration["Jwt:Key"],
                Issuer = builder.Configuration["Jwt:Issuer"]
            });
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer,",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.",
            });
    
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    public static void AddJwt(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(option => 
            { 
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
    }

    public static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<JwtService>();
        builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
        builder.Services.AddSingleton<IUserService, UserService>();

        // mongoDb
        builder.Services.AddSingleton<MongoDbConnectionFactory>();
        builder.Services.AddSingleton<IMongoDatabase>(ctx =>
        {
            var mongoFactory = ctx.GetRequiredService<MongoDbConnectionFactory>();

            return mongoFactory.GetDatabase(builder.Configuration["MongoDb:ConnectionString"]);
        });
        builder.Services.AddTransient<IUserRepository, MongoDbUserRepository>();
    }
}