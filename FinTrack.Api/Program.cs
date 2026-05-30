
using FinTrack.Api.Middleware;
using FinTrack.Core.Interfaces;
using FinTrack.Core.Services;
using FinTrack.Infraestructure.Data;
using FinTrack.Infraestructure.Repositories;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Services;
using FinTrack.Services.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            // --- REPOSITORIES ---
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            // --- SERVICES ---
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            // --- AUTOMAPPER ---
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // --- UNITOFWORK ---
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            // fábrica
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            // contexto
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            //Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend FinTrack API",
                    Version = "v1",
                    Description = "Documentación de la API de FinTrack .NET 10",
                    Contact = new()
                    {
                        Name = "Equipo de desarrollo UCB",
                        Email = "desarrollo@ucb.edu.bo"
                    }
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();

                // Definición de seguridad para el botón Authorize de Swagger
                options.DocumentFilter<FinTrack.Api.Filters.BearerSecurityDocumentFilter>();
                options.OperationFilter<FinTrack.Api.Filters.AuthorizeCheckOperationFilter>();
            });

            //Configurar JWT
            builder.Services.AddAuthentication(options =>
            {
                /*Esquema por defecto para autenticar (identificar quién es el usuario).
                 * DefaultAuthenticateScheme → Esquema por defecto para autenticar (identificar quién es el usuario).
                 * JwtBearerDefaults.AuthenticationScheme → Equivale al string "Bearer".
                 * Significado: "Cuando llegue una petición, usa JWT Bearer para identificar al usuario"
                 * */
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                /*DefaultChallengeScheme → Esquema por defecto cuando se desafía al usuario (por ejemplo, 
                 * si intenta acceder a algo sin estar autenticado).
                 * Si no hay token o es inválido, el sistema responderá con un desafío típico de JWT (401 Unauthorized)               
                 * */
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    //TokenValidationParameters → Objeto que contiene todas las reglas de validación para un token JWT.
                    new TokenValidationParameters
                    {
                        //Valida el emisor (iss) → Verifica que el token haya sido emitido por un servidor de confianza. Evita que alguien use tokens creados por otro sistema.
                        ValidateIssuer = true,
                        //Verifica que el token esté dirigido a esta API. Evita que un token para otro servicio(ej: un frontend) sea aceptado aquí.
                        ValidateAudience = true,
                        //Comprueba que el token no haya expirado y que su "no válido antes de" sea correcto. Es la razón por la que los tokens dejan de funcionar automáticamente después de X tiempo.
                        ValidateLifetime = true,
                        //Verifica que el token no haya sido modificado. Usa la clave secreta para asegurar que el token es genuino y no manipulado.
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        /*SymmetricSecurityKey → Clave simétrica (misma clave para firmar y verificar).
                         * Encoding.UTF8.GetBytes(...) → Convierte la clave(string) en bytes.
                         * builder.Configuration["Authentication:SecretKey"] → Lee la clave desde appsettings.json(ej: "miSuperSecretoLargo123!").
                        */
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                builder.Configuration["Authentication:SecretKey"]
                            )
                        )
                    };
            });

            builder.Services.Configure<PasswordOptions>(builder.Configuration.GetSection("PasswordOptions"));

            // Registrar Servicios de Aplicación
            builder.Services.AddTransient<ISecurityService, SecurityService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            //Usar Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend FinTrack API v1");
                    options.RoutePrefix = string.Empty; //Swagger sera accesible en la raíz
                });
            }

            // ---MIDDLEWARE ---
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
