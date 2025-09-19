using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using FluentAssertions;
using Xunit;
using TestCard = Croupier.Tests.Card;

namespace Croupier.Tests.TestModel;

public abstract class BaseEndpointTest : IDisposable
{
    protected readonly WebApplicationFactory<Program> App;
    protected readonly HttpClient Client;

    protected BaseEndpointTest()
    {
        App = new WebApplicationFactory<Program>();
        Client = App.CreateClient();
    }

    protected async Task<string> CreateNewGameSession(int? numberOfDecks = null)
    {
        var url = numberOfDecks.HasValue ? $"/new-game?numberOfDecks={numberOfDecks}" : "/new-game";
        return await Client.GetStringAsync(url);
    }

    protected async Task<List<TestCard>?> GetDeckForSession(string sessionId)
    {
        var response = await Client.GetAsync($"/see-deck?sessionId={sessionId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TestCard>>(content);
    }

    protected async Task<TestCard?> DrawCardFromSession(string sessionId)
    {
        var response = await Client.GetAsync($"/draw-card?sessionId={sessionId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TestCard>(content);
    }

    protected async Task<List<TestCard>?> ShuffleDeckForSession(string sessionId)
    {
        var response = await Client.GetAsync($"/shuffle-deck?sessionId={sessionId}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TestCard>>(content);
    }

    public void Dispose()
    {
        Client?.Dispose();
        App?.Dispose();
    }
}