using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class SeeDeckEndpointTests : BaseEndpointTest
{
    [Fact]
    public async Task SeeDeck_WithSingleDeck_ContainsExactly52Cards()
    {
        // Arrange
        var sessionId = await CreateNewGameSession();

        // Act
        var deck = await GetDeckForSession(sessionId);

        // Assert
        deck.Should().NotBeNull();
        deck.Should().HaveCount(52);
    }

    [Fact]
    public async Task SeeDeck_WithMultipleDecks_ContainsCorrectNumberOfCards()
    {
        // Arrange
        const int numberOfDecks = 3;
        var sessionId = await CreateNewGameSession(numberOfDecks);

        // Act
        var deck = await GetDeckForSession(sessionId);

        // Assert
        deck.Should().NotBeNull();
        deck.Should().HaveCount(numberOfDecks * 52);
    }
}