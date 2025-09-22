// LEGACY: This file has been superseded by the organized test structure:
// - Endpoints/NewGameEndpointTests.cs
// - Endpoints/SeeDeckEndpointTests.cs  
// - Endpoints/DrawCardEndpointTests.cs
// - Endpoints/ShuffleEndpointTests.cs
// - Domain/CardTests.cs
// - Domain/DeckTests.cs  
// - Infrastructure/SessionStoreTests.cs
// Consider removing this file after confirming all functionality is covered.

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

        var deck = JsonSerializer.Deserialize<List<Card>>(
            await result.Content.ReadAsStringAsync()
        );

        var shuffledResponse = await _client.GetAsync("/shuffle-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var shuffledDeck = JsonSerializer.Deserialize<List<Card>>(
            await shuffledResponse.Content.ReadAsStringAsync()
        );

        Assert.NotNull(deck);
        Assert.NotNull(shuffledDeck);
        Assert.Equal(deck.Count, shuffledDeck.Count);

        Assert.False(deck.SequenceEqual(shuffledDeck));
    }
    [Fact]
    public async Task DrawCardDrawsOnlyOneCard()
    {
        var id = await _client.GetStringAsync("/new-game");
        var result = await _client.GetAsync("/draw-card?sessionId=" + id);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        Assert.IsType<Card>(JsonSerializer.Deserialize<Card>(
            await result.Content.ReadAsStringAsync()
        ));
    }
    [Fact]
    public async Task DrawCardDrawsCardOnTopOfDeck()
    {
        var id = await _client.GetStringAsync("/new-game?numberOfDecks=1");

        var result = await _client.GetAsync("/see-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var deck = JsonSerializer.Deserialize<List<Card>>(
            await result.Content.ReadAsStringAsync()
        );

        var cardResult = await _client.GetAsync("/draw-card?sessionId=" + id);
        var card = JsonSerializer.Deserialize<Card>(
            await cardResult.Content.ReadAsStringAsync()
        );

        Assert.Equal(deck[0], card);
    }
    [Fact]
    public async Task DrawCardRemovesCardFromDeck()
    {
        var id = await _client.GetStringAsync("/new-game?numberOfDecks=1");

        var result = await _client.GetAsync("/see-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var deck = JsonSerializer.Deserialize<List<Card>>(
            await result.Content.ReadAsStringAsync()
        );

        await _client.GetAsync("/draw-card?sessionId=" + id);

        var resultAfterDraw = await _client.GetAsync("/see-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, resultAfterDraw.StatusCode);

        var deckAfterDraw = JsonSerializer.Deserialize<List<Card>>(
            await resultAfterDraw.Content.ReadAsStringAsync()
        );

        Assert.NotNull(deck);
        Assert.NotNull(deckAfterDraw);
        Assert.Equal(deck[1], deckAfterDraw[0]);
    }
    [Fact]
    public async Task DrawingAllCardsEmptiesDeck()
    {
        var id = await _client.GetStringAsync("/new-game?numberOfDecks=1");

        Enumerable.Range(1, 52).ToList().ForEach(e =>
        {
            _client.GetAsync("/draw-card?sessionId=" + id);
        });

        var voidDraw = await _client.GetAsync("/draw-card?sessionId=" + id);
        var card = JsonSerializer.Deserialize<Card>(
            await voidDraw.Content.ReadAsStringAsync()
        );

        var result = await _client.GetAsync("/see-deck?sessionId=" + id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var deck = await result.Content.ReadAsStringAsync();
        Console.WriteLine(deck);
        Assert.Null(card?.code);
    }
    //TODO: refactor querystring to become a route param
}
