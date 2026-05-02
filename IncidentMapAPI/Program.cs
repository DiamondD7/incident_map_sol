
using Azure.Identity;
using IncidentMapAPI.Application.Interfaces.Repositories;
using IncidentMapAPI.Infrastructure.Persistence;
using IncidentMapAPI.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IncidentMapAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("https://hotspotsnz.com") // Allow your frontend's URL
                              .AllowCredentials()  // Allow credentials (cookies, authorization headers)
                              .AllowAnyHeader()    // Allow any headers
                              .AllowAnyMethod();   // Allow any HTTP method
                    });
            });


            var keyVaultUri = new Uri("https://hotspots-kv.vault.azure.net/");

            builder.Configuration.AddAzureKeyVault(
                keyVaultUri,
                new DefaultAzureCredential()
            );

            var connString = builder.Configuration["DbConnectionString"];


            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connString));

            builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
            builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
