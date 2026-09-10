using System.ComponentModel.DataAnnotations;

namespace EcoReno.Models;

public class Project
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom du projet est obligatoire")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom du client est obligatoire")]
    public string Owner { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Le budget doit être positif")]
    public decimal Budget { get; set; }

    public string? Description { get; set; }

    public string Impact { get; set; } = "Impact estimé à calculer";

    public ProjectStatus Status { get; set; } = ProjectStatus.Planned;
}
