
using FinTrack.Api.Middleware;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Services;
using FinTrack.Services.Validators;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

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
                ).ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            ;

            //builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            // --- VALIDATORS (Transient está bien para estos) ---
            builder.Services.AddTransient<CrearCategoryDtoValidator>();
            builder.Services.AddTransient<ActualizarCategoryDtoValidator>();
            builder.Services.AddTransient<CrearTransactionDtoValidator>();
            builder.Services.AddTransient<ActualizarTransactionDtoValidator>();
            builder.Services.AddTransient<CrearUserDtoValidator>();
            builder.Services.AddTransient<ActualizarUserDtoValidator>();
            builder.Services.AddTransient<LoginUserDtoValidator>();

            // --- REPOSITORIES ---
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // --- SERVICES ---
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // --- AUTOMAPPER ---
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // --- UNITOFWORK ---
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            // fábrica
            builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

            // contexto
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // ---MIDDLEWARE ---
            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
