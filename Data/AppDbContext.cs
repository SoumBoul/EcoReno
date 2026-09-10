using EcoReno.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoReno.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().Property(p => p.Status).HasConversion<string>();

        modelBuilder.Entity<Project>().HasData(
            new Project
            {
                Id = 1,
                Name = "Rénovation cuisine",
                Owner = "Claire",
                Budget = 8500,
                Description = "Modernisation d'une cuisine éco-responsable",
                Impact = "Faible émission",
                Status = ProjectStatus.InProgress,
            },
            new Project
            {
                Id = 2,
                Name = "Isolation toiture",
                Owner = "Julien",
                Budget = 12000,
                Description = "Amélioration thermique d'un logement",
                Impact = "Économie d'énergie",
                Status = ProjectStatus.Planned,
            }
        );
    }
}
