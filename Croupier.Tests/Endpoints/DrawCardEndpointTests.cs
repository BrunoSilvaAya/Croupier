using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;
using TestCard = Croupier.Tests.Card;

namespace Croupier.Tests.Endpoints;

public class DrawCardEndpointTests : BaseEndpointTest
{
    [Fact]
    public async Task DrawCard_WhenCalled_ReturnsExactlyOneCard()
    {
        // Arrange
        var sessionId = await CreateNewGameSession();

        // Act
        var card = await DrawCardFromSession(sessionId);

        // Assert
        card.Should().NotBeNull();
        card.Should().BeOfType<TestCard>();
    }

    [Fact]
    public async Task DrawCard_WhenCalled_ReturnsTopCardFromDeck()
    {
        // Arrange
        var sessionId = await CreateNewGameSession(1);
        var deck = await GetDeckForSession(sessionId);

        // Act
        var drawnCard = await DrawCardFromSession(sessionId);

        // Assert
        drawnCard.Should().NotBeNull();
        drawnCard.Should().Be(deck![0]);
    }

    [Fact]
    public async Task DrawCard_WhenCalled_RemovesCardFromDeck()
    {
        // Arrange
        var sessionId = await CreateNewGameSession(1);
        var originalDeck = await GetDeckForSession(sessionId);

        // Act
        await DrawCardFromSession(sessionId);
        var deckAfterDraw = await GetDeckForSession(sessionId);

        // Assert
        originalDeck.Should().NotBeNull();
        deckAfterDraw.Should().NotBeNull();
        deckAfterDraw.Should().HaveCount(originalDeck.Count - 1);
        deckAfterDraw![0].Should().Be(originalDeck![1]);
    }

    [Fact]
    public async Task DrawCard_WhenAllCardsDrawn_EmptiesDeck()
    {
        // Arrange
        var sessionId = await CreateNewGameSession(1);

        // Act - Draw all 52 cards
        for (int i = 0; i < 52; i++)
        {
            await Client.GetAsync($"/draw-card?sessionId={sessionId}");
        }
        
        // Try to draw one more card - this will fail with 500 error due to empty stack
        var extraCardResponse = await Client.GetAsync($"/draw-card?sessionId={sessionId}");
        var finalDeck = await GetDeckForSession(sessionId);

        // Assert - we expect a server error when trying to draw from empty deck
        extraCardResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        finalDeck.Should().BeEmpty();
    }
}