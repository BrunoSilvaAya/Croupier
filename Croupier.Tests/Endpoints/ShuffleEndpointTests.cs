using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Endpoints;

public class ShuffleEndpointTests : BaseEndpointTest
{
    [Fact]
    public async Task ShuffleDeck_WhenCalled_ChangesCardOrder()
    {
        // Arrange
        var sessionId = await CreateNewGameSession(3);
        var originalDeck = await GetDeckForSession(sessionId);

        // Act
        var shuffledDeck = await ShuffleDeckForSession(sessionId);

        // Assert
        originalDeck.Should().NotBeNull();
        shuffledDeck.Should().NotBeNull();
        shuffledDeck.Should().HaveCount(originalDeck.Count);
        shuffledDeck.Should().NotEqual(originalDeck);
    }
}