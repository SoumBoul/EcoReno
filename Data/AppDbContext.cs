using EcoReno.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoReno.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Demande> Demandes => Set<Demande>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Demande>().Property(d => d.Status).HasConversion<string>();

        modelBuilder.Entity<Demande>().HasData(
            new Demande
            {
                Id = 1,
                Name = "Rénovation cuisine",
                Owner = "Claire",
                Budget = 8500,
                Description = "Modernisation d'une cuisine éco-responsable",
                Impact = "Faible émission",
                Status = DemandeStatus.EnQualification,
            },
            new Demande
            {
                Id = 2,
                Name = "Isolation toiture",
                Owner = "Julien",
                Budget = 12000,
                Description = "Amélioration thermique d'un logement",
                Impact = "Économie d'énergie",
                Status = DemandeStatus.Planifiee,
            }
        );
    }
}
