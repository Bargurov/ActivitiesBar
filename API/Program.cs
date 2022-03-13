using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FluentValidation.AspNetCore;
using API.Extensions;
using Microsoft.Extensions.DependencyInjection;

// var builder = WebApplication.CreateBuilder(args);

// // add services to container 

// builder.Services.AddControllers(opt =>
// {
//     var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//     opt.Filters.Add(new AuthorizeFilter(policy));
// })
//     .AddFluentValidation(config=>
//     {
//         config.RegisterValidatorsFromAssemblyContaining<Appliction.Activities.Create>();
//     });
// builder.Services.AddApplictionServices(builder.Configuration);
// builder.Services.AddIdentityServices(builder.Configuration);


// //configure the http request pipline 

// var app = builder.Build();

//     if (env.IsDevelopment())
//             {
//                 app.UseDeveloperExceptionPage();
//                 app.UseSwagger();
//                 app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
//             }

//             // app.UseHttpsRedirection();

//             app.UseRouting();

//             app.UseDefaultFiles();
//             app.UseStaticFiles();


//             app.UseCors("CorsPolicy");

//             app.UseAuthentication();

//             app.UseAuthorization();

//             app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllers();
//                 endpoints.MapHub<ChatHub>("/chat");
//                 endpoints.MapFallbackToController("Index","Fallback");
//             });





namespace API
{
    public class Program
    {        public static async Task  Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior",true);
             var host = CreateHostBuilder(args).Build();
             using var scope = host.Services.CreateScope();
             var services = scope.ServiceProvider;

             try
             {
                 var context = services.GetRequiredService<DataContext>();
                 var userManager =services.GetRequiredService<UserManager<AppUser>>();
                 await context.Database.MigrateAsync();
                 await Seed.SeedData(context,userManager);
             }
             catch(Exception ex)
             {
                 var logger = services.GetRequiredService<ILogger<Program>>();
                 logger.LogError(ex,"An error occured during migration");
             }
              await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
