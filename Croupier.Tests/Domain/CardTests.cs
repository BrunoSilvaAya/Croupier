using Xunit;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Domain;

public class CardTests
{
    [Fact]
    public void Card_WhenCreatedWithBuilder_ShouldHaveCorrectProperties()
    {
        var card = new CardBuilder()
            .WithValue("KING")
            .WithSuit("HEARTS")
            .Build();

        card.value.Should().Be("KING");
        card.suit.Should().Be("HEARTS");
        card.code.Should().Be("KH");
    }

    [Fact]
    public void Card_WithAceOfSpades_ShouldHaveCorrectCode()
    {
        var card = new CardBuilder()
            .WithAce()
            .WithSpades()
            .Build();

        card.code.Should().Be("AS");
        card.value.Should().Be("ACE");
        card.suit.Should().Be("SPADES");
    }

    [Fact]
    public void Card_With10Value_ShouldHaveCorrectCode()
    {
        var card = new CardBuilder()
            .WithValue("10")
            .WithDiamonds()
            .Build();

        card.code.Should().Be("10D");
    }

    [Theory]
    [InlineData("ACE", "SPADES", "AS")]
    [InlineData("2", "HEARTS", "2H")]
    [InlineData("10", "CLUBS", "10C")]
    [InlineData("KING", "DIAMONDS", "KD")]
    [InlineData("QUEEN", "SPADES", "QS")]
    [InlineData("JACK", "HEARTS", "JH")]
    public void Card_WithDifferentValues_ShouldGenerateCorrectCode(string value, string suit, string expectedCode)
    {
        var card = new CardBuilder()
            .WithValue(value)
            .WithSuit(suit)
            .Build();

        card.code.Should().Be(expectedCode);
    }
}