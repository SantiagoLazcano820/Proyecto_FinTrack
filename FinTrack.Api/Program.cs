using Microsoft.EntityFrameworkCore;
using FinTrack.Infraestructure.Mappings;
using FinTrack.Core.Interfaces;
using FinTrack.Services;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;
using FinTrack.Core.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace FinTrack.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configuración BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<FinTrackContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddTransient<CrearCategoryValidator>();
            builder.Services.AddTransient<ActualizarCategoryValidator>();
            builder.Services.AddTransient<CrearUserValidator>();
            builder.Services.AddTransient<ActualizarUserValidator>();
            builder.Services.AddTransient<CrearTransactionValidator>();
            builder.Services.AddTransient<ActualizarTransactionValidator>();

            builder.Services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
                );

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
