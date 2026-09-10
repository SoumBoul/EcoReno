using EcoReno.Data;
using EcoReno.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoReno.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects.OrderBy(p => p.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
        {
            return NotFound(new { error = $"Projet introuvable avec l'id {id}" });
        }
        return project;
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Project>> UpdateProject(int id, Project updatedProject)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
        {
            return NotFound(new { error = $"Projet introuvable avec l'id {id}" });
        }

        project.Name = updatedProject.Name;
        project.Owner = updatedProject.Owner;
        project.Budget = updatedProject.Budget;
        project.Description = updatedProject.Description;
        project.Impact = updatedProject.Impact;
        project.Status = updatedProject.Status;

        await _context.SaveChangesAsync();
        return project;
    }
}
