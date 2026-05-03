using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ToDo.Application.Interfaces.Identity;
using ToDo.Infrastructure.Data.Contexts;
using ToDo.Infrastructure.Data.Identity.Entities;
using ToDo.Infrastructure.Data.Identity.Services;

namespace ToDo.Infrastructure.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ToDoDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ToDoConnectionString"));
            });

            services.AddIdentity<ToDoUser, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            })
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ToDoUser, IdentityRole<int>>>()
                .AddEntityFrameworkStores<ToDoDBContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });

            services.AddAuthorization(options =>
            {

            });

            // Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
