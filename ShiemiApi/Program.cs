using DotNetEnv;
using ShiemiApi.Storage.HubStorage;
using ShiemiApi.Utility;

var builder = WebApplication.CreateBuilder(args);

// Load Environment variables.
Env.Load();

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// Configure Database pipeline.
var conn_string = Environment.GetEnvironmentVariable("SHIEMI_DB_STRING");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(conn_string, ServerVersion.AutoDetect(conn_string))
);

// Add repositories 
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<ChannelRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<DevRepository>();

// Add storage 
builder.Services.AddSingleton<UserStorageService>();
builder.Services.AddSingleton<UserStorage>();
builder.Services.AddSingleton<ProjectStorage>();

// Add services
// scoped
builder.Services.AddScoped<RoomService>();


var app = builder.Build();


// Add SignalR Endpoints
app.MapHub<RoomHub>("/hubs/room");
app.MapHub<ChannelHub>("/hubs/channel");
app.MapHub<ProjectHub>("/hubs/project");

app.MapControllers();

app.MapGet("/", () => Results.Ok("Shiemi says hello!"));

// Create Client Account via WAGURI
app.MapPost("/api/user/create",
    (
        [FromServices] UserRepository _userRepo,
        [FromBody] UserDto dto
    ) =>
    {
        try
        {
            var result = _userRepo.Create(dto);
            return result == true ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateUser error: {ex.Message}");
            return Results.InternalServerError();
        }
    });

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();