using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;

namespace Croupier.Tests;

public class CroupierTests
{
    private readonly WebApplicationFactory<Program> _app;
    private readonly HttpClient _client;
    public CroupierTests()
    {
        _app = new WebApplicationFactory<Program>();
        _client = _app.CreateClient();
    }
    [Fact]
    public async Task NewGameReturnsId()
    {
        Assert.IsType<string>(await _client.GetStringAsync("/new-game"));
    }
    [Fact]
    public async Task NewGameIdLengthEqualsEight()
    {
        var id = await _client.GetStringAsync("/new-game");
        Assert.True(id.Length > 5 && id.Length < 10);
    }
    [Fact]
    public async Task NormalDeckContains52Cards()
    {
        var id = await _client.GetStringAsync("/new-game");
        var result = await _client.GetAsync("/see-deck?sessionId=" + id);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var deck = await result.Content.ReadAsStringAsync();
        Assert.Equal(52, JsonSerializer.Deserialize<List<Card>>(deck)?.Count);
    }
    [Fact]
    public async Task MultipleDeckContainsCorrectNumberOfCards()
    {
        var id = await _client.GetStringAsync("/new-game?numberOfDecks=3");
        var result = await _client.GetAsync("/see-deck?sessionId=" + id);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var deck = await result.Content.ReadAsStringAsync();
        Assert.Equal(3 * 52, JsonSerializer.Deserialize<List<Card>>(deck)?.Count);
    }

    [Fact]
    public async Task ShufflerReallyShuffles()
    {
        var id = await _client.GetStringAsync("/new-game?numberOfDecks=3");
        var result = await _client.GetAsync("/see-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var deckJson = await result.Content.ReadAsStringAsync();
        var deck = JsonSerializer.Deserialize<List<Card>>(deckJson);

        var shuffledResponse = await _client.GetAsync("/shuffle-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var shuffledDeckJson = await shuffledResponse.Content.ReadAsStringAsync();
        var shuffledDeck = JsonSerializer.Deserialize<List<Card>>(shuffledDeckJson);

        Assert.NotNull(deck);
        Assert.NotNull(shuffledDeck);
        Assert.Equal(deck.Count, shuffledDeck.Count);

        Assert.False(deck.All(shuffledDeck.Contains));
    }
    [Fact]
    public async Task DrawCardDrawsOnlyOneCard()
    {
        var id = await _client.GetStringAsync("/new-game");
        var result = await _client.GetAsync("/draw-card?sessionId=" + id);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var deck = await result.Content.ReadAsStringAsync();
        Assert.IsType<Card>(JsonSerializer.Deserialize<Card>(deck));
    }

    //TODO: refactor querystring to become a route param

    //test drawing returns the card on the top of pile
    //test drawing until empty
}
