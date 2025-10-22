namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController(UserRepository userRepo)
{
    private readonly UserRepository _userRepo = userRepo;

    // CREATE
    [HttpPost]
    public IResult CreateUser(CreateUserDto dto)
    {
        try
        {
            User user = new()
            {
                UserId = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };
            _userRepo.Create(user);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("CreateUser error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    // READ
    [HttpGet("{Id}")]
    public IResult GetUser(int Id)
    {
        try
        {
            var dbUser = _userRepo.GetById(Id);
            return dbUser is null ? Results.NotFound() : Results.Ok(dbUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetUser error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpGet("id/{UserId}")]
    public IResult GetUserById(string UserId)
    {
        try
        {
            var dbUser = _userRepo.GetByUserId(UserId);
            if (dbUser is null)
                return Results.BadRequest();

            CreateUserDto dto = new()
            {
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                Email = dbUser.Email,
                Id = dbUser.UserId
            };

            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetUser error: " + ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{userId}/id")]
    public IResult GetUserId(string userId)
    {
        try
        {
            var user = _userRepo.GetByUserId(userId);
            if (user is null)
                return Results.BadRequest("user doesn't exist!");

            return Results.Ok(new { user.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetUserId error: {ex.Message}");
            return Results.InternalServerError();
        }
    }

    [HttpGet("/all")]
    public IResult GetAll()
    {
        try
        {
            return Results.Ok(_userRepo.GetAll());
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    // UPDATE
    [HttpPut("{Id}")]
    public IResult UpdateUser(int Id, User user)
    {
        try
        {
            _userRepo.Update(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("UpdateUser error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    // DELETE
    [HttpDelete("{Id}")]
    public IResult RemoveUser(int Id)
    {
        try
        {
            _userRepo.Remove(Id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("RemoveUser error: " + ex.Message);
            return Results.BadRequest();
        }
    }
}