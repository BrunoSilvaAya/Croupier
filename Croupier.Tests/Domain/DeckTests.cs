using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Domain;

public class DeckTests
{
    [Fact]
    public void CreateDeck_WithSingleDeck_Contains52Cards()
    {
        // Act
        var deck = new DeckBuilder().Build();

        // Assert
        deck.Cards.Should().HaveCount(52);
    }

    [Fact]
    public void CreateDeck_WithMultipleDecks_ContainsCorrectNumberOfCards()
    {
        // Arrange
        const int numberOfDecks = 3;

        // Act
        var deck = new DeckBuilder().WithDeckCount(numberOfDecks).Build();

        // Assert
        deck.Cards.Should().HaveCount(numberOfDecks * 52);
    }

    [Fact]
    public void CreateDeck_WhenShuffled_HasSameCountButDifferentOrder()
    {
        // Arrange
        var regularDeck = new DeckBuilder().WithDeckCount(2).Build();
        
        // Act
        var shuffledDeck = new DeckBuilder().WithDeckCount(2).Shuffled().Build();

        // Assert
        shuffledDeck.Cards.Should().HaveCount(regularDeck.Cards.Count);
        shuffledDeck.Cards.ToList().Should().NotEqual(regularDeck.Cards.ToList());
    }

    [Fact]
    public void CreateDeck_WithSpecificId_HasCorrectDeckId()
    {
        // Arrange
        const int expectedDeckId = 42;

        // Act
        var deck = new DeckBuilder().WithDeckId(expectedDeckId).Build();

        // Assert
        deck.DeckId.Should().Be(expectedDeckId);
    }
}