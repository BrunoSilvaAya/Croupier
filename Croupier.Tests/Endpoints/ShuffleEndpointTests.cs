using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class ShuffleEndpointTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;

    public ShuffleEndpointTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task ShuffleDeck_WhenCalled_ReturnsOkStatus()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(3)
            .BuildAsync();

        var response = await _client.GetAsync("/shuffle-deck?sessionId=" + sessionId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ShuffleDeck_WhenCalled_ChangesCardOrder()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(3)
            .BuildAsync();

        // Get original deck
        var originalResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var originalDeckJson = await originalResponse.Content.ReadAsStringAsync();
        var originalDeck = JsonSerializer.Deserialize<List<Card>>(originalDeckJson);

        // Shuffle deck
        var shuffleResponse = await _client.GetAsync("/shuffle-deck?sessionId=" + sessionId);

        // Get shuffled deck
        var shuffledDeckJson = await shuffleResponse.Content.ReadAsStringAsync();
        var shuffledDeck = JsonSerializer.Deserialize<List<Card>>(shuffledDeckJson);

        originalDeck.Should().NotBeNull();
        shuffledDeck.Should().NotBeNull();
        originalDeck.Should().HaveCount(shuffledDeck!.Count);
        
        // The deck should be shuffled (order changed)
        originalDeck.Should().NotEqual(shuffledDeck, "the deck should be shuffled");
    }

    [Fact] 
    public async Task ShuffleDeck_WhenCalled_PreservesAllCards()
    {
        var sessionId = await new TestSessionBuilder()
            .WithHttpClient(_client)
            .WithNumberOfDecks(2)
            .BuildAsync();

        // Get original deck
        var originalResponse = await _client.GetAsync("/see-deck?sessionId=" + sessionId);
        var originalDeckJson = await originalResponse.Content.ReadAsStringAsync();
        var originalDeck = JsonSerializer.Deserialize<List<Card>>(originalDeckJson);

        // Shuffle deck
        var shuffleResponse = await _client.GetAsync("/shuffle-deck?sessionId=" + sessionId);
        var shuffledDeckJson = await shuffleResponse.Content.ReadAsStringAsync();
        var shuffledDeck = JsonSerializer.Deserialize<List<Card>>(shuffledDeckJson);

        originalDeck.Should().NotBeNull();
        shuffledDeck.Should().NotBeNull();
        
        // Should have same number of cards
        shuffledDeck.Should().HaveCount(originalDeck!.Count);
        
        // Should contain all the same cards (just in different order)
        foreach (var card in originalDeck)
        {
            shuffledDeck.Should().Contain(c => 
                c.code == card.code && c.value == card.value && c.suit == card.suit,
                $"shuffled deck should contain card {card.code}");
        }
    }
}