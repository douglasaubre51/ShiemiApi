using System.Diagnostics;
using ShiemiApi.Utility;

namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController(
    UserRepository userRepo,
    MapperUtility mapper
)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly MapperUtility _mapper = mapper;

    [HttpPost]
    public IResult CreateUser(CreateUserDto dto)
    {
        try
        {
            var dbUser = _userRepo.GetAll().SingleOrDefault(u => u.UserId == dto.Id);
            if (dbUser is not null)
                return Results.Ok();

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
            Debug.WriteLine(ex.Message);
            return Results.BadRequest(ex);
        }
    }

    [HttpGet("{id}")]
    public IResult GetUser(int id)
    {
        try
        {
            var dbUser = _userRepo.GetById(id);
            return dbUser is null ? Results.BadRequest() : Results.Ok(dbUser);
        }
        catch (Exception ex) { return Results.BadRequest(ex); }
    }

    [HttpGet("id/{UserId}")]
    public IResult GetUserById(string UserId)
    {
        try
        {
            var dbUser = _userRepo.GetByUserId(UserId);
            if (dbUser is null)
                return Results.BadRequest();

            var mapper = _mapper.Get<User, UserDto>();
            UserDto dto = mapper.Map<UserDto>(dbUser);
            return Results.Ok(dto);
        }
        catch (Exception ex) { return Results.BadRequest(ex); }
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
        catch (Exception ex) { return Results.BadRequest(ex); }
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

    [HttpPut]
    public IResult UpdateUser(User user)
    {
        try
        {
            _userRepo.Update(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

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
            return Results.BadRequest(ex);
        }
    }
}