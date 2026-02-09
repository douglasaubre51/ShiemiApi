namespace ShiemiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevController(
    UserRepository userRepo,
    DevRepository devRepo,
    PhotoRepository photoRepo,
    ImageUtility imageUtil
)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly DevRepository _devRepo = devRepo;
    private readonly PhotoRepository _photoRepo = photoRepo;
    private readonly ImageUtility _imageUtil = imageUtil;

    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IResult> Add(
        [FromForm] string id,
        [FromForm] string shortDesc,
        [FromForm] string description,
        [FromForm] string startingPrice,
        [FromForm] IFormFile advertPhoto
    )
    {
        try
        {
            int userId = int.Parse(id);
            User dbUser = _userRepo.GetById(userId)!;
            if (dbUser is null)
                return Results.BadRequest(new
                {
                    Message = "user doesnt exist!"
                });

            UploadResult result = _imageUtil.UploadImage(advertPhoto);

            Dev newDev = new()
            {
                UserId = userId,
                User = dbUser,
                ShortDesc = shortDesc,
                Description = description,
                StartingPrice = decimal.Parse(startingPrice),
            };
            _devRepo.Add(newDev);

            Photo newAdvert = new()
            {
                Dev = _userRepo.GetById(userId)!.Dev,
                PublicId = result.PublicId,
                URL = result.Url.ToString()
            };
            _photoRepo.Add(newAdvert);

            dbUser.IsDeveloper = true;
            _userRepo.Save();

            return Results.Ok(new
            {
                Message = "new developer created!"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new
            {
                Message = "error creating new developer"
            });
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
                return Results.BadRequest(new
                {
                    Message = "user doesnot exist!"
                });

            dbUser.IsDeveloper = false;
            _userRepo.Save();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new
            {
                Message = "Error resetting devMode on user !"
            });
        }
    }
    [HttpGet("{userId}/userId/dev")]
    public IResult GetByUserId(int userId)
    {
        try
        {
            var dev = _devRepo.GetQueryable()
                .SingleOrDefault(d => d.UserId == userId);
            if (dev is null)
                return Results.BadRequest("dev doesn't exist!");

            return Results.Ok(dev);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
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

            DevDto dto = new DevDto() {
                Id = dev.Id,
                UserId = dev.UserId,
                Advert = dev.Advert.URL,
                ShortDesc = dev.ShortDesc,
                Username = dev.User.FirstName +" "+dev.User.LastName
            };

            return Results.Ok(new { Dev = dto });
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
                if (dbDevs[i].User!.ProfilePhoto is not null)
                    devs[i].Profile = dbDevs[i].User!.ProfilePhoto!.URL;
            }

            return Results.Ok(devs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.BadRequest(new { Message = ex.Message });
        }
    }
}
