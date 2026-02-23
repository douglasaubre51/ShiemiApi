namespace ShiemiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController(
    ReviewRepository reviewRepo,
    UserRepository userRepo,
    ProjectRepository projectRepo
) : ControllerBase
{
    private readonly ReviewRepository _reviewRepo = reviewRepo;
    private readonly UserRepository _userRepo = userRepo;
    private readonly ProjectRepository _projectRepo = projectRepo;

    [HttpGet("{projectId}/all")]
    public IResult GetAllByProject(int projectId)
    {
        try
        {
            List<Review> dbReviews = _reviewRepo.GetAllByProjectId(projectId);
            if (dbReviews.Count == 0)
                return Results.BadRequest(new { Message = "empty list !" });

            Mapper mapper = MapperUtility.Get<Review, ReviewDtos>();
            List<ReviewDtos> reviewDtos = mapper.Map<List<ReviewDtos>>(dbReviews);
            for (int i = 0; i < dbReviews.Count; i++)
            {
                reviewDtos[i].UserId = dbReviews[i].User!.Id;
                reviewDtos[i].UserName = dbReviews[i].User!.FirstName + " " + dbReviews[i].User!.LastName;
                reviewDtos[i].ProjectId = dbReviews[i].Project!.Id;
            }

            return Results.Ok(reviewDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Review: GetAllByProject: error: {ex.Message}");
            return Results.InternalServerError();
        }
    }

    // TODO :
    // Add a review count calculator for ADMIN !

    [HttpGet("project/{projectId}/review-count")]
    public IResult GetProjectReviewCountByProjectId(int projectId)
    {
        try
        {
//            int reviewCount = _reviewRepo.GetAll()
//                .Where(project => project.Id == projectId)
//                .ToList()
//                .Count();
              
                
            var reviews = _reviewRepo.GetAll()
                .Where(project => project.Id == projectId)
                .ToList();

            return Results.Ok();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    [HttpGet("all")]
    public IResult GetAll()
    {
        try
        {
            List<Review> dbReviews = _reviewRepo.GetAll();
            if (dbReviews.Count == 0)
                return Results.BadRequest(new { Message = "empty list !" });

            Mapper mapper = MapperUtility.Get<Review, ReviewDtos>();
            List<ReviewDtos> reviewDtos = mapper.Map<List<ReviewDtos>>(dbReviews);
            for (int i = 0; i < dbReviews.Count; i++)
            {
                reviewDtos[i].UserId = dbReviews[i].User!.Id;
                reviewDtos[i].UserName = dbReviews[i].User!.FirstName + " " + dbReviews[i].User!.LastName;
                reviewDtos[i].ProjectId = dbReviews[i].Project!.Id;
            }

            return Results.Ok(new { ReviewDtos = reviewDtos });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Review: GetAll: error: {ex.Message}");
            return Results.InternalServerError();
        }
    }

    [HttpPost]
    public IResult Create(CreateReviewDto dto)
    {
        try
        {
            User? dbUser = _userRepo.GetById(dto.UserId);
            Project? dbProject = _projectRepo.GetById(dto.ProjectId);
            Review newReview = new()
            {
                User = dbUser,
                Project = dbProject,
                Text = dto.Text,
                CreatedAt = dto.CreatedAt
            };
            _reviewRepo.Create(newReview);

            return Results.Ok(new { Message = "Created new Review !" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Review: Create: error: {ex.Message}");
            return Results.InternalServerError();
        }
    }
}
