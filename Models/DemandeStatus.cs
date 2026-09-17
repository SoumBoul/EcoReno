namespace EcoReno.Models;

/// <summary>
/// Cycle de vie d'une demande de rénovation (spécification EcoReno DAT) :
/// Recue -> EnQualification -> DevisEnvoye -> Planifiee -> Terminee, ou Refusee.
/// </summary>
public enum DemandeStatus
{
    /// <summary>Demande reçue, pas encore prise en charge.</summary>
    Recue,

    /// <summary>Qualification en cours (recevabilité, périmètre, budget).</summary>
    EnQualification,

    /// <summary>Devis transmis au client, en attente de sa décision.</summary>
    DevisEnvoye,

    /// <summary>Intervention planifiée.</summary>
    Planifiee,

    /// <summary>Travaux terminés (état final nominal).</summary>
    Terminee,

    /// <summary>Demande refusée (état final alternatif).</summary>
    Refusee
}
