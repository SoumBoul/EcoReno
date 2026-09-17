# EcoReno

Application de gestion de demandes de rénovation écologique : API REST (ASP.NET Core) et interface web simple pour suivre les demandes d'un client (nom, budget, description, impact estimé, statut).

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

L'application est disponible sur http://localhost:5110/ et l'API sur http://localhost:5110/api/demandes.

## Tests

```bash
dotnet test
```

## Stack technique

- .NET 10 + ASP.NET Core Web API
- Entity Framework Core + SQLite

## Structure du projet

- `Models/` : modèles (`Demande`, `DemandeStatus`)
- `Data/` : `AppDbContext` (EF Core)
- `Controllers/` : contrôleurs REST
- `Migrations/` : migrations EF Core
- `wwwroot/` : interface web
- `EcoReno.Tests/` : tests d'intégration

## API Endpoints

| Méthode | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/demandes` | Liste des demandes |
| POST | `/api/demandes` | Création d'une demande |
| GET | `/api/demandes/{id}` | Détail d'une demande |
| PUT | `/api/demandes/{id}` | Mise à jour d'une demande |

## Licence

Usage interne.
