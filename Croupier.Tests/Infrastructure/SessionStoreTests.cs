using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Infrastructure;

public class SessionStoreTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;

    public SessionStoreTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task SessionStore_WhenNewSessionCreated_ShouldPersistSession()
    {
        var sessionId1 = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        var sessionId2 = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        sessionId1.Should().NotBeNullOrEmpty();
        sessionId2.Should().NotBeNullOrEmpty();
        sessionId1.Should().NotBe(sessionId2, "each session should have a unique ID");
    }

    [Fact]
    public async Task SessionStore_WhenSessionExists_ShouldMaintainStateAcrossOperations()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        // Draw a card
        var drawResponse = await _client.GetAsync($"/draw-card?sessionId={sessionId}");
        drawResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // Verify the session still exists and deck was modified
        var deckResponse = await _client.GetAsync($"/see-deck?sessionId={sessionId}");
        deckResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var deckJson = await deckResponse.Content.ReadAsStringAsync();
        var deck = System.Text.Json.JsonSerializer.Deserialize<List<Card>>(deckJson);

        deck.Should().HaveCount(51, "one card should have been drawn");
    }

    [Fact]
    public async Task SessionStore_WithMultipleSessions_ShouldIsolateSessionData()
    {
        var sessionId1 = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        var sessionId2 = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(2)
            .BuildAsync();

        // Draw card from first session
        await _client.GetAsync($"/draw-card?sessionId={sessionId1}");

        // Check both sessions
        var deck1Response = await _client.GetAsync($"/see-deck?sessionId={sessionId1}");
        var deck2Response = await _client.GetAsync($"/see-deck?sessionId={sessionId2}");

        var deck1Json = await deck1Response.Content.ReadAsStringAsync();
        var deck2Json = await deck2Response.Content.ReadAsStringAsync();

        var deck1 = System.Text.Json.JsonSerializer.Deserialize<List<Card>>(deck1Json);
        var deck2 = System.Text.Json.JsonSerializer.Deserialize<List<Card>>(deck2Json);

        deck1.Should().HaveCount(51, "session 1 should have 51 cards after drawing one");
        deck2.Should().HaveCount(104, "session 2 should have full 2-deck (104 cards)");
    }
}