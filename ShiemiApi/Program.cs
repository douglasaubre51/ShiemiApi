var builder = WebApplication.CreateBuilder(args);

// Load Environment variables.

DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddControllers();

// Add Swagger.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Configure Database pipeline.

var conn_string = Environment.GetEnvironmentVariable("SHIEMI_DB_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(conn_string, ServerVersion.AutoDetect(conn_string))
);

builder.Services.AddSignalR();

// Add service to DI container

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<ProjectRepository>();

builder.Services.AddSingleton<UserStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add SignalR Endpoints

app.MapHub<MessageHub>("/hubs/message-hub");

app.MapControllers();

app.MapGet("/", () => Results.Ok("Shiemi says hello!"));

// Use Swagger

app.UseSwagger();

app.UseSwaggerUI();

app.Run();
