using Microsoft.AspNetCore.Mvc;

namespace ShiemiApi.Controllers{

	[ApiController]
	[Route("/api/[controller]")]
	public class UserController{
		
		[HttpGet("/{Id}")] 
		public IResult GetUser(int Id){
			try{
				
				return Results.Ok();
			}
			catch(Exception ex){
				Console.WriteLine("GetUser error: "+ex.Message);
				return Results.BadRequest();
			}
		}
	}
}
