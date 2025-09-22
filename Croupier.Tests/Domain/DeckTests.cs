using Xunit;
using FluentAssertions;
using Croupier.Tests.TestModel;

namespace Croupier.Tests.Domain;

public class DeckTests
{
    [Fact]
    public void Deck_WhenBuiltWithSingleDeck_ShouldContain52Cards()
    {
        var deck = new DeckBuilder().Build();

        deck.Should().HaveCount(52);
    }

    [Fact]
    public void Deck_WhenBuiltWithMultipleDecks_ShouldContainCorrectNumberOfCards()
    {
        var deck = new DeckBuilder()
            .WithDeckCount(3)
            .Build();

        deck.Should().HaveCount(3 * 52);
    }

    [Fact]
    public void Deck_WhenBuilt_ShouldContainAllStandardCards()
    {
        var deck = new DeckBuilder().Build();

        deck.Should().HaveCount(52);
        
        // Check we have all suits
        deck.Should().Contain(c => c.suit == "SPADES");
        deck.Should().Contain(c => c.suit == "HEARTS");
        deck.Should().Contain(c => c.suit == "CLUBS");
        deck.Should().Contain(c => c.suit == "DIAMONDS");
        
        // Check we have all values
        deck.Should().Contain(c => c.value == "ACE");
        deck.Should().Contain(c => c.value == "KING");
        deck.Should().Contain(c => c.value == "QUEEN");
        deck.Should().Contain(c => c.value == "JACK");
        deck.Should().Contain(c => c.value == "10");
        deck.Should().Contain(c => c.value == "2");
    }

    [Fact]
    public void Deck_WhenShuffled_ShouldHaveDifferentOrder()
    {
        var originalDeck = new DeckBuilder().Build();
        var shuffledDeck = new DeckBuilder().Shuffled().Build();

        originalDeck.Should().HaveCount(shuffledDeck.Count);
        originalDeck.Should().NotEqual(shuffledDeck, "shuffled deck should have different order");
    }

    [Fact]
    public void Deck_WhenShuffled_ShouldPreserveAllCards()
    {
        var originalDeck = new DeckBuilder().Build();
        var shuffledDeck = new DeckBuilder().Shuffled().Build();

        shuffledDeck.Should().HaveCount(originalDeck.Count);
        
        // Every card in original should exist in shuffled
        foreach (var card in originalDeck)
        {
            shuffledDeck.Should().Contain(c => 
                c.code == card.code && c.value == card.value && c.suit == card.suit,
                $"shuffled deck should contain card {card.code}");
        }
    }

    [Fact]
    public void Deck_WithMultipleDecks_ShouldHaveDuplicateCards()
    {
        var deck = new DeckBuilder()
            .WithDeckCount(2)
            .Build();

        deck.Should().HaveCount(2 * 52);
        
        // Should have duplicates of each card
        var aceOfSpades = deck.Where(c => c.code == "AS").ToList();
        aceOfSpades.Should().HaveCount(2, "should have 2 Aces of Spades in a 2-deck setup");
    }
}