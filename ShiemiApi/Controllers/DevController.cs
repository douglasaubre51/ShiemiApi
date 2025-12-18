namespace ShiemiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevController(
    UserRepository userRepo
)
{
    private readonly UserRepository _userRepo = userRepo;

    [HttpGet("{userId}/set-dev")]
    public IResult SetDeveloper(int userId)
    {
        try
        {
            User? dbUser = _userRepo.GetById(userId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnot exist!" });

            dbUser.IsDeveloper = true;
            _userRepo.Save();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "Error setting devMode on user !" });
        }
    }
    [HttpGet("{userId}/reset-dev")]
    public IResult ResetDeveloper(int userId)
    {
        try
        {
            User? dbUser = _userRepo.GetById(userId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnot exist!" });

            dbUser.IsDeveloper = false;
            _userRepo.Save();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "Error resetting devMode on user !" });
        }
    }

    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        try
        {
            var dbUser = _userRepo.GetById(id);
            return dbUser is null ? Results.BadRequest() : Results.Ok(dbUser);
        }
        catch (Exception ex) { return Results.BadRequest(ex); }
    }
    [HttpGet("{userId}/id")]
    public IResult GetById(string userId)
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
    [HttpGet("all")]
    public IResult GetAll()
    {
        try
        {
            List<User> dbUsers = _userRepo.GetQueryable()
                .Where(u => u.IsDeveloper == true)
                .ToList();
            if (dbUsers.Count() < 0)
                return Results.BadRequest(new { Message = "empty list!" });

            Mapper mapper = MapperUtility.Get<User, DevUserDto>();
            List<DevUserDto> devs = mapper.Map<List<DevUserDto>>(dbUsers);
            return Results.Ok(devs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut]
    public IResult Update(User user)
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
    public IResult Remove(int Id)
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