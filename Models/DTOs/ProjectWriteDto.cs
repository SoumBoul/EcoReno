using System.ComponentModel.DataAnnotations;

namespace EcoReno.Models.Dtos;

/// <summary>
/// Contrat d'entrée pour la création et la mise à jour d'un projet.
/// Volontairement dépourvu de propriété <c>Id</c> : empêche l'over-posting
/// et l'injection de clé primaire (mass assignment).
/// </summary>
public class ProjectWriteDto
{
    [Required(ErrorMessage = "Le nom du projet est obligatoire")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Le nom du projet doit contenir entre 3 et 120 caractères")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom du client est obligatoire")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Le nom du client doit contenir entre 2 et 120 caractères")]
    public string Owner { get; set; } = string.Empty;

    [Range(0.01, 10000000, ErrorMessage = "Le budget doit être compris entre 0,01 et 10 000 000")]
    public decimal Budget { get; set; }

    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "L'impact estimé est obligatoire")]
    [StringLength(300, ErrorMessage = "L'impact estimé ne peut pas dépasser 300 caractères")]
    public string Impact { get; set; } = string.Empty;

    [EnumDataType(typeof(ProjectStatus), ErrorMessage = "Statut de projet invalide")]
    public ProjectStatus Status { get; set; } = ProjectStatus.Planned;
}
