using System.Collections.Generic;

using ShiemiApi.Models;
using ShiemiApi.Data;

namespace ShiemiApi.Repositories
{

    public class ProjectRepository(ApplicationDbContext context)
    {

        private readonly ApplicationDbContext _context = context;

        public void Save()
            => _context.SaveChanges();

        // Create

        public void Create(Project project)
        {
            _context.Projects
            .Add(project);

            Save();
        }

        // Read

        public Project GetById(int id)
            => _context.Projects
            .Where(u => u.Id == id)
            .SingleOrDefault();

        public List<Project> GetAll()
            => _context.Projects
            .ToList();

        // Update

        public void Update(Project project)
        {
            _context.Projects
            .Update(project);

            Save();
        }

        // Delete

        public void Delete(Project project)
        {
            _context.Projects
            .Remove(project);

            Save();
        }
    }
}

