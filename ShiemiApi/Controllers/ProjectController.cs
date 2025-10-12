namespace ShiemiApi.Controllers;


[ApiController]
[Route("/api/[controller]")]
public class ProjectController(ProjectRepository projectRepo)
{

    private readonly ProjectRepository _projectRepo = projectRepo;

    // Create

    [HttpPost]
    public IResult CreateProject(ProjectDto dto)
    {
	try
	{
	    var project = new Project
	    {
		Title = dto.Title,
		      Description = dto.Description,
		      ShortDesc = dto.ShortDesc,
		      UserId = dto.UserId
	    };
	    _projectRepo.Create(project);

	    return Results.Ok();
	}
	catch (Exception ex)
	{
	    Console.WriteLine("CreateProject error: " + ex.Message);
	    return Results.BadRequest();
	}
    }

    // Read

    [HttpGet("all")]
    public IResult GetAll()
    {
	try
	{
	    var dbProjects = _projectRepo.GetAll();
	    if (dbProjects is null)
		return Results.NotFound();

	    return Results.Ok(dbProjects);
	}
	catch (Exception ex)
	{
	    Console.WriteLine("GetAllProjects error: " + ex.Message);
	    return Results.BadRequest();
	}
    }

    [HttpGet("all/{UserId}")]
    public IResult GetAllByUserId(int UserId)
    {
	try
	{
	    Console.WriteLine($"user id: {UserId}");
	    var dbProjects = _projectRepo.GetAllByUserId(UserId);
	    if (dbProjects is null)
		return Results.NotFound();

	    return Results.Ok( new { Projects = dbProjects } );
	}
	catch (Exception ex)
	{
	    Console.WriteLine("GetAllProjectByUserId error: " + ex.Message);
	    return Results.BadRequest();
	}
    }

    [HttpGet("{ProjectId}")]
    public IResult GetById(int ProjectId)
    {
	try
	{
	    var dbProject = _projectRepo.GetById(ProjectId);
	    if (dbProject is null)
		return Results.NotFound();

	    return Results.Ok(dbProject);
	}
	catch (Exception ex)
	{
	    Console.WriteLine("GetProjectById error: " + ex.Message);
	    return Results.BadRequest();
	}
    }

    // Update

    [HttpPut("{Id}")]
    public IResult UpdateProject(int Id, Project project)
    {
	try
	{
	    _projectRepo.Update(project);
	    return Results.Ok();
	}
	catch (Exception ex)
	{
	    Console.WriteLine("UpdateProject error: " + ex.Message);
	    return Results.BadRequest();
	}
    }

    // Remove

    [HttpDelete("{Id}")]
    public IResult RemoveProject(int Id)
    {
	try
	{
	    _projectRepo.Remove(Id);
	    return Results.Ok();
	}
	catch (Exception ex)
	{
	    Console.WriteLine("RemoveProject error: " + ex.Message);
	    return Results.BadRequest();
	}
    }
}
