using Microsoft.EntityFrameworkCore;

using ShiemiApi.Models;

namespace ShiemiApi.Data{

	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
		: DbContext(options){

			public DbSet<User> Users { get; set; }

			public DbSet<Project> Projects { get; set; }

		}
}
