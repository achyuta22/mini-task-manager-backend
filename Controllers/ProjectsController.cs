using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTOs;
using System.Linq;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/projects
        [HttpGet]
        public IActionResult GetUserProjects()
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var projects = _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    Tasks = p.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        DueDate = t.DueDate,
                        IsCompleted = t.IsCompleted
                    }).ToList()
                })
                .ToList();

            return Ok(projects);
        }

        // POST api/projects
        [HttpPost]
        public IActionResult AddProject([FromBody] Project project)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            project.UserId = userId;
            _context.Projects.Add(project);
            _context.SaveChanges();

            return Ok(project);
        }

        // POST api/projects/{projectId}/tasks
        [HttpPost("{projectId}/tasks")]
        public IActionResult AddTask(int projectId, [FromBody] TaskItem task)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId && p.UserId == userId);
            if (project == null) return NotFound();

            task.ProjectId = projectId;
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return Ok(task);
        }

        // PUT api/projects/{projectId}/tasks/{taskId}/toggle
        [HttpPut("{projectId}/tasks/{taskId}/toggle")]
        public IActionResult ToggleTaskCompletion(int projectId, int taskId)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == taskId && t.ProjectId == projectId && t.Project!.UserId == userId);

            if (task == null) return NotFound();

            task.IsCompleted = !task.IsCompleted;
            _context.SaveChanges();

            return Ok(task);
        }

        // DELETE api/projects/{projectId}/tasks/{taskId}
        [HttpDelete("{projectId}/tasks/{taskId}")]
        public IActionResult DeleteTask(int projectId, int taskId)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == taskId && t.ProjectId == projectId && t.Project!.UserId == userId);

            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}
