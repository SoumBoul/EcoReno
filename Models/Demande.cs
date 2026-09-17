using System.ComponentModel.DataAnnotations;

namespace EcoReno.Models;

public class Demande
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de la demande est obligatoire")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom du client est obligatoire")]
    public string Owner { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Le budget doit être positif")]
    public decimal Budget { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "L'impact estimé est obligatoire")]
    public string Impact { get; set; } = "Impact estimé à calculer";

    public DemandeStatus Status { get; set; } = DemandeStatus.Recue;
}
