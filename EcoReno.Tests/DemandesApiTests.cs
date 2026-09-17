using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace EcoReno.Tests;

public class DemandesApiTests : IClassFixture<EcoRenoWebApplicationFactory>
{
    private static readonly string[] LifecycleStatuses =
    {
        "Recue", "EnQualification", "DevisEnvoye", "Planifiee", "Terminee", "Refusee"
    };

    private readonly HttpClient _client;

    public DemandesApiTests(EcoRenoWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ShouldListSeededDemandes()
    {
        var response = await _client.GetAsync("/api/demandes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var demandes = await response.Content.ReadFromJsonAsync<List<JsonElement>>();
        Assert.NotNull(demandes);

        var names = demandes.Select(d => d.GetProperty("name").GetString()).ToList();
        Assert.Contains("Rénovation cuisine", names);
        Assert.Contains("Isolation toiture", names);

        // Les statuts du seed doivent appartenir au cycle de vie DAT.
        var statusByName = demandes.ToDictionary(
            d => d.GetProperty("name").GetString()!,
            d => d.GetProperty("status").GetString()!);

        Assert.Equal("EnQualification", statusByName["Rénovation cuisine"]);
        Assert.Equal("Planifiee", statusByName["Isolation toiture"]);
        Assert.All(statusByName.Values, s => Assert.Contains(s, LifecycleStatuses));
    }

    [Fact]
    public async Task ShouldCreateDemande()
    {
        var payload = new
        {
            name = "Isolation double vitrage",
            owner = "Sophie",
            budget = 6000,
            description = "Travaux complémentaires",
            impact = "Très bon",
            status = "Recue",
        };

        var response = await _client.PostAsJsonAsync("/api/demandes", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Isolation double vitrage", created.GetProperty("name").GetString());
        Assert.Equal("Recue", created.GetProperty("status").GetString());
    }

    [Fact]
    public async Task ShouldReturnNotFoundForUnknownDemande()
    {
        var response = await _client.GetAsync("/api/demandes/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ShouldIgnoreClientSuppliedIdOnCreate()
    {
        // Tentative de mass assignment : "id" collisionne avec une ligne du seed.
        var payload = new
        {
            id = 1,
            name = "Chaudière thermique",
            owner = "Nadia",
            budget = 4200,
            description = "Remplacement",
            impact = "Bon",
            status = "Recue",
        };

        var response = await _client.PostAsJsonAsync("/api/demandes", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetInt32();
        Assert.NotEqual(1, id);
        Assert.NotEqual(2, id);
    }

    [Fact]
    public async Task ShouldRejectInvalidPayload()
    {
        var payload = new
        {
            name = "",
            owner = "X",
            budget = 0,
            impact = "",
            status = "Recue",
        };

        var response = await _client.PostAsJsonAsync("/api/demandes", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ShouldRejectUnknownStatus()
    {
        var payload = new
        {
            name = "Statut inconnu",
            owner = "Zoe",
            budget = 1500,
            impact = "Neutre",
            status = "PasUnStatut",
        };

        var response = await _client.PostAsJsonAsync("/api/demandes", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("Recue")]
    [InlineData("EnQualification")]
    [InlineData("DevisEnvoye")]
    [InlineData("Planifiee")]
    [InlineData("Terminee")]
    [InlineData("Refusee")]
    public async Task ShouldAcceptEveryLifecycleStatus(string status)
    {
        var payload = new
        {
            name = $"Demande {status}",
            owner = "Alex",
            budget = 1500,
            description = "Vérification du cycle de vie",
            impact = "Neutre",
            status,
        };

        var response = await _client.PostAsJsonAsync("/api/demandes", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(status, created.GetProperty("status").GetString());
    }
}
