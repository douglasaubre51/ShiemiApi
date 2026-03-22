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

    // un Ban user by userId !
    [HttpGet("{userId}/un-ban")]
    public IResult UnBanUser(int userId)
    {
        try
        {
            var dbUser = _userRepo.GetById(userId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "User does'nt exists!" });

            dbUser.IsBanned = false;
            _userRepo.Save();

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    // Ban user by userId !
    [HttpGet("{userId}/ban")]
    public IResult BanUser(int userId)
    {
        try
        {
            var dbUser = _userRepo.GetById(userId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "User does'nt exists!" });

            dbUser.IsBanned = true;
            _userRepo.Save();

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

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

    // Get all past projects !
    [HttpGet("{id}/past-projects")]
    public IResult GetPastProjects(int id)
    {
        try
        {
            var dbPastProjects = _userRepo.GetQueryable()
                .SingleOrDefault(user => user.Id == id)!
                .PastProjects;
            if (dbPastProjects is null || dbPastProjects.Count is 0)
                return Results.BadRequest(new { Message = "Empty list !" });

            return Results.Ok(dbPastProjects);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    // Gets the optional user details by integer userId!
    [HttpGet("{userId}/optional-details")]
    public IResult GetOptionalDetails(int userId)
    {
        try
        {
            var dbUser = _userRepo.GetById(userId);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "User doesnt exist!" });

            Mapper mapper = MapperUtility.Get<User, OptionalUserDetailsDto>();
            OptionalUserDetailsDto dto = mapper.Map<OptionalUserDetailsDto>(dbUser);

            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "Error fetching optional details!" });
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

    [HttpGet("all")]
    public IResult GetAllNew()
    {
        try
        {
            var dbUsers = _userRepo.GetAll();
            if (dbUsers.Count is 0)
                return Results.BadRequest(new { Message = "empty list" });

            List<GetUserDto> getUserDtos = [];

            foreach (var user in dbUsers)
            {
                GetUserDto dto = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.UserId,
                    Email = user.Email,
                    ProfilePhotoURL = user.ProfilePhoto?.URL ?? string.Empty,
                    IsDeveloper = user.IsDeveloper
                };

                getUserDtos.Add(dto);
            }

            return Results.Ok(getUserDtos);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    // Update user details!
    [HttpPut("user-details")]
    public async Task<IResult> UpdateUser(UpdateUserWrapper userWrapper)
    {
        try
        {
            UserDto userDto = userWrapper.UserDto;
            OptionalUserDetailsDto optionalDetails = userWrapper.OptionalUserDetailsDto;

            User? dbUser = _userRepo.GetById(userDto.Id);
            if (dbUser is null)
                return Results.BadRequest(new { Message = "User doesnot exists!" });

            dbUser.FirstName = userDto.FirstName;
            dbUser.LastName = userDto.LastName;

            dbUser.AboutMe = optionalDetails.AboutMe;
            dbUser.Contact = optionalDetails.Contact;
            dbUser.Whatsaap = optionalDetails.Whatsaap;
            dbUser.Gmail = optionalDetails.Gmail;
            dbUser.LinkedIn = optionalDetails.LinkedIn;
            dbUser.Github = optionalDetails.Github;

            Console.WriteLine(optionalDetails.AboutMe);

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
    [HttpPut("user-profile-photo")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IResult> UpdateUserProfilePhoto(
        [FromForm] string id,
        [FromForm] IFormFile profilePhoto
    )
    {
        try
        {
            User dbUser = _userRepo.GetById(int.Parse(id))!;
            if (dbUser is null)
                return Results.BadRequest(new { Message = "User doesnot exists!" });

            UploadResult result = _imageUtil.UploadImage(profilePhoto);
            if (result is null)
                return Results.BadRequest(new { Message = "Failed to upload profile photo!" });

            if (dbUser.ProfilePhoto is null)
                dbUser.ProfilePhoto = new()
                {
                    PublicId = result.PublicId,
                    URL = result.SecureUrl.ToString()
                };
            else
            {
                dbUser.ProfilePhoto.PublicId = result.PublicId;
                dbUser.ProfilePhoto.URL = result.SecureUrl.ToString();
            }
            _userRepo.Update(dbUser);

            return Results.Ok(new
            {
                Message = "User Profile Photo updated successfully!"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "Error updating User Profile Photo!" });
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
