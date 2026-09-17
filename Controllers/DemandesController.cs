using EcoReno.Data;
using EcoReno.Models;
using EcoReno.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoReno.Controllers;

[ApiController]
[Route("api/demandes")]
public class DemandesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DemandesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Demande>>> GetDemandes()
    {
        return await _context.Demandes.OrderBy(d => d.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Demande>> GetDemande(int id)
    {
        var demande = await _context.Demandes.FindAsync(id);
        if (demande is null)
        {
            return NotFound(new { error = $"Demande introuvable avec l'id {id}" });
        }
        return demande;
    }

    [HttpPost]
    public async Task<ActionResult<Demande>> CreateDemande(DemandeWriteDto dto)
    {
        var demande = new Demande
        {
            Name = dto.Name,
            Owner = dto.Owner,
            Budget = dto.Budget,
            Description = dto.Description,
            Impact = dto.Impact,
            Status = dto.Status,
        };

        _context.Demandes.Add(demande);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDemande), new { id = demande.Id }, demande);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Demande>> UpdateDemande(int id, DemandeWriteDto dto)
    {
        var demande = await _context.Demandes.FindAsync(id);
        if (demande is null)
        {
            return NotFound(new { error = $"Demande introuvable avec l'id {id}" });
        }

        demande.Name = dto.Name;
        demande.Owner = dto.Owner;
        demande.Budget = dto.Budget;
        demande.Description = dto.Description;
        demande.Impact = dto.Impact;
        demande.Status = dto.Status;

        await _context.SaveChangesAsync();
        return demande;
    }
}
