using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class DrawCardEndpointTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;

    public DrawCardEndpointTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task DrawCard_WhenCalled_ReturnsOkStatus()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        var response = await _client.GetAsync("/draw-card?sessionId=" + sessionId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DrawCard_WhenCalled_ReturnsSingleCard()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        var response = await _client.GetAsync("/draw-card?sessionId=" + sessionId);
        var cardJson = await response.Content.ReadAsStringAsync();
        var card = JsonSerializer.Deserialize<Card>(cardJson);

        card.Should().NotBeNull();
        card!.code.Should().NotBeNullOrEmpty();
        card.value.Should().NotBeNullOrEmpty();
        card.suit.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task DrawCard_WhenCalled_ReturnsTopCardFromDeck()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        // Get deck to see top card
        var deckResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var deckJson = await deckResponse.Content.ReadAsStringAsync();
        var deck = JsonSerializer.Deserialize<List<Card>>(deckJson);

        // Draw card
        var cardResponse = await _client.GetAsync("/draw-card?sessionId=" + sessionId);
        var cardJson = await cardResponse.Content.ReadAsStringAsync();
        var drawnCard = JsonSerializer.Deserialize<Card>(cardJson);

        deck.Should().NotBeNull();
        drawnCard.Should().NotBeNull();
        drawnCard.Should().BeEquivalentTo(deck![0]);
    }

    [Fact]
    public async Task DrawCard_WhenCalled_RemovesCardFromDeck()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        // Get initial deck
        var initialDeckResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var initialDeckJson = await initialDeckResponse.Content.ReadAsStringAsync();
        var initialDeck = JsonSerializer.Deserialize<List<Card>>(initialDeckJson);

        // Draw card
        await _client.GetAsync("/draw-card?sessionId=" + sessionId);

        // Get deck after draw
        var finalDeckResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var finalDeckJson = await finalDeckResponse.Content.ReadAsStringAsync();
        var finalDeck = JsonSerializer.Deserialize<List<Card>>(finalDeckJson);

        initialDeck.Should().NotBeNull();
        finalDeck.Should().NotBeNull();
        initialDeck.Should().HaveCount(52);
        finalDeck.Should().HaveCount(51);
        finalDeck![0].Should().BeEquivalentTo(initialDeck![1]);
    }

    [Fact]
    public async Task DrawCard_WhenAllCardsDrawn_ThrowsException()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .BuildAsync();

        // Draw all 52 cards
        for (int i = 0; i < 52; i++)
        {
            await _client.GetAsync("/draw-card?sessionId=" + sessionId);
        }

        // Try to draw one more card - this should cause an exception
        // Based on the error log, drawing from empty stack throws InvalidOperationException
        var response = await _client.GetAsync("/draw-card?sessionId=" + sessionId);
        
        // The response should indicate a server error due to empty stack
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}