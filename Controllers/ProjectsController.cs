using EcoReno.Data;
using EcoReno.Models;
using EcoReno.Models.Dtos;
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
    public async Task<ActionResult<Project>> CreateProject(ProjectWriteDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Owner = dto.Owner,
            Budget = dto.Budget,
            Description = dto.Description,
            Impact = dto.Impact,
            Status = dto.Status,
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Project>> UpdateProject(int id, ProjectWriteDto dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
        {
            return NotFound(new { error = $"Projet introuvable avec l'id {id}" });
        }

        project.Name = dto.Name;
        project.Owner = dto.Owner;
        project.Budget = dto.Budget;
        project.Description = dto.Description;
        project.Impact = dto.Impact;
        project.Status = dto.Status;

        await _context.SaveChangesAsync();
        return project;
    }
}
