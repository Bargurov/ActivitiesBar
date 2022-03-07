using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Appliction.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Appliction.Activities;
using Appliction.Interfaces;
using Infrastructure.Security;
using Infrastructure.Photos;
using Appliction.Photos;

namespace API.Extensions
{
    public static class ApplictionServiceExtensions
    {
        public static IServiceCollection AddApplictionServices(this IServiceCollection services,IConfiguration config)
        {
             services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
            services.AddDbContext<DataContext>(opt=>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",policy =>{
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });
             services.AddMediatR(typeof(List.Handler).Assembly);

             services.AddAutoMapper(typeof(MappingProfiles).Assembly);
             services.AddScoped<IUsernameAccess,UserAccess>();
             services.AddScoped<IPhotoAccessor,PhotoAccessor>();
             services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

             return services;
        }
    }
}