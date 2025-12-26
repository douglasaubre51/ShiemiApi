namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController(
    UserRepository userRepo,
    ImageUtility imageUtil
)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly ImageUtility _imageUtil = imageUtil;

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

    // returns user using db integer id !
    [HttpGet("{id}")]
    public IResult GetUser(int id)
    {
        try
        {
            var dbUser = _userRepo.GetById(id);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnt exist!" });

            Mapper mapper = MapperUtility.Get<User, GetUserDto>();
            GetUserDto dto = mapper.Map<GetUserDto>(dbUser);
            if (dbUser.ProfilePhoto is not null)
                dto.ProfilePhotoURL = dbUser.ProfilePhoto.URL;

            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error fetching user!" });
        }
    }

    // returns user using string id !
    [HttpGet("id/{UserId}")]
    public IResult GetUserById(string UserId)
    {
        try
        {
            var dbUser = _userRepo.GetByUserId(UserId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnt exist!" });

            Mapper mapper = MapperUtility.Get<User, GetUserDto>();
            GetUserDto dto = mapper.Map<GetUserDto>(dbUser);
            if (dbUser.ProfilePhoto is not null)
                dto.ProfilePhotoURL = dbUser.ProfilePhoto.URL;

            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new
            {
                Message = "error fetching user using string id !"
            });
        }
    }

    // returns user's db integer id !
    [HttpGet("{userId}/id")]
    public IResult GetUserId(string userId)
    {
        try
        {
            var user = _userRepo.GetByUserId(userId);
            if (user is null)
                return Results.BadRequest("user doesn't exist!");

            return Results.Ok(new { Id = user.Id });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
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

    [HttpPut]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IResult> UpdateUser(
        [FromForm] string id,
        [FromForm] string firstName,
        [FromForm] string lastName,
        [FromForm] IFormFile profilePhoto
    )
    {
        try
        {
            User dbUser = _userRepo.GetById(int.Parse(id))!;
            if (dbUser is null)
                return Results.BadRequest(new { Message = "user doesnot exists!" });

            UploadResult result = _imageUtil.UploadImage(profilePhoto);
            if (result is null)
                return Results.BadRequest(new { Message = "failed to upload profile photo!" });

            dbUser.FirstName = firstName;
            dbUser.LastName = lastName;
            if (dbUser.ProfilePhoto is null)
                dbUser.ProfilePhoto = new()
                {
                    PublicId = result.PublicId,
                    URL = result.Url.ToString()
                };

            else
            {
                dbUser.ProfilePhoto.PublicId = result.PublicId;
                dbUser.ProfilePhoto.URL = result.Url.ToString();
            }

            _userRepo.Update(dbUser);
            return Results.Ok(new
            {
                Message = "user updated successfully!"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error updating user!" });
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