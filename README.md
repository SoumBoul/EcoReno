# EcoReno

Application de gestion de projets de rénovation écologique : API REST (ASP.NET Core) et interface web simple pour suivre les projets d'un client (nom, budget, description, impact estimé, statut).

## Prérequis

- .NET SDK 10+

## Installation

```bash
dotnet restore
dotnet ef database update
```

## Lancement

```bash
dotnet run
```

L'application est disponible sur http://localhost:5110/ et l'API sur http://localhost:5110/api/projects.

## Tests

```bash
dotnet test
```

## Stack technique

- .NET 10 + ASP.NET Core Web API
- Entity Framework Core + SQLite

## Structure du projet

- `Models/` : modèles (`Project`, `ProjectStatus`)
- `Data/` : `AppDbContext` (EF Core)
- `Controllers/` : contrôleurs REST
- `Migrations/` : migrations EF Core
- `wwwroot/` : interface web
- `EcoReno.Tests/` : tests d'intégration

## API Endpoints

| Méthode | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/projects` | Liste des projets |
| POST | `/api/projects` | Création d'un projet |
| GET | `/api/projects/{id}` | Détail d'un projet |
| PUT | `/api/projects/{id}` | Mise à jour d'un projet |

## Licence

Usage interne.
