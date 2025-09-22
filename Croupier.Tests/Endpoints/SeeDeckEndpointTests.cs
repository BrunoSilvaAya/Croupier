using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class SeeDeckEndpointTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;

    public SeeDeckEndpointTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task SeeDeck_WithSingleDeck_Returns52Cards()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        var response = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var deckJson = await response.Content.ReadAsStringAsync();
        var deck = JsonSerializer.Deserialize<List<Card>>(deckJson);
        
        deck.Should().HaveCount(52);
    }

    [Fact]
    public async Task SeeDeck_WithMultipleDecks_ReturnsCorrectNumberOfCards()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(3)
            .BuildAsync();

        var response = await _client.GetAsync("/see-deck?sessionId=" + sessionId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var deckJson = await response.Content.ReadAsStringAsync();
        var deck = JsonSerializer.Deserialize<List<Card>>(deckJson);
        
        deck.Should().HaveCount(3 * 52);
    }

    [Fact]
    public async Task SeeDeck_AfterDrawingCard_ReturnsFewerCards()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        // Get initial deck
        var initialResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var initialDeckJson = await initialResponse.Content.ReadAsStringAsync();
        var initialDeck = JsonSerializer.Deserialize<List<Card>>(initialDeckJson);

        // Draw a card
        await _client.GetAsync("/draw-card?sessionId=" + sessionId);

        // Check deck after draw
        var finalResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var finalDeckJson = await finalResponse.Content.ReadAsStringAsync();
        var finalDeck = JsonSerializer.Deserialize<List<Card>>(finalDeckJson);

        initialDeck.Should().HaveCount(52);
        finalDeck.Should().HaveCount(51);
        finalDeck.Should().NotBeNull();
        initialDeck.Should().NotBeNull();
        finalDeck![0].Should().BeEquivalentTo(initialDeck![1]);
    }
}