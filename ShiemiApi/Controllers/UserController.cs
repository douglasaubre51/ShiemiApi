namespace ShiemiApi.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class UserController(UserRepository userRepo)
    {

        private readonly UserRepository _userRepo = userRepo;

        [HttpPost("/create")]
        public IResult Create(User user)
        {
            try
            {
                _userRepo.Create(user);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateUser error: " + ex.Message);
                return Results.BadRequest();
            }
        }

        [HttpGet("/get/{Id}")]
        public IResult Get(int Id)
        {
            try
            {
                var dbUser = _userRepo.GetById(Id);
                if (dbUser is null)
                    return Results.NotFound();

                return Results.Ok(dbUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUser error: " + ex.Message);
                return Results.BadRequest();
            }
        }

        [HttpPut("/update/{Id}")]
        public IResult Update(int Id,User user)
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

        [HttpDelete("/remove/{Id}")]
        public IResult Update(int Id)
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
}
