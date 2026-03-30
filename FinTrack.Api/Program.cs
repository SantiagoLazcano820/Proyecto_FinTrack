
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Services;
using FinTrack.Services.Validators;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<FinTrackContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            builder.Services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
                );

            //builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddTransient<CrearCategoryDtoValidator>();
            builder.Services.AddTransient<ActualizarCategoryDtoValidator>();
            builder.Services.AddTransient<CrearTransactionDtoValidator>();
            builder.Services.AddTransient<ActualizarTransactionDtoValidator>();
            builder.Services.AddTransient<ITransactionService, TransactionService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
