namespace ShiemiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController(
        ProjectRepository projectRepo
        )
    {
        private readonly ProjectRepository _projectRepo = projectRepo;

        [HttpGet("{projectId}/{userId}")]
        public IResult AddToProject(int projectId, int userId)
        {
            try
            {
                Console.WriteLine($"projectId: {projectId}");
                Console.WriteLine($"userId: {userId}");

                Project? dbProject = _projectRepo.GetById(projectId);
                if (dbProject is null)
                    return Results.BadRequest(new { Message = "Project not found !" });

                dbProject.UserList.Add(userId);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddToProject error: " + ex.Message);
                return Results.InternalServerError();
            }
        }
    }
}
