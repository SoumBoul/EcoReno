using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace EcoReno.Tests;

public class ProjectsApiTests : IClassFixture<EcoRenoWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsApiTests(EcoRenoWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ShouldListSeededProjects()
    {
        var response = await _client.GetAsync("/api/projects");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var projects = await response.Content.ReadFromJsonAsync<List<JsonElement>>();
        var names = projects!.Select(p => p.GetProperty("name").GetString()).ToList();
        Assert.Contains("Rénovation cuisine", names);
        Assert.Contains("Isolation toiture", names);
    }

    [Fact]
    public async Task ShouldCreateProject()
    {
        var payload = new
        {
            name = "Isolation double vitrage",
            owner = "Sophie",
            budget = 6000,
            description = "Travaux complémentaires",
            impact = "Très bon",
            status = "Planned",
        };

        var response = await _client.PostAsJsonAsync("/api/projects", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Isolation double vitrage", created.GetProperty("name").GetString());
    }

    [Fact]
    public async Task ShouldReturnNotFoundForUnknownProject()
    {
        var response = await _client.GetAsync("/api/projects/999");

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
            status = "Planned",
        };

        var response = await _client.PostAsJsonAsync("/api/projects", payload);

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
            status = "Planned",
        };

        var response = await _client.PostAsJsonAsync("/api/projects", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

