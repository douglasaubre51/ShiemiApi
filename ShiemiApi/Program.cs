using Microsoft.EntityFrameworkCore;

using ShiemiApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Load Environment variables.

DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddControllers();

// Configure Database pipeline.

var conn_string = Environment.GetEnvironmentVariable("SHIEMI_DB_STRING");

builder.Services.AddDbContext<ApplicationDbContext>( options =>
	options.UseMySql(conn_string,ServerVersion.AutoDetect(conn_string))
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.MapGet("/",() => Results.Ok("Shiemi says hello!"));

app.Run();
