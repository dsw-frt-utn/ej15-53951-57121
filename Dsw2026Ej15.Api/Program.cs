
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Agregar servicios al contenedor.
       

            builder.Services.AddControllers();
            // Configuracion de OpenAPI.
            //builder.Services.AddOpenApi();}
            builder.Services.AddSwaggerGen();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Se utiliza SQLite por una limitacion del entorno: la VM de desarrollo corre Windows ARM64.
            // SQL Server LocalDB depende de componentes nativos x64 y falla al cargar SQLUserInstance.dll.
            builder.Services.AddDbContext<DoctorsDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddScoped<IPersistence, PersistenceEf>();
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Configurar el pipeline HTTP.
            if (app.Environment.IsDevelopment())
            {
               //app.MapOpenApi();
               app.UseSwagger();
               app.UseSwaggerUI();

            }

            app.Use(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (ValidationException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(exception.Message);
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Ocurrio un error inesperado."
                    });
                }
            });

            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
