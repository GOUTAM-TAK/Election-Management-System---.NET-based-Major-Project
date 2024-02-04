
using ElectionMamagementApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectionMamagementApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ElectionManagementDataBaseContext>(options =>
                       options.UseSqlServer(builder.Configuration.GetConnectionString("ElectionManagementDataBaseContext")));

            //Add AutoMapper Service 
            builder.Services.AddAutoMapper(typeof(Program));


            //enable cross origin
            builder.Services.AddCors(c=>c.AddDefaultPolicy(dfipolicy=>dfipolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}