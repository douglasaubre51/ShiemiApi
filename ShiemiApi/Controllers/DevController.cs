namespace ShiemiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevController(
    UserRepository userRepo,
    DevRepository devRepo
)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly DevRepository _devRepo = devRepo;

    [HttpPost]
    public IResult Add(DevDto dto)
    {
        try
        {
            User dbUser = _userRepo.GetById(dto.UserId)!;
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnt exist!" });

            Dev newDev = new()
            {
                UserId = dto.UserId,
                User = dbUser,
                ShortDesc = dto.ShortDesc,
                StartingPrice = dto.StartingPrice
            };
            _devRepo.Add(newDev);

            return Results.Ok(new { Message = "new developer created!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error creating new developer" });
        }
    }

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
    [HttpGet("{devId}")]
    public IResult GetById(int devId)
    {
        try
        {
            var dev = _devRepo.GetById(devId);
            if (dev is null)
                return Results.BadRequest("dev doesn't exist!");

            return Results.Ok(new { Dev = dev });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }
    [HttpGet("all")]
    public IResult GetAll()
    {
        try
        {
            List<Dev> dbDevs = [.. _devRepo.GetAll()];
            if (dbDevs.Count < 0)
                return Results.BadRequest(new { Message = "empty list!" });

            Mapper mapper = MapperUtility.Get<Dev, DevDto>();
            List<DevDto> devs = mapper.Map<List<DevDto>>(dbDevs);
            for (int i = 0; i < devs.Count; i++)
            {
                devs[i].Username = dbDevs[i].User!.FirstName + " " + dbDevs[i].User!.LastName;
                devs[i].Profile = dbDevs[i].User!.Profile;
            }

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