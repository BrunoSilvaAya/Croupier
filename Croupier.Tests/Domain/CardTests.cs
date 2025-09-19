using FluentAssertions;
using Xunit;
using Croupier.Tests.TestModel;
using DomainCard = Croupier.Model.Card;

namespace Croupier.Tests.Domain;

public class CardTests
{
    [Fact]
    public void CreateCard_WithAceOfSpades_GeneratesCorrectCode()
    {
        // Act
        var card = new CardBuilder().AsAceOfSpades().Build();

        // Assert
        card.Code.Should().Be("AS");
        card.Value.Should().Be("ACE");
        card.Suit.Should().Be("SPADES");
    }

    [Fact]
    public void CreateCard_WithKingOfHearts_GeneratesCorrectCode()
    {
        // Act
        var card = new CardBuilder().AsKingOfHearts().Build();

        // Assert
        card.Code.Should().Be("KH");
        card.Value.Should().Be("KING");
        card.Suit.Should().Be("HEARTS");
    }

    [Fact]
    public void CreateCard_WithTenValue_GeneratesCorrectCode()
    {
        // Act
        var card = new CardBuilder().WithValue("10").WithSuit("CLUBS").Build();

        // Assert
        card.Code.Should().Be("10C");
        card.Value.Should().Be("10");
        card.Suit.Should().Be("CLUBS");
    }

    [Fact]
    public void CreateCard_WithCustomValues_GeneratesExpectedProperties()
    {
        // Arrange
        const string expectedValue = "JACK";
        const string expectedSuit = "DIAMONDS";

        // Act
        var card = new CardBuilder()
            .WithValue(expectedValue)
            .WithSuit(expectedSuit)
            .Build();

        // Assert
        card.Value.Should().Be(expectedValue);
        card.Suit.Should().Be(expectedSuit);
        card.Code.Should().Be("JD");
    }
}